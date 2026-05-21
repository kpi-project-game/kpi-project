using Lib.Core.Interfaces;
using Lib.Core.Models.Skills.DefaultSkills;
using Lib.Core.Models.StatesAndEffects;

namespace Lib.Core.BaseClasses;

public abstract class EnemyBase : IBattleUnit
{
    public string Name { get; set; }

    public int MaxHp { get; set; }
    public int Hp { get; set; }

    public int PhisDefense { get; set; }

    public int HandDmg { get; set; }
    public int MagicPower { get; set; }

    public List<ActiveEffect> CurrentEffects { get; set; } = new();
    public List<ISkill> Skills { get; set; } = new();

    public bool IsDead => Hp <= 0;

    public EnemyBase()
    {
        Skills.Add(new HandAttack());
        Skills.Add(new Defend());
    }

    public abstract void CastSkill(ISkill chosenSkill, IBattleUnit target);
}