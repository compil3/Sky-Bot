using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines;
using LGFA.Modules.Helpers;
using Serilog;

namespace LGFA.Modules
{
    public class Table : ModuleBase
    {
        [Command("table")]
        [Alias("standings")]
        [Summary("Retrieve the current season standings.")]
        [RequireUserPermission(GuildPermission.SendMessages)]
        public async Task Standings(string league)
        {
            var options = new RequestOptions {Timeout = 2};
            await Context.Message.DeleteAsync(options);

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
               var guildId = Context.Guild.Id;
                var channelId = Context.Channel.Id;

                Log.Logger.Warning($"{Context.User.Username} Triggered: Table.Standings ");

                var commandId = Context.Message.Id;

                var (teamStandings, teamPoints, currentSeason, leagueUrl, system) = TeamStanding.GetStandings(league);

                await Context.Channel
                    .SendMessageAsync($"{Context.User.Mention}\n``[Stats Provided By Leaguegaming.com]``", embed: TeamHelper.StandingEmbed(league))
                    .ConfigureAwait(false);
            }
            else
            {
                await ReplyAsync(
                    $"{Context.User.Mention} you are using the command in the wrong channel, try again in " +
                    $"{MentionUtils.MentionChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))}");
            }
        }
    }
}