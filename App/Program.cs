using KPI_PROJECT.Models.FirstLevelEnemies;
using KPI_PROJECT.Telegram.Handlers;

class Program
{
    static async Task Main(string[] args)
    {
        BotManager botManager = new BotManager();
        await botManager.StartAsync();

        Console.WriteLine("Bot started");
        Console.ReadLine();
    }
}