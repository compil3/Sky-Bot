using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Serilog;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    internal class TeamStats : ModuleBase
    {
        [Command("ts")]
        [Summary(".ts TeamName xbox/psn [eg: .ts Liverpool Xbox]")]
        private async Task GetTeams(string teamName, string league)
        {
            if (Context.Channel.Id == 689119429375819951)
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
            //await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: Team.GetTeam(teamName, league)).ConfigureAwait(false);
            else if (Context.Guild.Id == 689119429375819951)
                await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
        }
    }
}