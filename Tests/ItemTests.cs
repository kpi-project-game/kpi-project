using Lib.Core.BaseClasses;
using Lib.Core.Models.Items.Epic;
using NUnit.Framework;

namespace Tests;

public class ItemTests
{
    [Test]
    public void Grape_ShouldAddFrenzySkill_ToCharacter()
    {
        var hero = new Character();
        var grape = new Grape();

        grape.AddBonuses(hero, isFirstPickup: true);

        Assert.IsTrue(hero.Skills.Any(s => s.Name == "Frenzy"));
    }

    [Test]
    public void NewCharacter_ShouldInitialize_WithEmptyEffects_AndFullHP()
    {
        var hero = new Character { Hp = 100, MaxHp = 100 };

        var effectsCount = hero.CurrentEffects.Count;

        Assert.AreEqual(0, effectsCount);
        Assert.AreEqual(hero.MaxHp, hero.Hp);
        Assert.AreEqual(1, hero.Location);
        Assert.AreEqual(1, hero.Floor);
    }
}