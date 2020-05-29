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
            var options = new RequestOptions{Timeout = 2};
            await Context.Message.DeleteAsync(options);

            const string botPrefix = ".";
            var helpEmbed = _service.GetDefaultHelpEmbed(command, botPrefix);
            await Context.User.SendMessageAsync(embed: helpEmbed);
            await ReplyAsync($"Check your DM's {Context.User.Mention}").AutoRemove(10);
        }
    }
}