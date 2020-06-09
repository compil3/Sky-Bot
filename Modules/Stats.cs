using System;
using System.Collections.Generic;
using System.Linq;
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
    public class Player : ModuleBase
    {
        [Command("ps")]
        [Summary("Gets a player's statistics for the current season.")]
        public async Task PlayerStats([Remainder] string playerLookup)
        {
            EmbedBuilder embed = new EmbedBuilder();
            var options = new RequestOptions { Timeout = 2 };
            await Context.Message.DeleteAsync(options);

            try
            {
                if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")) || Context.Channel.Id == 711778374720421918 || Context.Channel.Id == 713176040716894208)
                {
                    var (player, playerName) = CurrentSeason.SeasonStats(playerLookup);
                    if (player == null || !player.Any())
                    {
                        await ReplyAsync($"{Context.User.Mention}, it doesn't appear that ``{playerName}`` has any stats for the current regular season.");
                        return;
                    }
                    else
                    {
                        PlayerBuilder(player, ref embed);
                        embed.Footer = new EmbedFooterBuilder
                        {
                            Text = "leaguegaming.com",
                            IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                        };
                        foreach (var pStat in player)
                        {
                            embed.Title = $"{pStat.PlayerName} (Recent POS: {pStat.Position.Trim()})";
                            embed.WithUrl(pStat.PlayerUrl);
                            embed.Color= new Color(0x26A20B);
                            embed.WithCurrentTimestamp();
                        }

                        await ReplyAsync("", embed: embed.Build());
                        ////var embed = SeasonEmbed(player);

                        //await Context.Channel
                        //    .SendMessageAsync($"{Context.User.Mention}\n``[Stats Provided by Leaguegaming.com]``",
                        //        embed: embed)
                        //    .ConfigureAwait(false);
                    }
                }
                else
                {

                    await ReplyAsync(
                        $"{Context.User.Mention} you are using the command in the wrong channel, try again in " +
                        $"{MentionUtils.MentionChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))}");
                }
            }
            catch (NullReferenceException)
            {
                await ReplyAsync(
                    $"{Context.User.Mention} there was an error finding {playerLookup}.  Please try again using quotes around the gamer tag.");
            }
           
        }

        private void PlayerBuilder(List<CareerProperties> player, ref EmbedBuilder builder)
        {
            foreach (var pStat in player)
            {
                builder.Author = new EmbedAuthorBuilder()
                    .WithName("[Stats provided by Leaguegaming.com]")
                    .WithIconUrl(pStat.TeamIcon);
                builder.AddField("\u200B", "```Record```");
                builder.AddField("GP", pStat.GamesPlayed, true);
                builder.AddField("Record (W-D-L)", pStat.Record, true);
                builder.AddField("AMR", pStat.MatchRating, true);
                builder.AddField("\u200B", "```Offensive Stats```");
                builder.AddField("Goals", pStat.Goals, true);
                builder.AddField("G/Game", pStat.GoalsPerGame, true);
                builder.AddField("Shots - SOT", pStat.ShotPerGame, true);
                builder.AddField("Shots/Goal - SH%", pStat.ShotPerGoal, true);
                builder.AddField("\u200B", "```Passing Stats```");
                builder.AddField("Assists", pStat.Assists, true);
                builder.AddField("Pass - Pass Attempts", pStat.PassRecord, true);
                builder.AddField("Key Passes", pStat.KeyPasses, true);
                builder.AddField("AST/Game", pStat.AssistPerGame, true);
                builder.AddField("P/Game - Pass %", pStat.PassPerGame, true);
                builder.AddField("Key Pass/Game", pStat.KeyPassPerGame, true);
                builder.AddField("\u200B", "```Defensive Stats```");
                builder.AddField("Tac - Tac Att", pStat.Tackling, true);
                builder.AddField("Tac/Game", pStat.TacklesPerGame, true);
                builder.AddField("PossW-PossL", pStat.Poss, true);
                builder.AddField("Int-Blk", pStat.Wall, true);
                builder.AddField("\u200B", "```Discipline```");
                builder.AddField("YC-RC", pStat.Discipline, true);
            }
        }

        private static Embed SeasonEmbed(List<CareerProperties> player)
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
                            .WithIconUrl(pStat.TeamIcon);
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