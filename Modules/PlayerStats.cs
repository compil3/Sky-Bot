using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LGFA.Engines;
using LiteDB;
using Serilog;
using LGFA.Properties;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class PlayerStats : ModuleBase
    {
        [Command("ps")]
        [Summary(
            ".ps Gamertag Eg: .ps SpillShot 7\n If the website name has spaces try wrapping the name (.ps \"Name tag\" ")]
        public async Task GetPlayerStatsLG(string playerLookup, string seasonType = null, string seasonId = null)
        {
            if (Context.Guild.Id == 689119429375819951 || Context.Guild.Id == 688840425162801197 )
            {
                Log.Logger.Warning($"Guild: {Context.Guild.Name}\nID:{Context.Guild.Id}");
                if (Context.Channel.Id == 711778374720421918 || Context.Channel.Id == 713176040716894208 || Context.Channel.Id == 713237102145437776)
                {
                    Log.Logger.Warning($"Channel: {Context.Channel.Name}\nID: {Context.Channel.Id}");
                    Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                    await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``",
                        embed: Player.GetPlayer(playerLookup, seasonType, seasonId)).ConfigureAwait(false);
                }
                else
                    await ReplyAsync(
                        $"Channel permission denied.  Try again in the proper channel {Context.User.Mention}");
            }
        }
    }
}
