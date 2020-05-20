using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Discord;
using LGFA.Properties;
using Serilog;

namespace LGFA.Engines.Career
{
    class CareerEmbed
    {
        public static Embed VirtualCareerEmbed(List<CareerProperties> table)
        {
            Embed embed = null;
            EmbedBuilder builder = null;
            foreach (var vStat in table)
            {
                builder = new EmbedBuilder()
                    .WithTitle($"{vStat.PlayerName} - Career Statistics")
                    .WithUrl(vStat.PlayerUrl)
                    .WithColor(new Color(0x26A20B))
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("leaguegaming.com")
                            .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                    })
                    .AddField($"\u200B", $"```Record```", false)
                    .AddField("GP", vStat.GamesPlayed, true)
                    .AddField("Record (W-D-L)", vStat.Record, true)
                    .AddField("AMR", vStat.AvgMatchRating, true)

                    .AddField("\u200B", $"```Offensive Stats```", false)
                    .AddField("Goals", vStat.Goals, true)
                    .AddField("G/Game", vStat.GoalsPerGame, true)
                    .AddField("Shots - SOT", vStat.ShotSot, true)
                    .AddField("S/Game", vStat.ShotPerGame, true)
                    .AddField("Shots/Goal - SH%", vStat.ShotPerGoal, true)

                    .AddField("\u200B", $"```Passing Stats```", false)
                    .AddField("Assists", vStat.Assists, true)
                    .AddField("Pass - Pass Attempts", vStat.PassRecord, true)
                    .AddField("Key Passes", vStat.KeyPasses, true)
                    .AddField("Assist/Game", vStat.AssistPerGame, true)
                    .AddField("P/Game - Pass %", vStat.PassPerGame, true)

                    .AddField("Key Pass/Game", vStat.KeyPassPerGame, true)

                    .AddField("\u200B", $"```Defensive Stats```", false)
                    .AddField("Tackles - Tackle Attempts", vStat.Tackling, true)
                    .AddField("Tackles Per Game", vStat.TacklesPerGame, true)
                    .AddField("Tackle Success", vStat.TacklePercent, true)
                    .AddField("PossW-PossL", vStat.Poss, true)
                    .AddField("Int-Blk", vStat.Wall, true)

                    .AddField("\u200B", "```Discipline```", false)
                    .AddField("YC-RC", vStat.Discipline, true);
                embed = builder.Build();
            }
            return embed;

        }

        public static Embed VirtualSeasonEmbed(List<CareerProperties> table)
        {
            Embed embed = null;
            EmbedBuilder builder = null;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            foreach (var vStat in table)
            {
                builder = new EmbedBuilder()
                    .WithTitle($"{vStat.PlayerName} - {vStat.SeasonId} Statistics")
                    .WithUrl(vStat.PlayerUrl)
                    .WithColor(new Color(0x26A20B))
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("leaguegaming.com")
                            .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                    })
                    .AddField($"\u200B", $"```Record```", false)
                    .AddField("GP", vStat.GamesPlayed, true)
                    .AddField("Record (W-D-L)", vStat.Record, true)
                    .AddField("AMR", vStat.MatchRating, true)

                    .AddField("\u200B", $"```Offensive Stats```", false)
                    .AddField("Goals", vStat.Goals, true)
                    .AddField("G/Game", vStat.GoalsPerGame, true)
                    .AddField("Shots - SOT", vStat.ShotSot, true)
                    .AddField("S/Game", vStat.ShotPerGame, true)
                    .AddField("Shots/Goal - SH%", vStat.ShotPerGoal, true)

                    .AddField("\u200B", $"```Passing Stats```", false)
                    .AddField("Assists", vStat.Assists, true)
                    .AddField("Pass - Pass Attempts", vStat.PassRecord, true)
                    .AddField("Key Passes", vStat.KeyPasses, true)
                    .AddField("Assist/Game", vStat.AssistPerGame, true)
                    .AddField("P/Game - Pass %", vStat.PassPerGame, true)

                    .AddField("Key Pass/Game", vStat.KeyPassPerGame, true)

                    .AddField("\u200B", $"```Defensive Stats```", false)
                    .AddField("Tackles - Tackle Attempts", vStat.Tackling, true)
                    .AddField("Tackles Per Game", vStat.TacklesPerGame, true)
                    .AddField("Tackle Success", vStat.TacklePercent, true)
                    .AddField("PossW-PossL", vStat.Poss, true)
                    .AddField("Int-Blk", vStat.Wall, true)

                    .AddField("\u200B", "```Discipline```", false)
                    .AddField("YC-RC", vStat.Discipline, true);
                embed = builder.Build();
            }
            stopWatch.Stop();
            Log.Logger.Warning($"CareerSeasonEmbed: {stopWatch.Elapsed}");
            return embed;
        }
    }
}
