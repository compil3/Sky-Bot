using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Sky_Bot.Services
{
    public class CommandHandlingService
    {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += HandleCommand;
        }
        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommand(SocketMessage rawMessage)
        {
            var message = rawMessage as SocketUserMessage;
            //if (!(rawMessage is SocketUserMessage message)) return;
            //if (message.Source != MessageSource.User) return;
            if (message == null) return;

            if (message.Source != MessageSource.User) return;
            var context = new CommandContext(_discord,message);
            var argPos = 0;
            char prefix = '.';

            if (!(message.HasCharPrefix(prefix, ref argPos) || message.HasMentionPrefix(_discord.CurrentUser,ref argPos))) return;

            //var context = new SocketCommandContext(_discord,message);
            //var result = await _commands.ExecuteAsync(context, argPos, _services);
            //if (!result.IsSuccess)
            //{
            //    Log.Logger.Fatal($"{context.Message.Content} Error: {result.ErrorReason}");
            //    await context.Channel.SendMessageAsync($"{context.Message.Content} Error: {result.ErrorReason}");
            //}
            //if (result.IsSuccess) Log.Warning($"{context.Message.Content} command accepted");
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
            {
                if (result.ErrorReason != "Unknown command.")
                {
                    await message.Channel.SendMessageAsync($"**Error:** {result.ErrorReason}");
                }
            }
        }
        

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (!command.IsSpecified) return;
            if (result.IsSuccess) return;

            await context.Channel.SendMessageAsync($"error: {result}");
        }

      

    }
}
