using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Epic;

public class KnightsEmblem : BaseItem
{
    public override string Name => "Knights Emblem";
    public override string Description => "From now on everyone will call you sir...\n\nAdds Abschneiden spell to your arsenal...";
    public override Rarity Rarity => Rarity.Epic;
    public override void AddBonuses(Character character, bool isFirstPickup = false)
    {
        character.Skills.Add(new Abschneiden(8, 3));
    }
}