using Lib.Core.BaseClasses;
using Lib.Core.Interfaces;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Enemies.FirstLevel;

public class WiseMagician : EnemyBase, IBattleUnit
{
    public WiseMagician()
    {
        Hp = 35;
        MaxHp = 35;
        HandDmg = 6;
        Name = "Wise Magician";
        PhisDefense = 2;
        Skills.Add(new FireballSkill(12));
        Skills.Add(new CharmSkill());
        Skills.Add(new ThunderStrikeSkill(5));
    }
    
    public override void CastSkill(ISkill chosenSkill, IBattleUnit target)
    {
        chosenSkill.Execute(this, target);
    }
}