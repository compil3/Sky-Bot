using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines;
using Serilog;

namespace LGFA.Modules
{
    public class TeamStats: ModuleBase
    {
        [Command("ts")]
        [Alias("teamstats")]
        [Summary(".ts TeamName xbox/psn [eg: .ts Liverpool Xbox]")]
        [Remarks("Returns statistics for the team entered if found.")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task GetTeams(string teamName, string league)
        {
            var guildId = Context.Guild.Id;
            
            if (guildId == 689119429375819951)
            {
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                //await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: Team.GetTeam(teamName, league)).ConfigureAwait(false);
                GC.Collect();
            }
            else if (Context.Guild.Id == 689119429375819951) await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
        }
    }
}
