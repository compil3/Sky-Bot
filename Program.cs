using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Web;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Sky_Bot.Configuration.Bot;
using Sky_Bot.Modules;
using Sky_Bot.Schedule;
using Sky_Bot.Services;

namespace Sky_Bot
{
    internal class Program
    {
        //static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
            Thread.Sleep(-1);
        }
        private static DiscordSocketClient client;

        public static async Task MainAsync()
        {
            using (var services = ConfigureServices())
            {
                client = services.GetRequiredService<DiscordSocketClient>();

                client.Log += LogAsync;

                services.GetRequiredService<CommandService>().Log += LogAsync;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Sky Sports Bot v1.0");
                Console.ResetColor();


                await client.LoginAsync(TokenType.Bot, 
                    Environment.GetEnvironmentVariable("token"));

                await client.StartAsync();

                await services.GetRequiredService<CommandHandler>().InitializeAsync();
                client.Ready += Client_Ready;
                
                await client.SetGameAsync("Watching LGFA");


                await Task.Delay(Timeout.Infinite);
            }
        }

        public static Task Client_Ready()
        {
            ulong id = Convert.ToUInt64(Environment.GetEnvironmentVariable("update_log_channel"));
            var chnl = client.GetChannel(id) as IMessageChannel;

            Manager.Manage(chnl);
            Log.Logger.Warning("Schedules Initialized.");
            return Task.CompletedTask;
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
            Log.Warning("Modules loaded.");
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
