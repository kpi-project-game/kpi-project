using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Epic;

public class UnicornHorn : BaseItem
{
    public override string Name => "Unicorn Horn";
    public override string Description => "WAIT THEY ARE REAL?!...\n\nAdds Einhorn spell to your arsenal...";
    public override Rarity Rarity => Rarity.Epic;
    public override void AddBonuses(Character character, bool isFirstPickup = false)
    {
        character.Skills.Add(new Einhorn(6));
    }
}