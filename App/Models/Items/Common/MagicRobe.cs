namespace KPI_PROJECT.Models.Items;

public class MagicRobe : BaseItem
{
    public override string Name => "Magic Robe";
    public override string Description => "It's blue and has stars pattern on it...\n\nAdds 3 Magic Power";
    public override Rarity Rarity => Rarity.Common;
    
    public override void AddBonuses(Character character)
    {
        character.MagicPower += 3;
    }
}