using KPI_PROJECT.Database;
using KPI_PROJECT.Models;
using KPI_PROJECT.Models.CharacterFactory;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace KPI_PROJECT.Telegram.Handlers;

public class BotManager
{
    private readonly DatabaseManager _dbManager = new DatabaseManager();
    private string botToken { get; set; }
    string tokenPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Telegram", "Token.txt");
    private readonly ITelegramBotClient _botClient;
    
     public BotManager()
     {
         if (!File.Exists(tokenPath))
         {
             throw new FileNotFoundException("No Token file found");
         }
         botToken = File.ReadAllText(tokenPath).Trim();
         _botClient = new TelegramBotClient(botToken);
     }

     public async Task StartAsync()
     {
         _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, null, CancellationToken.None);
     }
     
     private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
     {
         if (update.Type == UpdateType.CallbackQuery)
         {
             long id = update.CallbackQuery.From.Id;
             string callbackData = update.CallbackQuery.Data;
             string username = update.CallbackQuery.From.Username ?? "Unknown";

             await botClient.AnswerCallbackQuery(update.CallbackQuery.Id, cancellationToken: cancellationToken);

             _dbManager.EnsureUserExists(id, username);
             Character newHero = CharacterFactory.CreateFromClass(id, callbackData);
             _dbManager.SaveCharacter(newHero);

             await botClient.SendMessage(id, $"Path chosen! You are a {newHero.Class}. HP: {newHero.Hp}/{newHero.MaxHp}.", cancellationToken: cancellationToken);
             return;
         }

         if (update.Type == UpdateType.Message && update.Message?.Text != null)
         {
             long id = update.Message.From.Id;
             string messageText = update.Message.Text;

             if (messageText == "/start")
             {
                 var classChoose = new InlineKeyboardMarkup(new[]
                 {
                     new []
                     {
                         InlineKeyboardButton.WithCallbackData("Thief", "class_thief"),
                         InlineKeyboardButton.WithCallbackData("Warrior", "class_warrior"),
                         InlineKeyboardButton.WithCallbackData("Paladin", "class_paladin")
                     }
                 });

                 await botClient.SendMessage(id, "This will be a very long night...\nChoose thyself:", replyMarkup: classChoose, cancellationToken: cancellationToken);
                 return;
             }

             Character? currentHero = _dbManager.GetActiveCharacter(id);

             if (currentHero == null)
             {
                 await botClient.SendMessage(id, "Your character is dead or not created. Type /start to begin.", cancellationToken: cancellationToken);
                 return;
             }

             await botClient.SendMessage(id, $"[{currentHero.Class}] is ready! HP: {currentHero.Hp}/{currentHero.MaxHp}", cancellationToken: cancellationToken);
         }
     }
     private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
     {
         Console.WriteLine($"Error: {exception.Message}");
         return Task.CompletedTask;
     }
}
     
     
    
