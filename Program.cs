﻿using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
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
        private static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Thread.Sleep(-1);
        }
        private static DiscordSocketClient client;

        public static async Task MainAsync(string[] args)
        {            
            //CreateHostBuilder(args).Build().Run();
            await using var services = ConfigureServices();
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

        public static Task Client_Ready()
        {
            ulong id = Convert.ToUInt64(Environment.GetEnvironmentVariable("update_log_channel"));
            var chnl = client.GetChannel(id) as IMessageChannel;
            Manager.Manage(chnl);
            client.ReactionAdded += RoleHandler.OnReaction;
            client.UserJoined += Joined.UserJoined;
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
                .AddSingleton<RoleHandler>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .UseSystemd()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<Worker>();
            });
    }
}
