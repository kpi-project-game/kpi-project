using Lib.Core.Models.Items;
using Lib.Core.Models.Items.Common;
using Lib.Core.Models.Items.Epic;
using Lib.Core.Models.Items.Legendary;
using Lib.Core.Models.Items.Uncommon;

namespace Lib.Core.Models.Factories;

public class ItemFactory
{
    public static BaseItem? CreateByName(string name)
    {
        return name switch
        {
            "Iron Plate"      => new IronPlate(),
            "Magic Robe"      => new MagicRobe(),
            "Red Heart"       => new RedHeart(),
            "Spiky Glove"     => new SpikyGlove(),
            "Creepy Doll"     => new CreepyDoll(),
            "Grape"           => new Grape(),
            "Knights Emblem"  => new KnightsEmblem(),
            "Lipstick"        => new Lipstick(),
            "Talking Fish"    => new TalkingFish(),
            "Unicorn Horn"    => new UnicornHorn(),
            "Clockwork Jaws"  => new ClockworkJaws(),
            "Fencing Book"    => new FencingBook(),
            "Lightning Rod"   => new LightningRod(),
            "Spiky Stick"     => new SpikyStick(),
            "Jocker"          => new Jocker(),
            "SandWatch"       => new SandWatch(),
            _ => null
        };
    }
}