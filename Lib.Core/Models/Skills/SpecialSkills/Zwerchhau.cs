using Lib.Core.Interfaces;

namespace Lib.Core.Models.Skills.SpecialSkills;

public class Zwerchhau : ISkill
{
    public string Name => "Zwerchhau";
    public int StrikeDmg { get; set; }
    public int AmountOfStrikes { get; set; }

    public Zwerchhau(int baseDmg, int amountOfStrikes)
    {
        StrikeDmg = baseDmg;
        AmountOfStrikes = amountOfStrikes;
    }
    
    public void Execute(IBattleUnit caster, IBattleUnit target)
    {
        for (int i = 0; i < AmountOfStrikes; i++)
        {
            target.Hp = Math.Max(0, target.Hp - StrikeDmg);
        }
    }
}