using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KPI_PROJECT.Models;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using KPI_PROJECT.Services;
using KPI_PROJECT.Models.Enums;
using KPI_PROJECT.Models.BaseClasses;
using KPI_PROJECT.Models.CharacterFactory;
using KPI_PROJECT.Models.Items;

namespace KPI_PROJECT.Handlers;

public class BotManager
{
    private readonly DatabaseManager _dbManager = new();
    private readonly ITelegramBotClient _botClient;

    public BotManager(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    private List<BaseItem> GetRoomLoot()
    {
        return new List<BaseItem> 
        { 
            new RedHeart(), 
            new IronPlate()
        };
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            long id = update.CallbackQuery.From.Id;
            string data = update.CallbackQuery.Data;

            await botClient.AnswerCallbackQuery(update.CallbackQuery.Id);

            if (data.StartsWith("class_"))
            {
                _dbManager.EnsureUserExists(id, update.CallbackQuery.From.Username ?? "Unknown");
                Character hero = CharacterFactory.CreateFromClass(id, data);
                
                int charId = _dbManager.SaveCharacter(hero);
                hero.Id = charId; 
                
                int r1 = _dbManager.CreateRoom(charId, RoomType.Empty);
                int r2 = _dbManager.CreateRoom(charId, RoomType.Empty);
                int r3 = _dbManager.CreateRoom(charId, RoomType.Loot);
                int r4 = _dbManager.CreateRoom(charId, RoomType.Exit);

                _dbManager.CreateConnection(r1, r2, "North");
                _dbManager.CreateConnection(r2, r1, "South");
                _dbManager.CreateConnection(r2, r3, "East");
                _dbManager.CreateConnection(r3, r2, "West");
                _dbManager.CreateConnection(r2, r4, "North");

                _dbManager.UpdateCharacterRoom(id, r1);
                hero.CurrentRoomId = r1;

                await ShowRoom(id, hero);
            }
            else if (data.StartsWith("move_to:"))
            {
                int targetId = int.Parse(data.Split(':')[1]);
                _dbManager.UpdateCharacterRoom(id, targetId);
                Character? hero = _dbManager.GetActiveCharacter(id);
                if (hero != null) await ShowRoom(id, hero);
            }
            else if (data.StartsWith("action_loot:"))
            {
                int roomId = int.Parse(data.Split(':')[1]);
                Character? hero = _dbManager.GetActiveCharacter(id);
                
                if (hero != null)
                {
                    var foundItems = GetRoomLoot();
                    string lootMsg = "🎉 You found items:\n\n";

                    foreach (var item in foundItems)
                    {
                        item.AddBonuses(hero);
                        _dbManager.AddItemToInventory(hero.Id, item.Name);
                        lootMsg += $"**{item.Name}** ({item.Rarity})\n_{item.Description}_\n\n";
                    }

                    _dbManager.UpdateCharacterStats(hero);
                    _dbManager.ChangeRoomType(roomId, RoomType.Empty);
                    
                    await botClient.SendMessage(id, lootMsg, parseMode: ParseMode.Markdown);
                    await ShowRoom(id, hero);
                }
            }
            return;
        }

        if (update.Type == UpdateType.Message && update.Message?.Text != null)
        {
            long id = update.Message.From.Id;
            if (update.Message.Text == "/start")
            {
                var menu = new InlineKeyboardMarkup(new[] {
                    new [] { InlineKeyboardButton.WithCallbackData("Warrior", "class_warrior"), 
                             InlineKeyboardButton.WithCallbackData("Thief", "class_thief"),
                             InlineKeyboardButton.WithCallbackData("Paladin",  "class_Paladin")
                    }
                });
                await botClient.SendMessage(id, "Choose class:", replyMarkup: menu);
            }
            else
            {
                Character? hero = _dbManager.GetActiveCharacter(id);
                if (hero != null)
                {
                    await ShowRoom(id, hero);
                }
            }
        }
    }

    private async Task ShowRoom(long chatId, Character hero)
    {
        var room = _dbManager.GetRoom(hero.CurrentRoomId);
        if (room == null) return;

        var inv = _dbManager.GetInventory(hero.Id);
        string bag = inv.Count > 0 ? string.Join(", ", inv) : "Empty";

        string icon = room.Type switch { RoomType.Loot => "🎁", RoomType.Exit => "🚪", _ => "🌫️" };
        
        string msg = $"{icon} **Room Type: {room.Type}**\n" +
                     $"❤ HP: {hero.Hp}/{hero.MaxHp} | 🪄 MP: {hero.MagicPower}\n" +
                     $"🛡 Def: {hero.PhisDefense} | ⚔️ Dmg: {hero.HandDmg}\n" +
                     $"🎒 Bag: {bag}\n\nWhere to?";

        var buttons = new List<InlineKeyboardButton[]>();
        
        if (room.Type == RoomType.Loot)
        {
            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData("🔍 Search the Room", $"action_loot:{room.Id}") });
        }

        foreach (var ex in room.Exits)
        {
            buttons.Add(new[] { InlineKeyboardButton.WithCallbackData($"Go {ex.Direction}", $"move_to:{ex.TargetRoomId}") });
        }

        await _botClient.SendMessage(chatId, msg, replyMarkup: new InlineKeyboardMarkup(buttons), parseMode: ParseMode.Markdown);
    }
}