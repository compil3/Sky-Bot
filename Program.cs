using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Web;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Sky_Bot.Schedule;
using Sky_Bot.Services;

namespace Sky_Bot
{
    internal static class Program
    {
        private static DiscordSocketClient _client;
        //static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
            Thread.Sleep(-1);
        }
        public static async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                JobManager.Initialize(new PSN());


                client.Log += LogAsync;
                //client.Ready += ClientReady;

                services.GetRequiredService<CommandService>().Log += LogAsync;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Sky Sports Bot v1.0");
                Console.ResetColor();

                Console.WriteLine($"Environment Variable: {Environment.GetEnvironmentVariable("token")}");
                await client.LoginAsync(TokenType.Bot, 
                    Environment.GetEnvironmentVariable("token"));
                await client.StartAsync();

                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
                Log.Warning("Command Modules loaded.");
                await client.SetGameAsync("Watching LGFA");
                await Task.Delay(-1);
            }
        }

        private static async Task ClientReady()
        {
            var chnl = _client.GetGuild(689119429375819951).GetTextChannel(704183525863063642) as ISocketMessageChannel;
            //JobManager.Initialize(new)
        }

        public static Task LogAsync(LogMessage log)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme:AnsiConsoleTheme.Code)
                .CreateLogger();
            Log.Fatal(log.ToString());
            return Task.CompletedTask;
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
