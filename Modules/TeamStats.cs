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
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
     public class TeamStats: ModuleBase
    {
        [Command("ts")]
        [Summary(".ts TeamName xbox/psn [eg: .ts Liverpool Xbox]")]
        public async Task GetTeams(string teamName, string league)
        {
            
            if (Context.Channel.Id == 689119429375819951)
            {
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                //await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: Team.GetTeam(teamName, league)).ConfigureAwait(false);
            }
            else if (Context.Guild.Id == 689119429375819951) await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
        }
    }
}
