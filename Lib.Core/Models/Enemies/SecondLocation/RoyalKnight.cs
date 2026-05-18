using Lib.Core.BaseClasses;
using Lib.Core.Interfaces;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Enemies.SecondLocation;

public class RoyalKnight : EnemyBase, IBattleUnit
{
    public RoyalKnight()
    {
        Hp = 55;
        MaxHp = 55;
        HandDmg = 9;
        Name = "RoyalKnight";
        PhisDefense = 5;
        CurrentSkills.Add(new Stechen(18));
        CurrentSkills.Add(new Abschneiden(10, 3));
        CurrentSkills.Add(new Einhorn(3));
    }
    
    public override void CastSkill(ISkill chosenSkill, IBattleUnit target)
    {
        chosenSkill.Execute(this, target);
    }
}