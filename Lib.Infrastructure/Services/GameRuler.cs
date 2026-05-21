using System;
using System.Collections.Generic;
using Lib.Core.BaseClasses;
using Lib.Core.Enums;
using Lib.Core.Models.Items;
using Lib.Core.Models.Items.Common;
using Lib.Core.Models.Items.Epic;
using Lib.Core.Models.Items.Legendary;
using Lib.Core.Models.Items.Uncommon;
using Lib.Infrastructure.Database;
using Lib.Infrastructure.Database.Repositories;
using Serilog;

namespace Lib.Infrastructure.Services;

public class GameRuler
{
    private readonly InventoryRepository _invRepo;
    private readonly CharacterRepository _charRepo;
    private readonly RoomRepository _roomRepo;
    private readonly Random _rand = new Random();

    public GameRuler(InventoryRepository invRepo, CharacterRepository charRepo, RoomRepository roomRepo)
    {
        _invRepo = invRepo;
        _charRepo = charRepo;
        _roomRepo = roomRepo;
    }

    public string ProcessLooting(Character hero, int roomId)
    {
        Log.Debug("Hero {HeroId} is looting room {RoomId}", hero.Id, roomId);

        hero.Hp = Math.Min(hero.MaxHp, hero.Hp + 15);

        var foundItems = RollLoot();
        string lootMsg = "🎉 You found items and recovered 15 HP!\n\n";

        foreach (var item in foundItems)
        {
            item.AddBonuses(hero, true);
            _invRepo.AddItemToInventory(hero.Id, item.Name);
            string rarityIcon = item.Rarity switch
            {
                Rarity.Common    => "⚪",
                Rarity.Uncommon  => "🟢",
                Rarity.Epic      => "🔵",
                Rarity.Legendary => "🟡",
                _ => ""
            };
            lootMsg += $"{rarityIcon} **{item.Name}** ({item.Rarity})\n_{item.Description}_\n\n";
        
            Log.Information("Hero {HeroId} found item {ItemName} ({Rarity}) in room {RoomId}", hero.Id, item.Name, item.Rarity, roomId);
        }

        _charRepo.UpdateCharacterStats(hero);
        _roomRepo.ChangeRoomType(roomId, RoomType.Empty);
    
        Log.Debug("Room {RoomId} marked as Empty after looting by hero {HeroId}", roomId, hero.Id);

        return lootMsg;
    }

    private List<BaseItem> RollLoot()
    {
        int count = 1; 
        var result = new List<BaseItem>();

        var common = new List<BaseItem>
        {
            new IronPlate(), new MagicRobe(), new RedHeart(), new SpikyGlove()
        };

        var uncommon = new List<BaseItem>
        {
            new ClockworkJaws(), new FencingBook(), new LightningRod(), new SpikyStick()
        };

        var epic = new List<BaseItem>
        {
            new CreepyDoll(), new Grape(), new KnightsEmblem(),
            new Lipstick(), new TalkingFish(), new UnicornHorn()
        };
        
        var legendary = new List<BaseItem>
        {
            new Jocker(), new SandWatch()
        };

        for (int i = 0; i < count; i++)
        {
            int roll = _rand.Next(1, 101);
            List<BaseItem> pool = roll switch
            {
                <= 55 => common,
                <= 80 => uncommon,
                <= 95 => epic,
                _     => legendary
            };
            result.Add(pool[_rand.Next(pool.Count)]);
        }

        return result;
    }
}