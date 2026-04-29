namespace KPI_PROJECT.Models.Items;

public class RedHeart : BaseItem
{
    public override string Name => "Red Heart";
    public override string Description => "It looks way too realistic... IT STILL BEATS!?...\n\nAdds 15 Max HP";
    public override Rarity Rarity => Rarity.Common;

    public override void AddBonuses(Player player)
    {
        player.MaxHp += 15;
        player.Hp += 15;
    }
}