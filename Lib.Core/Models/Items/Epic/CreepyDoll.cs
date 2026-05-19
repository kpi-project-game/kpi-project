using Lib.Core.BaseClasses;
using Lib.Core.Models.Skills.SpecialSkills;

namespace Lib.Core.Models.Items.Epic;

public class CreepyDoll : BaseItem
{
    public override string Name => "Creepy Doll";
    public override string Description => "Even you are scared of it...\n\nAdds Frighten spell to your arsenal...";
    public override Rarity Rarity => Rarity.Epic;
    public override void AddBonuses(Character character)
    {
        character.Skills.Add(new Frighten());
    }
}