using Lib.Core.Models.Skills.DefaultSkills;
using Lib.Core.Models.Skills.SpecialSkills;
using Lib.Core.Models.StatesAndEffects;
using NUnit.Framework;

namespace Tests;

public class SkillTests
{
    [Test]
    public void HandAttack_ShouldDealCorrectDamage_ConsideringDefense()
    {
        var attacker = new DummyUnit { HandDmg = 15 };
        var target = new DummyUnit { Hp = 50, PhisDefense = 5 };
        var skill = new HandAttack();

        skill.Execute(attacker, target);

        Assert.AreEqual(40, target.Hp);
    }

    [Test]
    public void FireballSkill_ShouldApplyBurning_AndDealDamage()
    {
        var attacker = new DummyUnit { MagicPower = 5 };
        var target = new DummyUnit { Hp = 50 };
        var fireball = new FireballSkill(10);

        fireball.Execute(attacker, target);

        Assert.AreEqual(35, target.Hp);
        Assert.IsTrue(target.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Burning && e.TurnsLeft == 3));
    }

    [Test]
    public void Zwerchhau_ShouldDealDamageMultipleTimes()
    {
        var attacker = new DummyUnit();
        var target = new DummyUnit { Hp = 50 };
        var zwerchhau = new Zwerchhau(5, 3);

        zwerchhau.Execute(attacker, target);

        Assert.AreEqual(35, target.Hp);
    }

    [Test]
    public void Defend_ShouldAddDefensiveEffect()
    {
        var defender = new DummyUnit();
        var target = new DummyUnit();
        var defendSkill = new Defend();

        defendSkill.Execute(defender, target);

        Assert.IsTrue(target.CurrentEffects.Any(e => e.BattleStateEnum == BattleStateEnum.Defensive && e.TurnsLeft == 1));
    }
}