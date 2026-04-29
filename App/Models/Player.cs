using KPI_PROJECT.Models.EnemySkills.IBattleUnit;
using KPI_PROJECT.Models.EnumStates;
using KPI_PROJECT.Models.Items;

namespace KPI_PROJECT.Models;

public class Player : IBattleUnit
{
    public long TelegramId { get; set; }
    public string Nickname { get; set; }
    public string Class { get; set; }
    
    public int Level { get; set; } = 1;
    public string LocationName { get; set; }
    
    public int MaxHp { get; set; }
    public int Hp { get; set; }
    
    public int BasePhisDefense { get; set; }
    public int PhisDefense { get; set; }
    
    public List<ActiveEffect> CurrentEffects { get; set; }
    public List<BaseItem> Items { get; set; }
    
    public int HandDmg { get; set; }
    public int MagicPower { get; set; }
}