using Lib.Core.Interfaces;
using Lib.Core.Models.StatesAndEffects;

namespace Lib.Core.Models.Skills.SpecialSkills;

public class Abschneiden : ISkill
{
    public string Name => "Abschneiden";
    public int BaseDmg { get; set; }
    public int BleedingDuration { get; set; }

    public Abschneiden(int baseDmg, int bleedingDuration)
    {
        BaseDmg = baseDmg;
        BleedingDuration = bleedingDuration;
    }

    public void Execute(IBattleUnit caster, IBattleUnit target)
    {
        var bleeding = target.CurrentEffects.Find(e => e.BattleStateEnum == BattleStateEnum.Bleeding);

        if (bleeding == null)
        {
            target.CurrentEffects.Add(new ActiveEffect(BattleStateEnum.Bleeding, BleedingDuration));
        }
        
        target.Hp = Math.Max(0, target.Hp - BaseDmg);
    }
}