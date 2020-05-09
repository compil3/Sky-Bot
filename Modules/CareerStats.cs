using System;
using System.Collections.Generic;
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
        public async Task GetPlayerCareer(string playerLookup)
        {
           var guildId = Context.Guild.Id;
            
            if (guildId == 689119429375819951)
            {
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                await Context.Channel.SendMessageAsync("```asciidoc\n[Stats Provided by LGFA] ```", embed: Career.GetCareer(playerLookup)).ConfigureAwait(false);
                GC.Collect();
            }
            else if (Context.Guild.Id == 689119429375819951) await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
            GC.Collect();
        }
    }
}
