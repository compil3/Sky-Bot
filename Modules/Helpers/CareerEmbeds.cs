using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Discord;
using LGFA.Properties;

namespace LGFA.Modules.Helpers
{
    public class CareerEmbeds
    {
        public static Embed CareerSeasonalEmbed(string foundPlayerName, string foundPlayerUrl, string record, string amr, string goals, string assists, 
            string sot, string shots, string passC, string passA, string keypass, string interceptions, string tac, string tacA, string possW, string possL, string blk, string rc, string yc, string type, string seasonId)
        {
           EmbedBuilder builder = null;
            Embed embed = null;
            var offense = "";
            if (type == "Reg") type = "Regular";
            var seasonSplit = seasonId.Replace("S", "");
            //var stats = Compressor(record, amr, goals, assists, sot, shots, passC, passA, key, intercept, tac, tacA, blk, rc, yc
            CareerProperties cStat = new CareerProperties
            {
                SeasonType = type,
                //Record
                GamesPlayed = record,
                Record = record,
                AvgMatchRating = amr,

                //Offensive
                Goals = goals,
                Assists = assists,
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
                KeyPasses = keypass,
                KeyPassPerGame = "0",
                PassingPercentage = "0",
                PassPerGame = "0",
                AssistPerGame = "0",
                
                Interceptions = interceptions,
                Blocks = blk,
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
                Poss = "0",


                YellowCards = yc,
                RedCards = rc,
                Discipline =  "0",
            };

            builder = new EmbedBuilder()
                .WithTitle($"{foundPlayerName} - {cStat.SeasonType} Season {seasonSplit} Statistics.")
                .WithUrl(foundPlayerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                })
                .AddField($"\u200B", $"```Record```", false)
                .AddField("GP", cStat.GamesPlayed, true)
                .AddField("Record (W-D-L)", cStat.Record, true)
                .AddField("AMR", cStat.AvgMatchRating, true)

                .AddField("\u200B", $"```Offensive Stats```", false)
                .AddField("Goals", cStat.Goals, true)
                .AddField("G/Game", cStat.GoalsPerGame, true)
                .AddField("Shots - SOT", cStat.ShotSot, true)
                .AddField("S/Game", cStat.ShotPerGame, true)
                .AddField("Shots/Goal - SH%", cStat.ShotPerGoal, true)
               // .AddField("SH%", cStat.ShotPercentage,true)

                .AddField("\u200B", $"```Passing Stats```", false)
                .AddField("Assists", cStat.Assists,true)
                .AddField("Pass - Pass Attempts", cStat.PassRecord, true)
                .AddField("Key Passes", cStat.KeyPasses, true)
                .AddField("Assist/Game", cStat.AssistPerGame, true)
                .AddField("P/Game - Pass %", cStat.PassPerGame,true)

                //.AddField("Pass %", cStat.PassingPercentage, true)
                .AddField("Key Pass/Game", cStat.KeyPassPerGame, true)

                .AddField("\u200B", $"```Defensive Stats```", false)
                .AddField("Tackles - Tackle Attempts", cStat.Tackling, true)
                .AddField("Tackles Per Game", cStat.TacklesPerGame, true)
                .AddField("Tackle Success", cStat.TacklePercent, true)
                .AddField("PossW-PossL", cStat.Poss, true)
                .AddField("Int-Blk", cStat.Wall, true)

                .AddField("\u200B", "```Discipline```", false)
                .AddField("YC-RC", cStat.Discipline, true);
             embed = builder.Build();
            return embed;
        }
        //string foundPlayerName, string foundPlayerUrl, string playerRecord, string playerAvgMatchRating, string playerGoals, string playerAssists, 
        //string playerShotsOnTarget, string playerShotAttempts, string playerPassesCompleted, string playerPassesAttempted, string playerKeyPasses, string playerInterceptions, string playerTackles, 
        //string playerTackleAttempts, string playerPossW, string playerPossL, string playerBlocks, string playerRedCards, string playerYellowCards

        public static List<CareerProperties> CareerSeasonEmbed(List<CareerProperties> career)
        {
            EmbedBuilder builder = null;
            Embed embed = null;
            var offense = "";


            //CareerProperties cStat = new CareerProperties
            //{
            //    //Record
            //    GamesPlayed = careerplayerRecord,
            //    Record = playerRecord,
            //    AvgMatchRating = playerAvgMatchRating,

            //    Offensive
            //    Goals = playerGoals,
            //    Assists = playerAssists,
            //    GoalsAssist = "0",
            //    GoalsPerGame = "0",

            //    ShotAttempts = playerShotAttempts,
            //    ShotsOnTarget = playerShotsOnTarget,
            //    ShotPercentage = "0",
            //    ShotPerGame = "0",
            //    ShotPerGoal = "0",
            //    ShotSot = "0",

            //    PassesAttempted = playerPassesAttempted,
            //    PassesCompleted = playerPassesCompleted,
            //    PassRecord = "0",
            //    KeyPasses = playerKeyPasses,
            //    KeyPassPerGame = "0",
            //    PassingPercentage = "0",
            //    PassPerGame = "0",
            //    AssistPerGame = "0",

            //    Interceptions = playerInterceptions,
            //    PossW = playerPossW,
            //    PossL = playerPossL,
            //    Poss = "0",
            //    Blocks = playerBlocks,
            //    TackleAttempts = playerTackleAttempts,
            //    Tackles = playerTackles,
            //    Wall = "0",
            //    Tackling = "0",
            //    TacklePercent = "0",
            //    TacklesPerGame = "0",
            //    InterPerGame = "0",
            //    BlocksPerGame = "0",

            //    YellowCards = playerYellowCards,
            //    RedCards = playerRedCards,
            //    Discipline = "0",
            //};

            //builder = new EmbedBuilder()
            //    .WithTitle($"{foundPlayerName} - CareerBuilder Statistics.")
            //    .WithUrl(foundPlayerUrl)
            //    .WithColor(new Color(0x26A20B))
            //    .WithCurrentTimestamp()
            //    .WithFooter(footer =>
            //    {
            //        footer
            //            .WithText("leaguegaming.com")
            //            .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
            //    })
            //    .AddField($"\u200B", $"```Record```", false)
            //    .AddField("GP", cStat.GamesPlayed, true)
            //    .AddField("Record (W-D-L)", cStat.Record, true)
            //    .AddField("AMR", cStat.AvgMatchRating, true)

            //    .AddField("\u200B", $"```Offensive Stats```", false)
            //    .AddField("Goals", cStat.Goals, true)
            //    .AddField("G/Game", cStat.GoalsPerGame, true)
            //    .AddField("Shots - SOT", cStat.ShotSot, true)
            //    .AddField("S/Game", cStat.ShotPerGame, true)
            //    .AddField("Shots/Goal - SH%", cStat.ShotPerGoal, true)
            //   // .AddField("SH%", cStat.ShotPercentage,true)

            //    .AddField("\u200B", $"```Passing Stats```", false)
            //    .AddField("Assists", cStat.Assists,true)
            //    .AddField("Pass - Pass Attempts", cStat.PassRecord, true)
            //    .AddField("Key Passes", cStat.KeyPasses, true)
            //    .AddField("Assist/Game", cStat.AssistPerGame, true)
            //    .AddField("P/Game - Pass %", cStat.PassPerGame,true)

            //    //.AddField("Pass %", cStat.PassingPercentage, true)
            //    .AddField("Key Pass/Game", cStat.KeyPassPerGame, true)

            //    .AddField("\u200B", $"```Defensive Stats```", false)
            //    .AddField("Tackles - Tackle Attempts", cStat.Tackling, true)
            //    .AddField("Tackles Per Game", cStat.TacklesPerGame, true)
            //    .AddField("Tackle Success", cStat.TacklePercent, true)
            //    .AddField("PossW-PossL", cStat.Poss, true)
            //    .AddField("Int-Blk", cStat.Wall, true)

            //    .AddField("\u200B", "```Discipline```", false)
            //    .AddField("YC-RC", cStat.Discipline, true);
            // embed = builder.Build();
            //return embed;
            return null;
        }
    }
}
