using System.Collections.Generic;
using Discord;
using LGFA.Properties;

namespace LGFA.Engines.Current.Player
{
    internal class PlayerEmbed
    {
        public static Embed VirtualSeason(List<SeasonProperties> table)
        {
            Embed embed = null;
            EmbedBuilder builder = null;

            foreach (var player in table)
            {
                builder = new EmbedBuilder()
                    .WithAuthor(author =>
                    {
                        author
                            .WithName($"{player.System} - {player.SeasonId} Stats")
                            .WithIconUrl(player.SystemIcon);
                    })
                    .WithTitle($"Statistics for {player.PlayerName.Trim()} - {player.Position.Trim()}")
                    .WithUrl(player.PlayerUrl)
                    .WithColor(new Color(0x26A20B))
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("leaguegaming.com")
                            .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                    })
                    .WithThumbnailUrl(player.TeamIcon)
                    .AddField("\u200B", "```Record```")
                    .AddField("GP", player.GamesPlayed, true)
                    .AddField("Record (W-D-L)", player.Record, true)
                    .AddField("AMR", player.AvgMatchRating, true)
                    .AddField("\u200B", "```Offensive Stats```")
                    .AddField("Goals", player.Goals, true)
                    .AddField("G/Game", player.GoalsPerGame, true)
                    .AddField("Shots - SOT", player.ShotSot, true)
                    .AddField("S/Game", player.ShotPerGame, true)
                    .AddField("Shots/Goal - SH%", player.ShotPerGoal, true)
                    .AddField("\u200B", "```Passing Stats```")
                    .AddField("Assists", player.Assists, true)
                    .AddField("Pass - Pass Attempts", player.PassRecord, true)
                    .AddField("Key Passes", player.KeyPasses, true)
                    .AddField("Assist/Game", player.AssistPerGame, true)
                    .AddField("P/Game - Pass %", player.PassPerGame, true)
                    .AddField("Key Pass/Game", player.KeyPassPerGame, true)
                    .AddField("\u200B", "```Defensive Stats```")
                    .AddField("Tackles - Tackle Attempts", player.Tackling, true)
                    .AddField("Tackles Per Game", player.TacklesPerGame, true)
                    .AddField("Tackle Success", player.TacklePercent, true)
                    .AddField("PossW-PossL", player.Poss, true)
                    .AddField("Int-Blk", player.Wall, true)
                    .AddField("\u200B", "```Discipline```")
                    .AddField("YC-RC", player.Discipline, true);
                embed = builder.Build();
            }

            return embed;
        }
    }
}