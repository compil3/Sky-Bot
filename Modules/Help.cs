using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Sky_Bot.Modules
{
    public class HelpCommand : ModuleBase
    {
        private readonly CommandService _service;

        public HelpCommand(CommandService commands)
        {
            _service = commands;
        }

        [Command("help")]
        [Summary("Displays this help message.")]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task HelpDisplay()
        {
            var builder = new EmbedBuilder();

            var user = Context.User as SocketGuildUser;

            if (!user.GuildPermissions.KickMembers)
            {
                foreach (var command in _service.Commands)
                {
                    if (!user.GuildPermissions.Administrator && command.Name == "restart") continue;
                    if (!user.GuildPermissions.KickMembers && command.Name == "update") continue;
                    string embedFieldText = command.Summary ?? "No description available.\n";
                    builder.AddField(command.Name, embedFieldText, true);
                }
                await Context.User.SendMessageAsync("List of commands and their usage: ", false, builder.Build());
                return;
            }
            else
            {
                foreach (var command in _service.Commands)
                {
                    string embedFieldText = command.Summary ?? "No description available.\n";
                    builder.AddField(command.Name, embedFieldText, true);
                }
            }

            await Context.User.SendMessageAsync("List of commands and their usage: ", false, builder.Build());
        }
    }
}
