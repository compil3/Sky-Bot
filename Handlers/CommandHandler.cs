﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LGFA.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LGFA.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            //_discord.ReactionAdded += RulesRole;
            _discord.Ready += _botReady;
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;
            _commands = CommandConfig();

            

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += HandleCommand;
        }

        private static async Task MemberJoined(SocketGuildUser arg)
        {
            await Joined.UserJoined(arg);
            await Task.CompletedTask;
        }

        private static async Task RulesRole(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {

           await RoleHandler.OnReaction(arg1, arg2, arg3);
           await Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public static CommandService CommandConfig()
        {
            var commandServiceConfig = new CommandServiceConfig
            {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async, 
                IgnoreExtraArgs = true, 
                LogLevel = LogSeverity.Info
            };
            return new CommandService(commandServiceConfig);
        }

        private async Task HandleCommand(SocketMessage rawMessage)
        {
            var message = rawMessage as SocketUserMessage;
            if (message == null) return;

            if (message.Source != MessageSource.User) return;
            var context = new CommandContext(_discord,message);
            var argPos = 0;
            char prefix = '.';

            if (!(message.HasCharPrefix(prefix, ref argPos) || message.HasMentionPrefix(_discord.CurrentUser,ref argPos))) return;

            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
            {
                if (result.ErrorReason != "Unknown command.")
                {
                    await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
                }
            }
        }
        

        private static async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified) return;
            if (result.IsSuccess) return;

            await context.Channel.SendMessageAsync($"error: {result}");
        }

      

    }
}
