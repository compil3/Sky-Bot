using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines;
using LGFA.Engines.Current;
using LGFA.Extensions;
using LGFA.Modules.Helpers;
using Serilog;

namespace LGFA.Modules
{
    public class Table : ModuleBase
    {
        [Command("table")]
        [Alias("standings")]
        [Summary("Retrieve the current season standings of Xbox or PSN.")]
        [RequireUserPermission(GuildPermission.SendMessages)]
        public async Task Standings(string league)
        {
            EmbedBuilder embed = new EmbedBuilder();
            var options = new RequestOptions {Timeout = 2};
            await Context.Message.DeleteAsync(options);

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
               TeamStanding.GetStandings(league, ref embed,"Standings");
               await ReplyAsync("", embed: embed.Build());
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