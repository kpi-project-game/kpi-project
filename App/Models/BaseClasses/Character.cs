using KPI_PROJECT.Models.EnemySkills.IBattleUnit;
using KPI_PROJECT.Models.Enums;
using KPI_PROJECT.Models.EnumStates;
using KPI_PROJECT.Models.Items;

namespace KPI_PROJECT.Models;

public class Character : IBattleUnit
{
    public int Id { get; set; } 
    
    public long TelegramId { get; set; } 
    
    public string Class { get; set; }
    public int Level { get; set; } = 1;
    public LocationName LocationName { get; set; } 
    
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    
    public int BasePhisDefense { get; set; }
    public int PhisDefense { get; set; }
    
    public List<ActiveEffect> CurrentEffects { get; set; } = new();
    public List<BaseItem> Items { get; set; } = new();
    
    public int HandDmg { get; set; }
    public int MagicPower { get; set; }
}