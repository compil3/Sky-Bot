using System;
using System.Diagnostics;
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
        [Summary("Retrieve a players overall career stats or a season.  You are able to use a part of the players GT or wrap the GT in quotes.")]
        [Remarks("Gamertag [optional season number] (use quotes for gamertags with spaces) eg: .cs \"Criminal Lazer\" 15 or .cs Lazer 15")]
        public async Task GetPlayerCareer([Summary("Gamertag [optional season number] (use quotes for gamertags with spaces) \neg: .cs \"Criminal Lazer\" 15 or .cs Lazer 15")] params string[] playerLookup)
        {
            var options = new RequestOptions{Timeout = 2};
            await Context.Message.DeleteAsync(options);

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
                Embed embed = null;
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");

                if (playerLookup[1] == null) //if no season number is entered in the command, than look for the Career stats "Official" table.
                {
                    var (playerCareer, playerUrl, playerName) = CareerBuilder.GetCareerNoSeason(playerLookup[0], playerLookup[1]);
                    if (playerCareer == null && playerUrl == null && playerName == null)
                        embed = Missing.CareerNotFound(playerLookup[0], playerUrl);
                    else
                        embed = CareerEmbed.VirtualCareerEmbed(playerCareer);
                }
                else
                {
                    var (career, playerUrl, playerName, seasonNumber) =
                        CareerBuilder.GetCareerSeason(playerLookup[0], playerLookup[1]);
                    if (career.Count == 1) embed = CareerEmbed.VirtualSeasonEmbed(career);
                    else if (career.Count == 0) embed = Missing.NotFound(playerName, playerUrl);
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