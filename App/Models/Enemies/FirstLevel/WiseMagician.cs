using KPI_PROJECT.Models.BaseClasses;
using KPI_PROJECT.Models.EnemySkills;
using KPI_PROJECT.Models.EnemySkills.IBattleUnit;
using KPI_PROJECT.Models.EnumStates;

namespace KPI_PROJECT.Models.Enemies.FirstLevel;

public class WiseMagician : EnemyBase, IBattleUnit
{
    public WiseMagician()
    {
        HP = 35;
        MaxHp = 35;
        HandDmg = 6;
        Name = "Wise Magician";
        PhisDefense = 2;
        Speed = 5;
        CurrentSkills.Add(new FireballSkill(10));
        CurrentSkills.Add(new CharmSkill());
        CurrentSkills.Add(new ThunderStrikeSkill(4));
    }
    
    public override void CastSkill(ISkill chosenSkill, IBattleUnit target)
    {
        chosenSkill.Execute(this, target);
    }
}