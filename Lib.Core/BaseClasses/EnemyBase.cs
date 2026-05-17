using KPI_PROJECT.Models.EnemySkills;
using KPI_PROJECT.Models.EnemySkills.DefaultSkills;
using KPI_PROJECT.Models.EnemySkills.IBattleUnit;
using KPI_PROJECT.Models.EnumStates;

namespace KPI_PROJECT.Models.BaseClasses;

public abstract class EnemyBase : IBattleUnit
{
    public string Name { get; set; }
    
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    
    public int PhisDefense { get; set; }
    
    public int HandDmg { get; set; }
    public int MagicPower { get; set; }
    
    public List<ActiveEffect> CurrentEffects { get; set; } = new List<ActiveEffect>();
    public List<ISkill> CurrentSkills { get; set; } = new List<ISkill>();
    
    public bool IsDead => Hp <= 0;

    public EnemyBase()
    {
        CurrentSkills.Add(new HandAttack());
        CurrentSkills.Add(new Defend());
    }

    public abstract void CastSkill(ISkill chosenSkill, IBattleUnit target);
}