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
    public class CareerStats : ModuleBase
    {
        [Command("cs")]
        [Alias("career")]
        //[Summary(".cs Web-Site-UserName SeasonNumber (optional)\n Eg: .ps SpillShot 7\n If the website name has spaces try wrapping the name (.ps \"Name tag\" ")]
        [Summary("Retrieve a players overall career stats or a season.")]
        public async Task GetPlayerCareer(string playerLookup, string seasonId = null)
        {
            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
                if (!Context.User.IsBot)
                {
                    var options = new RequestOptions { Timeout = 2 };
                    await Context.Message.DeleteAsync(options);
                }

                var guildId = Context.Guild.Id;
                Embed embed = null;
                Console.Write($"Guild ID: {Context.Guild.Id}");
                var stopWatch = new Stopwatch();
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                stopWatch.Start();

                if (seasonId == null
                ) //if no season number is entered in the command, than look for the Career stats "Official" table.
                {
                    var (playerCareer, playerUrl, playerName) = CareerBuilder.GetCareerNoSeason(playerLookup, seasonId);
                    if (playerCareer == null && playerUrl == null && playerName == null)
                        embed = Missing.CareerNotFound(playerLookup, playerUrl);
                    else
                        embed = CareerEmbed.VirtualCareerEmbed(playerCareer);
                }
                else
                {
                    var (career, playerUrl, playerName, seasonNumber) =
                        CareerBuilder.GetCareerSeason(playerLookup, seasonId);
                    if (career.Count <= 1) embed = CareerEmbed.VirtualSeasonEmbed(career);
                }

                await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: embed)
                    .ConfigureAwait(false);
                stopWatch.Stop();
                Log.Logger.Warning($"Total Time Taken: {stopWatch.Elapsed}");
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