using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Discord;
using Microsoft.Extensions.Primitives;
using Sky_Bot.Properties;

namespace Sky_Bot.Modules
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
                GamesPlayed = record,
                AvgMatchRating = Convert.ToDouble(amr),
                Goals = Convert.ToDouble(goals),
                Assists = assists,
                KeyPasses = key,
                ShotAttempts = shots,
                ShotsOnTarget = sot,
                Blocks = blk,
                TackleAttempts = tacA,
                Tackles = tac,
                PassesAttempted = passA,
                PassesCompleted = passC,
                YellowCards = yc,
                RedCards = rc
            };

            string[] scoring = new string[3];
            scoring[0] = cStat.Goals.ToString(CultureInfo.InvariantCulture);
            scoring[1] = cStat.Assists;
            scoring[2] = cStat.KeyPasses;
            offense = string.Join("-", scoring);

            string[] shooting = new string[3];
            shooting[0] = cStat.ShotAttempts;
            shooting[1] = cStat.ShotsOnTarget;


            //foreach (var stat in stats)
            //{
             builder = new EmbedBuilder()
                 .WithTitle($"{playerName}")
                 .WithUrl(playerUrl)
                 .WithColor(new Color(0x26A20B))
                 .WithCurrentTimestamp()
                 .WithFooter(footer =>
                 {
                     footer
                         .WithText("leaguegaming.com")
                         .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                 })
                 .AddField("GP", cStat.GamesPlayed, true)
                 .AddField("Record (W-D-L)", record, true)
                 .AddField("AMR", cStat.AvgMatchRating, true)
                 .AddField("G-A-Key", offense, true)
                 .AddField("S-SOT-SH%", cStat.ShotPercentage,true);
                 //.AddField("Passing (PC-PA-P%)", passing, true)
                 //.AddField("Defensive (Int-Blk)", defensive, true)
                 //.AddField("Discipline (YC-RC)",discipline, true);
             embed = builder.Build();
            //}

            return embed;
        }

        


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

        internal class Career
        {
            private string gamesPlayed;
            public string GamesPlayed
            {
                get => gamesPlayed;
                set
                {
                    this.gamesPlayed = value;
                    var splitRecord = this.gamesPlayed.Split('-');
                    int wins = int.Parse(splitRecord[0]);
                    int draws = int.Parse(splitRecord[1]);
                    int loses = int.Parse(splitRecord[2]);
                    int gamesPlayed = wins + draws + loses;
                    this.gamesPlayed = gamesPlayed.ToString();
                }
            }

            public string Record { get; set; }

            public decimal AvgMatchRating
            {
                get; set;
            }

            public double Goals { get; set; }
            public string Assists { get; set; }

            public double ShotAttempts { get; set; }
            public string ShotsOnTarget { get; set; }
            public double ShotPercentage { get; set; }

            public double PassesCompleted { get; set; }
            public double PassesAttempted { get; set; }
            public double PassingPercentage { get; set; }
            public string KeyPasses { get; set; }

            public double Tackles { get; set; }
            public double TackleAttempts { get; set; }
            public double TacklePercentage { get; set; }

            public string Interceptions { get; set; }
            public string Blocks { get; set; }
            public string YellowCards { get; set; }
            public string RedCards { get; set; }
        }
    }
}

