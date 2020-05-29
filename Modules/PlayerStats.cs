using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines.Current.Player;
using LGFA.Properties;
using Serilog;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class PlayerStats : ModuleBase
    {
        [Command("ps")]
        [Summary("Get the players current season statistics.")]
        public async Task GetPlayerStatsLg(string playerLookup, string seasonType = null, string seasonId = null)
        {
            var options = new RequestOptions { Timeout = 2 };
            await Context.Message.DeleteAsync(options);

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")) || Context.Channel.Id == 711778374720421918 || Context.Channel.Id == 713176040716894208)
            {
                Log.Logger.Warning($"Guild: {Context.Guild.Name} ID:{Context.Guild.Id}");
                var player = CurrentSeason.SeasonStats(playerLookup);
                var embed = SeasonEmbed(player);
                await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: embed)
                    .ConfigureAwait(false);
            }
            else
            {

                await ReplyAsync(
                    $"{Context.User.Mention} you are using the command in the wrong channel, try again in " +
                    $"{MentionUtils.MentionChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))}");
            }
        }

        private Embed SeasonEmbed(List<CareerProperties> player)
        {
            Embed embed = null;

            foreach (var pStat in player)
            {
                var builder = new EmbedBuilder()
                    .WithTitle($"{pStat.PlayerName} (Recent POS: {pStat.Position.Trim()})")
                    .WithUrl(pStat.PlayerUrl)
                    .WithColor(new Color(0x26A20B))
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("leaguegaming.com")
                            .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                    })
                    .WithAuthor(author =>
                    {
                        author
                            .WithName($"{pStat.System.ToUpper()} {pStat.SeasonId} Stats")
                            .WithIconUrl(pStat.SystemIcon);
                    })
                    .AddField("\u200B", "```Record```")
                    .AddField("GP", pStat.GamesPlayed, true)
                    .AddField("Record (W-D-L)", pStat.Record, true)
                    .AddField("AMR", pStat.MatchRating, true)
                    .AddField("\u200B", "```Offensive Stats```")
                    .AddField("Goals", pStat.Goals, true)
                    .AddField("G/Game", pStat.GoalsPerGame, true)
                    .AddField("Shots - SOT", pStat.ShotSot, true)
                    .AddField("S/Game", pStat.ShotPerGame, true)
                    .AddField("Shots/Goal - SH%", pStat.ShotPerGoal, true)
                    .AddField("\u200B", "```Passing Stats```")
                    .AddField("Assists", pStat.Assists, true)
                    .AddField("Pass - Pass Attempts", pStat.PassRecord, true)
                    .AddField("Key Passes", pStat.KeyPasses, true)
                    .AddField("AST/Game", pStat.AssistPerGame, true)
                    .AddField("P/Game - Pass %", pStat.PassPerGame, true)
                    .AddField("Key Pass/Game", pStat.KeyPassPerGame, true)
                    .AddField("\u200B", "```Defensive Stats```")
                    .AddField("Tac - Tac Att", pStat.Tackling, true)
                    .AddField("Tac/Game", pStat.TacklesPerGame, true)
                    .AddField("Tac %", pStat.TacklePercent, true)
                    .AddField("PossW-PossL", pStat.Poss, true)
                    .AddField("Int-Blk", pStat.Wall, true)
                    .AddField("\u200B", "```Discipline```")
                    .AddField("YC-RC", pStat.Discipline, true);
                embed = builder.Build();
            }

            return embed;
        }
    }
}