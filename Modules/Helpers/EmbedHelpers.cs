using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Discord;
using LGFA.Properties;
using Microsoft.Extensions.Primitives;

namespace LGFA.Modules
{
    public class EmbedHelpers
    {
        public static Embed SeasonEmbed(string playerName, string userSystem, string systemIcon, string record, string amr, string goals, string assists, string sot, string shots, string passC, string passA,
            string key, string interceptions, string tac, string tacA, string blks, string rc, string yc, string seasonId, string playerUrl, string teamIcon, string position)
        {

            EmbedBuilder builder = null;

            #region stat compression
            
            string[] scoring = new string[3];
            scoring[0] = goals;
            scoring[1] = assists;
            scoring[2] = key;
            var scoringRecord = string.Join(" - ", scoring);


            string[] recordStrip = record.Split('-');
            var wins = int.Parse(recordStrip[0]);
            var draws = int.Parse(recordStrip[1]);
            var loses = int.Parse(recordStrip[2]);
            var gamesPlayed = wins + draws + loses;

            decimal shotPercentage = 0;
            string[] shooting = new string[3];
            shooting[0] = shots;
            shooting[1] = sot;
            if (shots == "0" & sot == "0") shotPercentage = 0;
            else shotPercentage = Convert.ToDecimal(sot) / Convert.ToDecimal(shots);
            shooting[2] = shotPercentage.ToString("P", CultureInfo.InvariantCulture);
            var shootingRecord = string.Join(" - ", shooting);

            string[] tackling = new string[3];
            tackling[0] = tac;
            tackling[1] = tacA;
            var tempTacPerc = Convert.ToDecimal(tac) / Convert.ToDecimal(tacA);
            tackling[2] = tempTacPerc.ToString("P", CultureInfo.InvariantCulture);
            var tackleRecord = string.Join(" - ", tackling);

            string[] passing = new string[3];
            passing[0] = passC;
            passing[1] = passA;
            var tempPassPerc = Convert.ToDecimal(passC) / Convert.ToDecimal(passA);
            passing[2] = tempPassPerc.ToString("P", CultureInfo.InvariantCulture);
            var passingRecord = string.Join(" - ", passing);

            var defensive = new string[2];
            defensive[0] = interceptions;
            defensive[1] = blks;
            var defensiveRecord = string.Join(" - ", defensive);

            var discipline = new string[2];
            discipline[0] = yc;
            discipline[1] = rc;
            var disciplineRecord = string.Join(" - ", discipline);

            #endregion
            #region Builder
            builder = new EmbedBuilder()
                .WithTitle($"Statistics for ***{playerName}*** ({position.Trim()})")
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
                .AddField("GP", gamesPlayed, true)
                .AddField("Record (W-D-L)", record, true)
                .AddField("AMR", amr, true)
                .AddField("G-A-Key", scoringRecord, true)
                .AddField("SOG-SOT-SH%", shootingRecord, true)
                .AddField("TK-TKA-TK%", tackleRecord, true)
                .AddField("Passing (PC-PA-P%)", passingRecord, true)
                .AddField("Defensive (Int-Blks)", defensiveRecord, true)
                .AddField("Discipline (YC-RC)", disciplineRecord, true);
            var embed = builder.Build();
            #endregion
            return embed;
        }

        public static Embed CareerEmbed(string playerName, string playerUrl, string record, string amr, string goals, string assists, string sot, string shots, string passC, string passA, string key, string intercept, string tac,
            string tacA, string blk, string rc, string yc)
        {
            EmbedBuilder builder = null;
            Embed embed = null;
            var offense = "";
            //var stats = Compressor(record, amr, goals, assists, sot, shots, passC, passA, key, intercept, tac, tacA, blk, rc, yc);
            CareerProperties cStat = new CareerProperties
            {
                //Record
                GamesPlayed = record,
                Record = record,
                AvgMatchRating = amr,

                //Offensive
                Goals = goals,
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
                
                Interceptions = intercept,
                Blocks = blk,
                TackleAttempts = tacA,
                Tackles = tac,
                Wall = "0",
                Tackling = "0",
                TacklePercent = "0",
                TacklesPerGame = "0",
                InterPerGame = "0",
                BlocksPerGame = "0",

                YellowCards = yc,
                RedCards = rc,
                Discipline =  "0",
            };

            string[] scoring = new string[3];
            scoring[0] = cStat.Goals.ToString(CultureInfo.InvariantCulture);
            scoring[1] = cStat.Assists;
            scoring[2] = cStat.KeyPasses;
            offense = string.Join("-", scoring);

            string[] shooting = new string[3];
            shooting[0] = cStat.ShotAttempts;
            shooting[1] = cStat.ShotsOnTarget;
            var recordBreak = "Record";
            var dotLine = "---";

            //foreach (var stat in stats)
            //{
            builder = new EmbedBuilder()
                .WithTitle($"Career stats: {playerName}")
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                })
                .AddField($"\u200B", "```Career Record```", false)
                .AddField("GP", cStat.GamesPlayed, true)
                .AddField("Record (W-D-L)", cStat.Record, true)
                .AddField("AMR", cStat.AvgMatchRating, true)

                .AddField("\u200B", "```Career Offensive Stats```", false)
                .AddField("Goals", cStat.Goals, true)
                .AddField("G/Game", cStat.GoalsPerGame, true)
                .AddField("Shots - SOT", cStat.ShotSot, true)
                .AddField("S/Game", cStat.ShotPerGame, true)
                .AddField("Shots/Goal - SH%", cStat.ShotPerGoal, true)
               // .AddField("SH%", cStat.ShotPercentage,true)

                .AddField("\u200B", "```Career Passing Stats```", false)
                .AddField("Assists", cStat.Assists,true)
                .AddField("Pass - Pass Attempts", cStat.PassRecord, true)
                .AddField("Key Passes", cStat.KeyPasses, true)
                .AddField("Assist/Game", cStat.AssistPerGame, true)
                .AddField("P/Game - Pass %", cStat.PassPerGame,true)

                //.AddField("Pass %", cStat.PassingPercentage, true)
                .AddField("Key Pass/Game", cStat.KeyPassPerGame, true)

                .AddField("\u200B", "```Career Defensive Stats```", false)
                .AddField("Tackles - Tackle Attempts", cStat.Tackling, true)
                .AddField("Tackles Per Game", cStat.TacklesPerGame, true)
                .AddField("Tackle Success", cStat.TacklePercent, true)
                .AddField("Int-Blk", cStat.Wall, true)

                .AddField("\u200B", "```Career Discipline```", false)
                .AddField("YC-RC", cStat.Discipline, true);
                 //.AddField("Discipline (YC-RC)",discipline, true);
             embed = builder.Build();
            //}

            return embed;
        }

        internal static Embed CareerEmbed(string foundPlayerName, string foundPlayerUrl, string record, string amr, string goals, string assists, 
            string sot, string shots, string passC, string passA, string keypass, string interceptions, string tac, string tacA, string blk, string rc, string yc, string type, string seasonId)
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
                .AddField("Int-Blk", cStat.Wall, true)

                .AddField("\u200B", "```Discipline```", false)
                .AddField("YC-RC", cStat.Discipline, true);
                 //.AddField("Discipline (YC-RC)",discipline, true);
             embed = builder.Build();
            //}

            return embed;
        }

        //private static string SavePercent(string statOne, string statTwo)
        //{

        //}


        public static Embed NotFound(string playerName, string playerSystem, string playerUrl)
        {
            var systemIcon = "";
            if (playerSystem == "psn") systemIcon = "73";
            else if (playerSystem == "xbox") systemIcon = "53";
            EmbedBuilder builder;
            builder = new EmbedBuilder()
                .WithTitle(playerName)
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithAuthor(author =>
                {
                    author
                        .WithName("There doesn't seem to be any statistics for this season.")
                        .WithIconUrl(playerSystem);
                })
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                });
            return builder.Build();
        }
        public static Embed NotFound(string playerName, string playerUrl)
        {
            EmbedBuilder builder;
            builder = new EmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                        .WithName($"No career statistics found for ***{playerName}***");
                })
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                });
            return builder.Build();
        }

        public static string Splitter(string lgfa)
        {
            var seasonNumber = "";
            var removedLGFA = "";
            var removedPSNLGFA = "";
            var removedSeason = "";

            removedLGFA = lgfa.Replace("LGFA - ", "");
            if (lgfa.Contains("LGFA - Season"))
            {
                return lgfa.Replace("LGFA - Season", "");
            }
            else if (lgfa.Contains("LGFA PSN - Season"))
            {
                return lgfa.Replace("LGFA PSN - Season", "");
            }
            return lgfa;
        }

    }
}

