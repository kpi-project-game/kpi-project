using Lib.Core.BaseClasses;
using Lib.Core.Interfaces;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Enemies.FirstLevel;

public class MagicianShishian : EnemyBase, IBattleUnit
{
    public MagicianShishian()
    {
        Hp = 45;
        MaxHp = 45;
        HandDmg = 7;
        Name = "Magician Shishian";
        PhisDefense = 3;
        Skills.Add(new FireballSkill(15));
        Skills.Add(new CharmSkill());
        Skills.Add(new ThunderStrikeSkill(6));
        Skills.Add(new Doom());
    }
    
    public override void CastSkill(ISkill chosenSkill, IBattleUnit target)
    {
        chosenSkill.Execute(this, target);
    }
}