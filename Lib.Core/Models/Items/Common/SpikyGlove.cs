using Lib.Core.BaseClasses;

namespace Lib.Core.Models.Items.Common;

public class SpikyGlove : BaseItem
{
    public override string Name => "Spiky Glove";
    public override string Description => "It probably hurts to get punched with...\n\nAdds 3 to Hand Damage";
    public override Rarity Rarity => Rarity.Common;

    public override void AddBonuses(Character character, bool isFirstPickup = false)
    {
        character.HandDmg += 3;
    }
}