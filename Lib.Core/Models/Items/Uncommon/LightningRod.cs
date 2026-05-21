using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Uncommon;

public class LightningRod : BaseItem
{
    public override string Name => "Lightning Rod";
    public override string Description => "Made of plastic...\n\nAdds Thunder Strike spell to your arsenal...";
    public override Rarity Rarity => Rarity.Uncommon;
    public override void AddBonuses(Character character, bool isFirstPickup = false)
    {
        character.Skills.Add(new ThunderStrikeSkill(10));
    }
}