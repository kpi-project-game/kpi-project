using Lib.Core.Interfaces;

namespace Lib.Core.Models.Skills.SpecialSkills;

public class Einhorn : ISkill
{
    public string Name => "Einhorn";
    public int BonusDefence { get; set; }

    public Einhorn(int bonusDefence)
    {
        BonusDefence = bonusDefence;
    }
    
    public void Execute(IBattleUnit caster, IBattleUnit target)
    {
        caster.PhisDefense += BonusDefence;
    }
}