using Lib.Core.BaseClasses;

namespace Lib.Core.Models.Items;

public abstract class BaseItem
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract Rarity Rarity { get; }

    public abstract void AddBonuses(Character character, bool isFirstPickup = false);
}

public enum Rarity
{
    Common,
    Uncommon,
    Epic,
    Legendary
}