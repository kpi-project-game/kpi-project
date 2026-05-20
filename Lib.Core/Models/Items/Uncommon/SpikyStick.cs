using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Uncommon;

public class SpikyStick : BaseItem
{
    public override string Name => "Spiky Stick";
    public override string Description => "Now you can stub people...\n\nAdds Stechen spell to your arsenal...";
    public override Rarity Rarity => Rarity.Uncommon;
    public override void AddBonuses(Character character)
    {
        character.Skills.Add(new Stechen(10));
    }
}