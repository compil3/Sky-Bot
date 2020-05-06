using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Discord;

namespace Sky_Bot.Modules
{
    public class Helpers
    {
        public static Embed BuildEmbed(string playerName, string userSystem, string systemIcon, string record, string amr, string goals, string assists, string sot, string shots, string passC, string passA,
            string key, string interceptions, string tac, string tacA, string blks, string rc, string yc, string seasonId, string playerUrl, string teamIcon, string position)
        {
            EmbedBuilder builder = null;

            #region stat compression

            string[] scoring = new string[3];
            scoring[0] = goals;
            scoring[1] = assists;
            scoring[3] = key;
            var scoringRecord = string.Join(" - ", scoring);


            string[] recordStrip = record.Split('-');
            var wins = int.Parse(recordStrip[0]);
            var draws = int.Parse(recordStrip[1]);
            var loses = int.Parse(recordStrip[2]);
            var gamesPlayed = wins + draws + loses;
            
            string[] shooting = new string[3];
            shooting[0] = shots;
            shooting[1] = sot;
            decimal shotPercentage = Convert.ToDecimal(sot) / Convert.ToDecimal(shots);
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

            builder = new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"{userSystem.ToUpper()} Season {seasonId} Stats provided by Sky Sports.")
                    .WithIconUrl(systemIcon))
                .WithTitle($"Statistics for ***{playerName}*** {position}")
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
                })
                .WithThumbnailUrl(teamIcon)
                .AddField("GP", gamesPlayed, true)
                .AddField("Record (W-D-L)", record, true)
                .AddField("AMR", amr, true)
                .AddField("G-A-Key", scoringRecord, true)
                .AddField("SOG-SOT-SH%", shootingRecord, true)
                .AddField("TK-TKA-TK%", tackleRecord, true)
                .AddField("Passing (PC-PA-P%", passingRecord, true)
                .AddField("Defensive (Int-Blks)", defensiveRecord, true)
                .AddField("Discipline (YC-RC)", disciplineRecord, true);
            var embed = builder.Build();
            return embed;
        }

        public static Embed GoalieEmebd()
        {
            EmbedBuilder builder = null;
            builder = new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"{userSystem.ToUpper()} Season {seasonId} Stats provided by Sky Sports.")
                    .WithIconUrl(systemIcon))
                .WithTitle($"Statistics for ***{playerName}*** {position}")
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
                })
                .WithThumbnailUrl(teamIcon)
                .AddField("Record", record, true)
                .AddField("AMR", amr, true)
                .AddField("SH-SAV-S%", shootingRecord, true)
                .AddField("GA-GAA", goalRecord, true)
                .AddField("CS-MOTM", cleanMan, true);

            var embed = builder.Build();
            return embed;

        }
        public static Embed NotFound(string playerName, string playerSystem, string playerUrl)
        {
            var systemIcon = "";
            if (playerSystem == "psn") systemIcon = "73";
            else if (playerSystem == "xbox") systemIcon = "53";
                EmbedBuilder builder;
            builder = new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"No statistics found for {playerName}")
                    .WithIconUrl(playerSystem))
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl($"https://www.leaguegaming.com/images/league/icon/l{systemIcon}.png");
                });
            return builder.Build();
        }

        
    }
}

