using System.Threading.Tasks;
using Discord;
using Discord.Addons.CommandsExtension;
using Discord.Commands;
using LGFA.Extensions;

namespace LGFA.Modules
{
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class HelpCommand : ModuleBase
    {
        private readonly CommandService _service;

        public HelpCommand(CommandService commands)
        {
            _service = commands;
        }

        [Command("help")]
        [Summary("Displays this help message.")]
        public async Task HelpDisplay([Remainder] string command = null)
        {
            if (!Context.User.IsBot)
            {
                var options = new RequestOptions {Timeout = 2};
                await Context.Message.DeleteAsync(options);
            }

            const string botPrefix = ".";
            var helpEmbed = _service.GetDefaultHelpEmbed(command, botPrefix);
            await Context.User.SendMessageAsync(embed: helpEmbed);
            await ReplyAsync($"Check your DM's {Context.User.Mention}").AutoRemove(10);
            //var builder = new EmbedBuilder();

            //var user = Context.User as SocketGuildUser;

            //if (user.GuildPermissions.KickMembers == true)
            //{
            //    foreach (var command in _service.Commands)
            //    {
            //        var helpEmbed = _service.GetDefaultHelpEmbed()
            //        if (!user.GuildPermissions.Administrator && command.Name == "restart") continue;
            //        if (!user.GuildPermissions.KickMembers && command.Name == "update") continue;
            //        var embedFieldText = command.Summary ?? "No description available.";
            //        builder.AddField(command.Name, embedFieldText, false);
            //    }
            //    await Context.User.SendMessageAsync("List of commands and their usage: ", false, builder.Build());
            //    return;
            //}
            //else
            //{
            //    foreach (var command in _service.Commands)
            //    {
            //        string embedFieldText = command.Summary ?? "No description available.\n";
            //        builder.AddField(command.Name, embedFieldText, false);
            //    }
            //}

            //await Context.User.SendMessageAsync("List of commands and their usage: ", false, builder.Build());
        }
    }
}