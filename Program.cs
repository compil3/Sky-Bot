using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using LGFA.Handlers;
using LGFA.Schedule;
using LGFA.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace LGFA
{
    internal class Program
    {
        private static DiscordSocketClient client;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
       

        public async Task MainAsync()
        {

            //CreateHostBuilder(args).Build().Run();
            using (var services = ConfigureServices())
            {
                var _client = services.GetRequiredService<DiscordSocketClient>();
                _client.Log += LogAsync;
                services.GetRequiredService<CommandService>().Log += LogAsync;
               
                await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));
                await _client.StartAsync();

                await services.GetRequiredService<CommandHandler>().InitializeAsync();
                _client.Ready += Client_Ready;
                await _client.SetGameAsync("LGFA | .help for info");
                client = _client;
                await Task.Delay(Timeout.Infinite);

            }
            //client = services.GetRequiredService<DiscordSocketClient>();

            //client.Log += LogAsync;

            //services.GetRequiredService<CommandService>().Log += LogAsync;

            //Console.ForegroundColor = ConsoleColor.Cyan;
            //Console.WriteLine("Sky Sports Bot v2.0");
            //Console.ResetColor();

            //await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));

            //await client.StartAsync();

            //await services.GetRequiredService<CommandHandler>().InitializeAsync();
            //client.Ready += Client_Ready;

            //await client.SetGameAsync("LFGA | .help for info");


            //await Task.Delay(Timeout.Infinite);
        }

        public static Task Client_Ready()
        {
            var newsChn = client.GetChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("news_channel"))) as IMessageChannel;
            var chnl = client.GetChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("update_log_channel"))) as IMessageChannel;
            Manager.Manage(chnl, newsChn);
            client.ReactionAdded += RoleHandler.OnRulesReaction;
            client.ReactionAdded += RoleHandler.OnSystemReaction;
            client.UserJoined += Joined.UserJoined;
            return Task.CompletedTask;
        }

        public static Task LogAsync(LogMessage log)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            Log.Logger.Fatal(log.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<RoleHandler>()
                .AddSingleton<HttpClient>()
                .AddSingleton<InteractiveService>()
                .BuildServiceProvider();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSystemd()
                .ConfigureServices((hostContext, services) => { services.AddHostedService<Worker>(); });
        }
    }
}