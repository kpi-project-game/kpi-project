namespace KPI_PROJECT.Models.Items;

public class IronPlate : BaseItem
{
    public override string Name => "Iron Plate";
    public override string Description => "Ooooo Shiny...\n\nAdds 4 Physical Defence";
    public override Rarity Rarity => Rarity.Common;
    
    public override void AddBonuses(Player player)
    {
        player.BasePhisDefense += 4;
        player.PhisDefense += 4;
    }
}