using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
        [Command("c")]
        [Summary(
            ".c Web-Site-UserName SeasonNumber (optional)\n Eg: .ps SpillShot 7\n If the website name has spaces try wrapping the name (.ps \"Name tag\" ")]
        public async Task GetPlayerCareer(string playerLookup, string seasonId = null)
        {
            var guildId = Context.Guild.Id;
            Embed embed = null;
            EmbedBuilder builder = null;
            if (guildId == 689119429375819951)
            {
                var stopWatch = new Stopwatch();
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                stopWatch.Start();
                if (seasonId == null) //if no season number is entered in the command, than look for the Career stats "Official" table.
                {
                    var (playerCareer, playerUrl, playerName) = CareerBuilder.GetCareerNoSeason(playerLookup, seasonId);
                    if (playerCareer == null && playerUrl == null && playerName == null)
                        embed = Missing.CareerNotFound(playerLookup, playerUrl);
                    else
                    {
                        embed = CareerEmbed.VirtualCareerEmbed(playerCareer);
                    }
                }
                else
                {
                    var (career, playerUrl, playerName, seasonNumber) =
                        CareerBuilder.GetCareerSeason(playerLookup, seasonId);
                    if (career == null)
                    {
                        embed = CareerEmbed.VirtualCareerEmbed(career);
                    }
                }

                await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: embed).ConfigureAwait(false);
                stopWatch.Stop();
                Log.Logger.Warning($"Time taken: {stopWatch.Elapsed}");
            }
            else if (Context.Guild.Id == 689119429375819951) await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
        }
    }
}
