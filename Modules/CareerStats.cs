using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Serilog;
using Sky_Bot.Engines;

namespace Sky_Bot.Modules
{
    public class CareerStats : ModuleBase
    {
        [Command("cs")]
        [Summary(
            ".cs Web-Site-UserName SeasonNumber (optional)\n Eg: .ps SpillShot 7\n If the website name has spaces try wrapping the name (.ps \"Name tag\" ")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task GetPlayerCareer(string playerLookup, string seasonId = null)
        {
           var guildId = Context.Guild.Id;
            
            if (guildId == 689119429375819951)
            {
                var stopWatch = new Stopwatch();
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                stopWatch.Start();
                await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: Career.GetCareer(playerLookup, seasonId)).ConfigureAwait(false);
                stopWatch.Stop();
                Log.Logger.Warning($"Time taken: {stopWatch.Elapsed}");
            }
            else if (Context.Guild.Id == 689119429375819951) await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
            GC.Collect();
        }
    }
}
