using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Epic;

public class Lipstick : BaseItem
{
    public override string Name => "Lipstick";
    public override string Description => "It's blue and smells like... strawberries?...\n\nAdds Charm spell to your arsenal... why?...";
    public override Rarity Rarity => Rarity.Epic;
    public override void AddBonuses(Character character)
    {
        character.Skills.Add(new CharmSkill());
    }
}