using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Discord;
using Sky_Bot.Properties;

namespace Sky_Bot.Modules.Helpers
{
    public class SeasonHelper
    {
        public static Embed SeasonEmbed(string playerName, string userSystem, string systemIcon, string record, string amr, string goals, string assists, string sot, string shots, string passC, string passA,
            string key, string interceptions, string tac, string tacA, string possW, string possL, string blks, string rc, string yc, string seasonId, string playerUrl, string teamIcon, string position)
        {

            EmbedBuilder builder = null;

            #region stat compression

            SeasonProperties sStat = new SeasonProperties
            {
                //Record
                GamesPlayed = record,
                Record = record,
                AvgMatchRating = amr,

                //Offensive
                Goals = Convert.ToDouble(goals),
                Assists = assists,
                GoalsAssist = "0",
                GoalsPerGame = "0",

                ShotAttempts = shots,
                ShotsOnTarget = sot,
                ShotPercentage = "0",
                ShotPerGame = "0",
                ShotPerGoal = "0",
                ShotSot = "0",

                PassesAttempted = passA,
                PassesCompleted = passC,
                PassRecord = "0",
                KeyPasses = key,
                KeyPassPerGame = "0",
                PassingPercentage = "0",
                PassPerGame = "0",
                AssistPerGame = "0",

                Interceptions = interceptions,
                Blocks = blks,
                TackleAttempts = tacA,
                Tackles = tac,
                Wall = "0",
                Tackling = "0",
                TacklePercent = "0",
                TacklesPerGame = "0",
                InterPerGame = "0",
                BlocksPerGame = "0",

                PossW = possW,
                PossL = possL,
                Poss = "",

                YellowCards = yc,
                RedCards = rc,
                Discipline =  "0",
            };
            

            #endregion
            #region Builder
            builder = new EmbedBuilder()
                .WithTitle($"Statistics for ***{playerName}***  (Recent Pos: {position.Trim()})")
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                })
                .WithThumbnailUrl(teamIcon)
                .WithAuthor(author =>
                {
                    author
                        .WithName($"{userSystem.ToUpper()} Season {seasonId} Stats.") // provided by Sky Sports.")
                        .WithIconUrl(systemIcon);
                })
                .AddField("\u200B", $"```Record```", false)
                .AddField("GP", sStat.GamesPlayed, true)
                .AddField("Record (W-D-L)", sStat.Record, true)
                .AddField("AMR", sStat.AvgMatchRating, true)

                .AddField("\u200B", "```Offensive Stats```", false)
                .AddField("Goals", sStat.Goals, true)
                .AddField("G/Game", sStat.GoalsPerGame, true)
                .AddField("Shots - SOT", sStat.ShotSot, true)
                .AddField("S/Game", sStat.ShotPerGame, true)
                .AddField("Shots/Goal - SH%", sStat.ShotPerGoal, true)

                .AddField("\u200B", "```Passing Stats```", false)
                .AddField("Assists", sStat.Assists,true)
                .AddField("Pass - Pass Attempts", sStat.PassRecord, true)
                .AddField("Key Passes", sStat.KeyPasses, true)
                .AddField("Assist/Game", sStat.AssistPerGame, true)
                .AddField("P/Game - Pass %", sStat.PassPerGame,true)
                .AddField("Key Pass/Game", sStat.KeyPassPerGame, true)

                .AddField("\u200B", "```Defensive Stats```", false)
                .AddField("Tackles - Tackle Attempts", sStat.Tackling, true)
                .AddField("Tackles Per Game", sStat.TacklesPerGame, true)
                .AddField("Tackle Success", sStat.TacklePercent, true)
                .AddField("PossW-PossL",sStat.Poss, true)
                .AddField("Int-Blk", sStat.Wall, true)

                .AddField("\u200B", "```Discipline```", false)
                .AddField("YC-RC", sStat.Discipline, true);
            var embed = builder.Build();
            #endregion
            return embed;
        }
    }
}
