﻿using Discord;
using LGFA.Properties;

namespace LGFA.Modules.Helpers
{
    public class CareerEmbeds
    {
        public static Embed CareerSeasonalEmbed(string foundPlayerName, string foundPlayerUrl, string record,
            string amr, string goals, string assists,
            string sot, string shots, string passC, string passA, string keypass, string interceptions, string tac,
            string tacA, string possW, string possL, string blk, string rc, string yc, string type, string seasonId)
        {
            EmbedBuilder builder = null;
            Embed embed = null;
            if (type == "Reg") type = "Regular";
            var seasonSplit = seasonId.Replace("S", "");
            //var stats = Compressor(record, amr, goals, assists, sot, shots, passC, passA, key, intercept, tac, tacA, blk, rc, yc
            var cStat = new CareerProperties
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
                Discipline = "0"
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
                .AddField("​", "```Record```")
                .AddField("GP", cStat.GamesPlayed, true)
                .AddField("Record (W-D-L)", cStat.Record, true)
                .AddField("AMR", cStat.AvgMatchRating, true)
                .AddField("\u200B", "```Offensive Stats```")
                .AddField("Goals", cStat.Goals, true)
                .AddField("G/Game", cStat.GoalsPerGame, true)
                .AddField("Shots - SOT", cStat.ShotSot, true)
                .AddField("S/Game", cStat.ShotPerGame, true)
                .AddField("Shots/Goal - SH%", cStat.ShotPerGoal, true)
                // .AddField("SH%", cStat.ShotPercentage,true)
                .AddField("\u200B", "```Passing Stats```")
                .AddField("Assists", cStat.Assists, true)
                .AddField("Pass - Pass Attempts", cStat.PassRecord, true)
                .AddField("Key Passes", cStat.KeyPasses, true)
                .AddField("Assist/Game", cStat.AssistPerGame, true)
                .AddField("P/Game - Pass %", cStat.PassPerGame, true)

                //.AddField("Pass %", cStat.PassingPercentage, true)
                .AddField("Key Pass/Game", cStat.KeyPassPerGame, true)
                .AddField("\u200B", "```Defensive Stats```")
                .AddField("Tackles - Tackle Attempts", cStat.Tackling, true)
                .AddField("Tackles Per Game", cStat.TacklesPerGame, true)
                .AddField("Tackle Success", cStat.TacklePercent, true)
                .AddField("PossW-PossL", cStat.Poss, true)
                .AddField("Int-Blk", cStat.Wall, true)
                .AddField("\u200B", "```Discipline```")
                .AddField("YC-RC", cStat.Discipline, true);
            embed = builder.Build();
            return embed;
        }
    }
}