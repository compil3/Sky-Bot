using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines.Current.Goalie;
using LGFA.Properties;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class Goalie : ModuleBase
    {
        [Command("gs")]
        [Summary("Get a goalies current statistics. [Command not yet implemented]")]
        public async Task GetGoalieStats(string playerLookup)
        {
            var options = new RequestOptions { Timeout = 2 };
            await Context.Message.DeleteAsync(options);
            EmbedBuilder embed = new EmbedBuilder();

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
                var (goalie, playerName) = CurrentSeason.SeasonStats(playerLookup);
                if (goalie == null || !goalie.Any())
                {
                    await ReplyAsync($"{Context.User.Mention}, it doesn't appear that {playerName} is a goalie or there are no regular season stats.");
                    return;
                }
                else
                {
                    GoalieBuilder(goalie, ref embed);
                    //embed.Author = new EmbedAuthorBuilder()
                    //    .WithName("[Stats Provided by Leaguegaming.com]");
                    embed.Footer = new EmbedFooterBuilder
                    {
                        Text = "leaguegaming.com",
                        IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                    };
                    foreach (var gStat in goalie)
                    {
                        embed.Title = $"{gStat.playerName} POS: G";
                        embed.WithUrl(gStat.playerURL);
                        embed.Color = new Color(0x26A20B);
                        embed.WithCurrentTimestamp();
                    }

                    await ReplyAsync("", embed: embed.Build());
                }
            }
        }

        private void GoalieBuilder(List<GoalieProperties> goalie, ref EmbedBuilder builder)
        {
            foreach (var gStat in goalie)
            {
                builder.Author = new EmbedAuthorBuilder()
                    .WithName("[Stats Provided by Leaguegaming.com]")
                    .WithIconUrl(gStat.teamIcon);

                builder.AddField("\u200B", "```Record```");
                builder.AddField("GP", gStat.gamesPlayed,true);
                builder.AddField("Record (W-D-L)", gStat.record, true);
                builder.AddField("AMR", gStat.avgMatchRating, true);
                builder.AddField("\u200B", "```Goals```");
                builder.AddField("GA", gStat.goalsAgainst, true);
                builder.AddField("GAA", gStat.goalsAgainstAvg, true);
                builder.AddField("\u200b", "```Saves```");
                builder.AddField("Saves", gStat.saves, true);
                builder.AddField("SA", gStat.shotsAgainst, true);
                builder.AddField("Save %", gStat.savePercentage, true);
                builder.AddField("\u200B", "```Other```");
                builder.AddField("CS", gStat.cleanSheets, true);
                builder.AddField("MOTM", gStat.manOfTheMatch, true);
            };
            
        }
    }
}
