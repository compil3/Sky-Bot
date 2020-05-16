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
    public class PlayerStats : ModuleBase
    {
        [Command("ps")]
        [Summary(
            ".ps Web-Site-UserName SeasonNumber (optional)\n Eg: .ps SpillShot 7\n If the website name has spaces try wrapping the name (.ps \"Name tag\" ")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task GetPlayerStatsLG(string playerLookup, string seasonType = null, string seasonId = null)
        {
            var tableName = "";
            var dbName = "";
            var outPutSeason = "";
            var guildID = Context.Guild.Id;
            
            if (guildID == 689119429375819951)
            {
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: Player.GetPlayer(playerLookup, seasonType, seasonId)).ConfigureAwait(false);
                GC.Collect();
            }
            else if (Context.Guild.Id == 689119429375819951) await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
        }
    }
}
