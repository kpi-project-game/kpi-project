using System.Text.Json;
using Lib.Core.BaseClasses;
using Lib.Core.Enums;
using Lib.Core.Factories;
using Lib.Core.Models;
using Lib.Core.Models.StatesAndEffects;
using Lib.Infrastructure.Database;
using Lib.Infrastructure.Database.Repositories;
using Serilog;

namespace Lib.Infrastructure.Services;

public enum BattleResult
{
    Ongoing,
    Victory,
    Defeat
}

public class BattleRuler
{
    private readonly CharacterRepository _charRepo;
    private readonly ActiveBattleRepository _battleRepo;
    private readonly RoomRepository _roomRepo;
    private readonly Random _rand = new Random();
    
    public BattleRuler(CharacterRepository charRepo, ActiveBattleRepository battleRepo, RoomRepository roomRepo)
    {
        _charRepo = charRepo;
        _battleRepo = battleRepo;
        _roomRepo = roomRepo;
    }

    public (string Message, List<EnemyBattleData> Enemies) GetBattleState(long telegramId)
    {
        var hero = _charRepo.GetActiveCharacter(telegramId);
        if (hero == null)
        {
            Log.Warning("Attempted to get battle state for a non-existent character. TelegramId: {TelegramId}", telegramId);
            return ("Character not found.", new());
        }

        var battleData = _battleRepo.GetBattle(hero.Id);
        if (battleData == null)
        {
            Log.Warning("Battle not found for hero {HeroId}", hero.Id);
            return ("Battle not found.", new());
        }

        var enemies = JsonSerializer.Deserialize<List<EnemyBattleData>>(battleData.Value.EnemiesJson) ?? new();
        hero.CurrentEffects = JsonSerializer.Deserialize<List<ActiveEffect>>(battleData.Value.HeroEffectsJson) ?? new();

        int effectiveMaxHp = GetEffectiveMaxHp(hero);

        string effectsInfo = hero.CurrentEffects.Count > 0
            ? "\n⚡ Effects: " + string.Join(", ", hero.CurrentEffects.Select(e => $"{e.BattleStateEnum}({e.TurnsLeft})"))
            : "";

        string enemyList = string.Join("\n", enemies.Select((e, i) => $"{i + 1}. {e.Name} ❤️ {e.Hp}/{e.MaxHp}"));

        string msg = $"⚔️ Battle!\n" +
                     $"❤️ HP: {hero.Hp}/{effectiveMaxHp} | ⌛ Turns: {hero.TurnsLeft}{effectsInfo}\n\n" +
                     $"{enemyList}\n\n" +
                     $"Choose your action:";

        return (msg, enemies);
    }

    public (string Message, BattleResult Result) ProcessAttack(long telegramId, int enemyIndex)
    {
        var hero = _charRepo.GetActiveCharacter(telegramId);
        if (hero == null) return ("Character not found.", BattleResult.Defeat);
        if (hero.State != 1) return ("You are not in battle.", BattleResult.Ongoing);

        var battleData = _battleRepo.GetBattle(hero.Id);
        if (battleData == null)
        {
            _charRepo.UpdateCharacterState(hero.Id, 0);
            return ("Error: battle not found.", BattleResult.Ongoing);
        }

        var enemies = JsonSerializer.Deserialize<List<EnemyBattleData>>(battleData.Value.EnemiesJson) ?? new();
        hero.CurrentEffects = JsonSerializer.Deserialize<List<ActiveEffect>>(battleData.Value.HeroEffectsJson) ?? new();

        int effectiveMaxHp = GetEffectiveMaxHp(hero);
        if (hero.Hp > effectiveMaxHp)
        {
            hero.Hp = effectiveMaxHp;
            _charRepo.UpdateCharacterStats(hero);
        }

        if (enemyIndex < 0 || enemyIndex >= enemies.Count)
            return ("Invalid target.", BattleResult.Ongoing);

        var target = enemies[enemyIndex];
        string result;
        bool isFrightened = hero.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Frightened);

        if (isFrightened)
        {
            result = $"😨 You are frightened and skip your turn!";
        }
        else if (hero.HasJokerEffect && _rand.Next(1, 55) == 1)
        {
            result = $"🃏 The Joker smiles... {target.Name} instantly defeated!";
            enemies.RemoveAt(enemyIndex);
        }
        else
        {
            int currentDmg = hero.HandDmg;
            if (hero.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Poisoned))
            {
                currentDmg = Math.Max(0, currentDmg - 1);
            }

            int dmgToEnemy = Math.Max(0, currentDmg - GetEnemyDefense(target.ClassType));
            target.Hp = Math.Max(0, target.Hp - dmgToEnemy);
            result = $"⚔️ You attacked {target.Name} for {dmgToEnemy} damage.";

            if (target.Hp <= 0)
            {
                enemies.RemoveAt(enemyIndex);
                result += $"\n💀 {target.Name} has been defeated!";
            }
        }

        return FinalizeTurn(hero, enemies, result, effectiveMaxHp);
    }

    public (string Message, BattleResult Result) ProcessSkill(long telegramId, int skillIndex, int enemyIndex)
    {
        var hero = _charRepo.GetActiveCharacter(telegramId);
        if (hero == null) return ("Character not found.", BattleResult.Defeat);
        if (hero.State != 1) return ("You are not in battle.", BattleResult.Ongoing);

        var battleData = _battleRepo.GetBattle(hero.Id);
        if (battleData == null)
        {
            _charRepo.UpdateCharacterState(hero.Id, 0);
            return ("Error: battle not found.", BattleResult.Ongoing);
        }

        var enemies = JsonSerializer.Deserialize<List<EnemyBattleData>>(battleData.Value.EnemiesJson) ?? new();
        hero.CurrentEffects = JsonSerializer.Deserialize<List<ActiveEffect>>(battleData.Value.HeroEffectsJson) ?? new();

        int effectiveMaxHp = GetEffectiveMaxHp(hero);
        if (hero.Hp > effectiveMaxHp)
        {
            hero.Hp = effectiveMaxHp;
            _charRepo.UpdateCharacterStats(hero);
        }

        if (skillIndex < 0 || skillIndex >= hero.Skills.Count)
            return ("Invalid skill.", BattleResult.Ongoing);
        if (enemyIndex < 0 || enemyIndex >= enemies.Count)
            return ("Invalid target.", BattleResult.Ongoing);

        var skill = hero.Skills[skillIndex];
        var targetEnemy = enemies[enemyIndex];
        string result;
        bool isFrightened = hero.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Frightened);

        if (isFrightened)
        {
            result = $"😨 You are frightened and cannot cast skills!";
        }
        else
        {
            var enemyObj = EnemyFactory.CreateByClassName(targetEnemy.ClassType);
            enemyObj.Hp = targetEnemy.Hp;
            enemyObj.CurrentEffects = targetEnemy.Effects;

            skill.Execute(hero, enemyObj);

            targetEnemy.Hp = enemyObj.Hp;
            targetEnemy.Effects = enemyObj.CurrentEffects;

            result = $"✨ You used {skill.Name} on {targetEnemy.Name}!";

            if (targetEnemy.Hp <= 0)
            {
                enemies.RemoveAt(enemyIndex);
                result += $"\n💀 {targetEnemy.Name} has been defeated!";
            }
        }

        return FinalizeTurn(hero, enemies, result, effectiveMaxHp);
    }

    private (string Message, BattleResult Result) FinalizeTurn(Character hero, List<EnemyBattleData> enemies, string initialResult, int effectiveMaxHp)
    {
        if (enemies.Count == 0)
        {
            hero.Hp = Math.Min(hero.MaxHp, hero.Hp + 23);
            _battleRepo.EndBattle(hero.Id);
            _charRepo.UpdateCharacterState(hero.Id, 0);
            _charRepo.UpdateTurnsLeft(hero.Id, hero.TurnsLeft - 1);
            _charRepo.UpdateCharacterStats(hero);
            _roomRepo.ChangeRoomType(hero.CurrentRoomId, RoomType.Empty); 
            return (initialResult + "\n\n✅ All enemies defeated! You recovered 23 HP and may move on.", BattleResult.Victory);
        }

        string result = initialResult;
        foreach (var enemy in enemies)
        {
            var enemyObj = EnemyFactory.CreateByClassName(enemy.ClassType);
            enemyObj.CurrentEffects = enemy.Effects;
            if (enemyObj.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Poisoned))
            {
                enemyObj.HandDmg = Math.Max(0, enemyObj.HandDmg - 1);
            }

            var skill = enemyObj.Skills[_rand.Next(enemyObj.Skills.Count)];
            skill.Execute(enemyObj, hero);
            result += $"\n🗡 {enemy.Name} used {skill.Name}";
        }

        TickEffects(hero);
        foreach (var enemy in enemies)
        {
            enemy.Effects = TickEffectsList(enemy.Effects, enemy);
        }

        enemies.RemoveAll(e => e.Hp <= 0);

        _charRepo.UpdateTurnsLeft(hero.Id, hero.TurnsLeft - 1);
        _charRepo.UpdateCharacterStats(hero);
        
        if (hero.Hp <= 0)
        {
            _battleRepo.EndBattle(hero.Id);
            _charRepo.UpdateCharacterState(hero.Id, 0);
            _charRepo.KillCharacter(hero.Id);
            return (result + "\n\n💀 You died. DARKNESS TOOK YOU.", BattleResult.Defeat);
        }

        if (enemies.Count == 0)
        {
            hero.Hp = Math.Min(hero.MaxHp, hero.Hp + 23);
            _battleRepo.EndBattle(hero.Id);
            _charRepo.UpdateCharacterState(hero.Id, 0);
            _charRepo.UpdateCharacterStats(hero);
            _roomRepo.ChangeRoomType(hero.CurrentRoomId, RoomType.Empty); 
            return (result + "\n\n✅ DoT effects killed the remaining enemies! You recovered 23 HP and may move on.", BattleResult.Victory);
        }

        _battleRepo.SaveBattleState(
            hero.Id,
            JsonSerializer.Serialize(enemies),
            JsonSerializer.Serialize(hero.CurrentEffects)
        );

        result += $"\n\n❤️ Your HP: {hero.Hp}/{effectiveMaxHp}";
        return (result, BattleResult.Ongoing);
    }

    public string ProcessDefend(long telegramId)
    {
        var hero = _charRepo.GetActiveCharacter(telegramId);
        if (hero == null) return "Character not found.";

        hero.CurrentEffects.Add(new ActiveEffect(BattleStateEnum.Defensive, 1));
        _charRepo.UpdateTurnsLeft(hero.Id, hero.TurnsLeft - 1);

        var battleData = _battleRepo.GetBattle(hero.Id);
        if (battleData == null) return "Battle not found.";

        _battleRepo.SaveBattleState(
            hero.Id,
            battleData.Value.EnemiesJson,
            JsonSerializer.Serialize(hero.CurrentEffects)
        );

        return "🛡 You take a defensive stance. Incoming damage reduced this turn.";
    }

    private int GetEnemyDefense(string classType)
    {
        return classType switch
        {
            "StarterMagician"  => 1,
            "WiseMagician"     => 2,
            "MagicianShishian" => 3,
            "Squire"           => 4,
            "RoyalKnight"      => 5,
            "Warlord"          => 6,
            "Skeleton"         => 7,
            "Zombie"           => 8,
            "HeadlessWarden"   => 9,
            _                  => 0
        };
    }

    private int GetEffectiveMaxHp(Character hero)
    {
        return hero.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Frenzy) 
            ? Math.Max(1, hero.MaxHp - 5) 
            : hero.MaxHp;
    }

    private void TickEffects(Character hero)
    {
        for (int i = hero.CurrentEffects.Count - 1; i >= 0; i--)
        {
            var effect = hero.CurrentEffects[i];

            if (effect.BattleStateEnum == BattleStateEnum.Burning)
                hero.Hp = Math.Max(0, hero.Hp - 2);
            else if (effect.BattleStateEnum == BattleStateEnum.Bleeding)
                hero.Hp = Math.Max(0, hero.Hp - 3);
            else if (effect.BattleStateEnum == BattleStateEnum.Poisoned)
                hero.Hp = Math.Max(0, hero.Hp - 1);
            else if (effect.BattleStateEnum == BattleStateEnum.Doomed && effect.TurnsLeft <= 1)
                hero.Hp = 0;

            effect.TurnsLeft--;
            if (effect.TurnsLeft <= 0)
                hero.CurrentEffects.RemoveAt(i);
        }
    }

    private List<ActiveEffect> TickEffectsList(List<ActiveEffect> effects, EnemyBattleData enemy)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            var effect = effects[i];

            if (effect.BattleStateEnum == BattleStateEnum.Burning)
                enemy.Hp = Math.Max(0, enemy.Hp - 2);
            else if (effect.BattleStateEnum == BattleStateEnum.Bleeding)
                enemy.Hp = Math.Max(0, enemy.Hp - 3);
            else if (effect.BattleStateEnum == BattleStateEnum.Poisoned)
                enemy.Hp = Math.Max(0, enemy.Hp - 1);
            else if (effect.BattleStateEnum == BattleStateEnum.Doomed && effect.TurnsLeft <= 1)
                enemy.Hp = 0;

            effect.TurnsLeft--;
            if (effect.TurnsLeft <= 0)
                effects.RemoveAt(i);
        }
        return effects;
    }
}