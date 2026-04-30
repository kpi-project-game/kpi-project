using KPI_PROJECT.Handlers;
using KPI_PROJECT.Models.FirstLevelEnemies;
using KPI_PROJECT.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

class Program
{
    static async Task Main(string[] args)
    {
        var dbManager = new DatabaseManager();

        string tokenPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Telegram", "Token.txt");
        string botToken = await File.ReadAllTextAsync(tokenPath);
        
        var botClient = new TelegramBotClient(botToken.Trim());
        BotManager botManager = new BotManager(botClient);

        using CancellationTokenSource cts = new();

        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() 
        };

        botClient.StartReceiving(
            updateHandler: botManager.HandleUpdateAsync,
            errorHandler: ErrorHandler,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await botClient.GetMe();
        Console.WriteLine($"Bot started successfully: @{me.Username}");
        Console.WriteLine("Pres enter to stop...");
        Console.ReadLine();

        cts.Cancel(); 
    }

    private static Task ErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Error: {exception.Message}");
        return Task.CompletedTask;
    }
}