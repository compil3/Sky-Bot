using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Serilog;
using LGFA.Engines;
using LGFA.Engines.Current;
using LGFA.Modules.Helpers;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    internal class TeamStats : ModuleBase
    {
        [Command("ts")]
        [Summary("Retrieves the current season statistics for the team entered.")]
        public async Task ClubStats(string teamName, string league)
        {
            //var options = new RequestOptions { Timeout = 2 };
            //await Context.Message.DeleteAsync(options);
            
            EmbedBuilder embed = new EmbedBuilder();
            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
              TeamInfo.ClubInfo(league, teamName, ref embed);
              await ReplyAsync("", embed: embed.Build());
            }
            else
                await ReplyAsync(
                    $"{Context.User.Mention} you are using the command in the wrong channel, try again in " +
                    $"{MentionUtils.MentionChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))}");
        }
    }
}