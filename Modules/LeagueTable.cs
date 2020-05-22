using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using LGFA.Engines;
using LGFA.Modules.Helpers;
using Microsoft.VisualBasic;
using Serilog;

namespace LGFA.Modules
{
    public class LeagueTable : ModuleBase
    {
        [Command("table", RunMode = RunMode.Async)]
        [Alias("standings")]
        [Summary(".table system Eg. .standings PSN")]
        [Remarks("Pulls up the standings table for either system")]
        [RequireUserPermission(GuildPermission.SendMessages)]
        public async Task GetStandings(string league)
        {

            if (!Context.User.IsBot)
            {
                await Context.Message.DeleteAsync();
            }

            var guildId = Context.Guild.Id;
            var channelId = Context.Channel.Id;

            Log.Logger.Warning($"{Context.User.Username} Triggered: LeagueTable.GetStandings ");

            if (Context.Channel.Id == 705197391984197683 || Context.Channel.Id == 713176040716894208 || Context.Channel.Id == 713237102145437776)
            {
                var commandId = Context.Message.Id;

                var (teamStandings, teamPoints, currentSeason, leagueUrl, system) = TeamStanding.GetStandings(league);

                await Context.Channel
                    .SendMessageAsync("``[Stats Provided By LGFA``", embed: TeamHelper.StandingEmbed(league))
                    .ConfigureAwait(false);
            }
            else
            {
                await ReplyAsync($"Channel permission denied.  Try again in the proper channel {Context.User.Mention}");
            }

        }
    }
}
