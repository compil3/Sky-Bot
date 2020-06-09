using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines;
using LGFA.Engines.Career;
using LGFA.Modules.Helpers;
using Serilog;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    //[Group("Career Stats")]
    public class Career : ModuleBase
    {
        [Command("cs")]
        [Alias("career")]
        //[Summary(".cs Web-Site-UserName SeasonNumber (optional)\n Eg: .ps SpillShot 7\n If the website name has spaces try wrapping the name (.ps \"Name tag\" ")]
        [Summary("Retrieve a players overall career statistics or for a season specific season if found.")]
        [Remarks("In order to use the command you may enter a part of the player's __gamer tag__.  For example **.cs Lazer** when looking up \"**Criminal Lazer**\".\n" +
                 "If the __gamer tag__ is complex such as **\"lL A lK lE\"**, you can attempt a search by wraping the __gamer tag__ in quotes such as **.cs \"lL A lK lE\".**\n" +
                 "If you'd like to find stats for a specific season just enter the season number (eg: .cs Brisan 10), and if found will display results for that season.")]
        public async Task GetPlayerCareer(string playerLookup, [Optional]string seasonId)
        {
            var options = new RequestOptions{Timeout = 2};
            await Context.Message.DeleteAsync(options);

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
                Embed embed = null;
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");

                if (seasonId == null) //if no season number is entered in the command, than look for the Career stats "Official" table.
                {
                    var (playerCareer, playerUrl, playerName) = CareerBuilder.GetCareerNoSeason(playerLookup);
                    if (playerCareer == null && playerUrl == null && playerName == null)
                        embed = Missing.CareerNotFound(playerLookup,playerUrl);
                    else
                        embed = CareerEmbed.VirtualCareerEmbed(playerCareer);
                }
                else
                {
                    var (career, playerUrl, playerName, seasonNumber) =
                        CareerBuilder.GetCareerSeason(playerLookup, seasonId);
                    embed = career.Count switch
                    {
                        1 => CareerEmbed.VirtualSeasonEmbed(career),
                        0 => Missing.NotFound(playerName, playerUrl),
                        _ => embed
                    };
                }

                await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n``[Stats Provided by Leaguegaming.com]``", embed: embed)
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