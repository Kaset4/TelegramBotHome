using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using TelegramBotHome;
using TelegramBotHome.Controllers;
using TelegramBotHome.Models;

static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.Unicode;

        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();

        Console.WriteLine("Starting Service");
        await host.RunAsync();
        Console.WriteLine("Service stopped");
    }

    static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient("7022843258:AAFHf1WPDOuIlLgDEvfNh4fcRyLMycrbEIY"));

        services.AddTransient<TextMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddTransient<CalcMessageController>();
        services.AddTransient<CountMessageController>();
        services.AddSingleton<IStorage, MemoryStorage>();
        services.AddHostedService<Bot>();
    }
}