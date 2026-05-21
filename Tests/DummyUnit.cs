using Lib.Core.Interfaces;
using Lib.Core.Models.StatesAndEffects;

namespace Tests;

public class DummyUnit : IBattleUnit
{
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int HandDmg { get; set; }
    public int PhisDefense { get; set; }
    public int MagicPower { get; set; }
    public List<ActiveEffect> CurrentEffects { get; set; } = new();
    public List<ISkill> Skills { get; set; } = new();
}