namespace KPI_PROJECT.Models.Items;

public abstract class BaseItem
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract Rarity Rarity { get; }

    public abstract void AddBonuses(Character character);
}

public enum Rarity
{
    Common,
    Uncommon,
    Epic,
    Legendary
}