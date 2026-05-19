using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Uncommon;

public class TalkingFish : BaseItem
{
    public override string Name => "Talking Fish";
    public override string Description => "What?...\"What?\"... WHAT?!...\n\nAdds Fireball spell to your arsenal... why?...";
    public override Rarity Rarity => Rarity.Uncommon;
    public override void AddBonuses(Character character)
    {
        character.Skills.Add(new FireballSkill(10));
    }
}