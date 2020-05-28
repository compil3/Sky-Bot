using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Serilog;

namespace LGFA.Modules
{
    internal class LastFiveStats : ModuleBase
    {
        [Command("l5")]
        [Summary(
            ".l5 GamerTag\nEg: .l5 GingaNinja\nIf the Gamertag (or part of) does not work try wrapping the name in quotes (.l5 \"A GingaNinja12\"")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task GetLastFive(string playerLookup)
        {
            var guildId = Context.Guild.Id;

            if (guildId == 689119429375819951)
            {
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                //await Context.Channel.SendMessageAsync(null, embed: Recent.GetLastFive(playerLookup)).ConfigureAwait(false);
                GC.Collect();
            }
            else if (Context.Guild.Id == 689119429375819951)
            {
                await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
            }

            GC.Collect();
        }
    }
}