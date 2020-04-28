using System;
using System.Collections.Generic;
using System.Text;
using Discord;

namespace Sky_Bot.Modules
{
    public class Helpers
    {
        public static Embed BuilderLG(string userSystem, string shotRecord, string goalRecord, string cleanMan, string systemIcon, int seasonId, string playerName, string position, string playerUrl, string teamIcon, string record,
            string avgMatchRating, ulong guild, string division)
        {
            EmbedBuilder builder;
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
                         .AddField("AMR", avgMatchRating, true)
                         .AddField("SH-SAV-S%", shotRecord, true)
                         .AddField("GA-GAA", goalRecord, true)
                         .AddField("CS-MOTM", cleanMan, true);
            return builder.Build();
        }

        public static Embed BuilderPCN(string foundUserSystem, string shotRecord, string goalRecord, string cleanMan, string systemIcon, int foundSeasonId, string foundPlayerName, string foundPosition, string foundPlayerUrl, 
            string foundTeamIcon, string foundRecord, string foundAvgMatchRating, in ulong guild, string division)
        {
            EmbedBuilder builder;

            builder = new EmbedBuilder()
                .WithAuthor(author => author
                    .WithName($"Season {foundSeasonId} Stats provided by Sky Sports."))
                .WithTitle($"Statistics for ***{foundPlayerName}*** {foundPosition}")
                .WithUrl(foundPlayerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
                })
                .WithThumbnailUrl(foundTeamIcon)
                .AddField("Record", foundRecord, true)
                .AddField("AMR", foundAvgMatchRating, true)
                .AddField("SH-SAV-S%", shotRecord, true)
                .AddField("GA-GAA", goalRecord, true)
                .AddField("CS-MOTM", cleanMan, true);;
            return builder.Build();
        }
    }
}

