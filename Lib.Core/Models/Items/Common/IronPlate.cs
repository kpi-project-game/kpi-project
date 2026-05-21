using Lib.Core.BaseClasses;

namespace Lib.Core.Models.Items.Common;

public class IronPlate : BaseItem
{
    public override string Name => "Iron Plate";
    public override string Description => "Ooooo Shiny...\n\nAdds 4 Physical Defence";
    public override Rarity Rarity => Rarity.Common;
    
    public override void AddBonuses(Character character, bool isFirstPickup = false)
    {
        character.BasePhisDefense += 4;
        character.PhisDefense += 4;
    }
}