using Lib.Core.Interfaces;

namespace Lib.Core.Models.Skills.SpecialSkills;

public class Stechen : ISkill
{
    public string Name => "Stechen";
    public int BaseDmg { get; set; }

    public Stechen(int baseDmg)
    {
        BaseDmg = baseDmg;
    }
    public void Execute(IBattleUnit caster, IBattleUnit target)
    {
        target.Hp = Math.Max(0, target.Hp - BaseDmg);
    }
}