using Lib.Core.BaseClasses;
using Lib.Core.Interfaces;
using Lib.Core.Models.Skills.SpecialSkills;
using Lib.Core.Models.StatesAndEffects;

namespace Lib.Core.Models.Enemies.SecondLocation;

public class Squire : EnemyBase, IBattleUnit
{
    public Squire()
    {
        Hp = 50;
        MaxHp = 50;
        HandDmg = 8;
        Name = "Squire";
        PhisDefense = 4;
        Skills.Add(new Stechen(17));
        Skills.Add(new Abschneiden(9, 2));
    }

    public override void CastSkill(ISkill chosenSkill, IBattleUnit target)
    {
        chosenSkill.Execute(this, target);
    }
}