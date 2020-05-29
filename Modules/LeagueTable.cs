using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines;
using LGFA.Modules.Helpers;
using Serilog;

namespace LGFA.Modules
{
    public class LeagueTable : ModuleBase
    {
        [Command("table")]
        [Alias("standings")]
        [Summary("Retrieve the current season standings.")]
        [RequireUserPermission(GuildPermission.SendMessages)]
        public async Task GetStandings(string league)
        {
            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
                if (!Context.User.IsBot)
                {
                    var options = new RequestOptions();
                    options.Timeout = 2;
                    await Context.Message.DeleteAsync(options);
                }

                var guildId = Context.Guild.Id;
                var channelId = Context.Channel.Id;

                Log.Logger.Warning($"{Context.User.Username} Triggered: LeagueTable.GetStandings ");

                var commandId = Context.Message.Id;

                var (teamStandings, teamPoints, currentSeason, leagueUrl, system) = TeamStanding.GetStandings(league);

                await Context.Channel
                    .SendMessageAsync("``[Stats Provided By LGFA``", embed: TeamHelper.StandingEmbed(league))
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