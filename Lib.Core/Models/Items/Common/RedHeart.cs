using Lib.Core.BaseClasses;

namespace Lib.Core.Models.Items.Common;

public class RedHeart : BaseItem
{
    public override string Name => "Red Heart";
    public override string Description => "It looks way too realistic... IT STILL BEATS!?...\n\nAdds 15 Max HP";
    public override Rarity Rarity => Rarity.Common;

    public override void AddBonuses(Character character, bool isFirstPickup = false)
    {
        character.MaxHp += 15;

        if (isFirstPickup)
        {
            character.Hp += 15;
            
            if (character.Hp > character.MaxHp)
            {
                character.Hp = character.MaxHp;
            }
        }
    }
}