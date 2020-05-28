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
            if (!Context.User.IsBot)
            {
                var options = new RequestOptions();
                options.Timeout = 2;
                await Context.Message.DeleteAsync(options);
            }

            var guildId = Context.Guild.Id;
            var channelId = Context.Channel.Id;

            Log.Logger.Warning($"{Context.User.Username} Triggered: LeagueTable.GetStandings ");

            if (Context.Channel.Id == 705197391984197683 || Context.Channel.Id == 713176040716894208 ||
                Context.Channel.Id == 713237102145437776 || Context.Channel.Id == 711778374720421918)
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