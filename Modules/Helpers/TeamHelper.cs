using System;
using Discord;
using LGFA.Engines;
using LGFA.Extensions;

namespace LGFA.Modules.Helpers
{
    public class TeamHelper
    {
        internal static Embed StandingEmbed(string league)
        {
            var (teamStandings, teamPoints, currentSeason, leagueUrl, system) = TeamStanding.GetStandings(league);
            var ranking = string.Join(Environment.NewLine, teamStandings);
            var points = string.Join(Environment.NewLine, teamPoints);
            var systemIcon = "";
            EmbedBuilder builder = null;
            if (teamStandings == null)
            {
                return NoTable(league);
            }

            if (system == "PSN" || system == "psn")
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
            else if (system == "XBOX" || system == "xbox")
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
            var lgInfo = LeagueInfo.GetSeason(league);
            foreach (var info in lgInfo)
            {
                builder = new EmbedBuilder()
                    .WithAuthor(author =>
                    {
                        author
                            .WithName($"LGFA {system}")
                            .WithIconUrl(systemIcon);
                    })
                    .WithTitle($"Current Standings - S{info.Season}")
                    .WithUrl(leagueUrl)
                    .WithColor(new Color(0x26A20B))
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("leaguegaming.com")
                            .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                    })
                    .AddField("Ranking", ranking, true)
                    .AddField("Points", points, true);
            }
            return builder.Build();
        }

        internal static Embed NoTable(string league)
        {
            EmbedBuilder builder = null;
            builder = new EmbedBuilder()
                .WithTitle("Season has not started yet or could not be found.")
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                });
            return builder.Build();
        }
    }
}