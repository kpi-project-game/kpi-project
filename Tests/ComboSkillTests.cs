using Lib.Core.Models.Skills.SpecialSkills;
using Lib.Core.Models.StatesAndEffects;
using NUnit.Framework;

namespace Tests;

public class ComboSkillTests
{
    [Test]
    public void ThunderStrike_ShouldDealDoubleDamage_IfTargetIsCharmed()
    {
        var attacker = new DummyUnit();
        var target = new DummyUnit { Hp = 100 };
        target.CurrentEffects.Add(new ActiveEffect(BattleStateEnum.Charmed, 2));
        var thunder = new ThunderStrikeSkill(10);

        thunder.Execute(attacker, target);

        Assert.AreEqual(80, target.Hp);
    }

    [Test]
    public void ThunderStrike_ShouldDealNormalDamage_IfTargetIsNotCharmed()
    {
        var attacker = new DummyUnit();
        var target = new DummyUnit { Hp = 100 };
        var thunder = new ThunderStrikeSkill(10);

        thunder.Execute(attacker, target);

        Assert.AreEqual(90, target.Hp);
    }

    [Test]
    public void CharmSkill_ShouldReducePhisDefense_AndAddEffect()
    {
        var attacker = new DummyUnit();
        var target = new DummyUnit { PhisDefense = 10 };
        var charm = new CharmSkill();

        charm.Execute(attacker, target);

        Assert.AreEqual(7, target.PhisDefense);
        Assert.IsTrue(target.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Charmed && e.TurnsLeft == 3));
    }

    [Test]
    public void Abschneiden_ShouldDealDamage_AndApplyBleeding()
    {
        var attacker = new DummyUnit();
        var target = new DummyUnit { Hp = 50 };
        var abschneiden = new Abschneiden(6, 3);

        abschneiden.Execute(attacker, target);

        Assert.AreEqual(44, target.Hp);
        Assert.IsTrue(target.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Bleeding && e.TurnsLeft == 3));
    }
}