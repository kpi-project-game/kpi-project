using Lib.Core.Factories;
using Lib.Core.Models.Skills.SpecialSkills;
using Lib.Core.Models.Enemies.SecondLocation;
using NUnit.Framework;

namespace Tests;

public class FactoryTests
{
    [Test]
    public void SkillFactory_ShouldReturnCorrectSkill_ByName()
    {
        string skillName = "Zwerchhau";

        var skill = SkillFactory.CreateByName(skillName);

        Assert.IsNotNull(skill);
        Assert.IsInstanceOf<Zwerchhau>(skill);
        Assert.AreEqual(skillName, skill.Name);
    }

    [Test]
    public void EnemyFactory_ShouldReturnCorrectEnemy_ByClassName()
    {
        string enemyClass = "RoyalKnight";

        var enemy = EnemyFactory.CreateByClassName(enemyClass);

        Assert.IsNotNull(enemy);
        Assert.IsInstanceOf<RoyalKnight>(enemy);
    }
}