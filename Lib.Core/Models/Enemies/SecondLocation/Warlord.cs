using Lib.Core.BaseClasses;
using Lib.Core.Interfaces;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Enemies.SecondLocation;

public class Warlord : EnemyBase, IBattleUnit
{
    public Warlord()
    {
        Hp = 60;
        MaxHp = 60;
        HandDmg = 10;
        Name = "Warlord";
        PhisDefense = 6;
        Skills.Add(new Stechen(19));
        Skills.Add(new Abschneiden(11, 4));
        Skills.Add(new Einhorn(5));
        Skills.Add(new Zwerchhau(4, 3));
    }
    
    public override void CastSkill(ISkill chosenSkill, IBattleUnit target)
    {
        chosenSkill.Execute(this, target);
    }
}