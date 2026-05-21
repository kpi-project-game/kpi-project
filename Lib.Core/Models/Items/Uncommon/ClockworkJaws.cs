using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Uncommon;

public class ClockworkJaws : BaseItem
{
    public override string Name => "Clockwork Jaws";
    public override string Description => "They work on... batteries?...\n\nAdds Bite spell to your arsenal...";
    public override Rarity Rarity => Rarity.Uncommon;
    public override void AddBonuses(Character character, bool isFirstPickup = false)
    {
        character.Skills.Add(new Bite(10));
    }
}