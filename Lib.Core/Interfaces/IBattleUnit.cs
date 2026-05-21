using Lib.Core.Models.StatesAndEffects;

namespace Lib.Core.Interfaces;

public interface IBattleUnit
{
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int HandDmg { get; set; }
    public int PhisDefense { get; set; }
    public int MagicPower { get; set; }
    public List<ActiveEffect> CurrentEffects { get; set; }
    public List<ISkill> Skills { get; set; }
}