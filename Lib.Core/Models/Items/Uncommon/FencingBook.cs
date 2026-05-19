using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Uncommon;

public class FencingBook : BaseItem
{
    public override string Name => "Fencing Book";
    public override string Description => "It's written in German...\n\nAdds Zwerchhau spell to your arsenal...";
    public override Rarity Rarity => Rarity.Uncommon;
    public override void AddBonuses(Character character)
    {
        character.Skills.Add(new Zwerchhau(4, 3));
    }
}