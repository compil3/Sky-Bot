using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using LGFA.Engines;
using LGFA.Properties;

namespace LGFA.Modules.Helpers
{
    public class TeamHelper
    {
        internal static Embed StandingEmbed(string league)
        {
            var (teamStandings, teamPoints,currentSeason, leagueUrl,system) = TeamStanding.GetStandings(league);
            var ranking = string.Join(Environment.NewLine, teamStandings);
            var points = string.Join(Environment.NewLine, teamPoints);
            var systemIcon = "";
            if (teamStandings == null)
            {
               return NoTable(league);
            }
            else
            {
                if (system == "PSN" || system == "psn")
                {
                    systemIcon =
                        "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                }
                else if (system == "XBOX" || system == "xbox")
                {
                    systemIcon =
                        "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                }

                var builder = new EmbedBuilder()
                    .WithAuthor(author =>
                    {
                        author
                            .WithName($"LGFA {system}")
                            .WithIconUrl(systemIcon);
                    })
                    .WithTitle($"Current Standings")

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
                return builder.Build();
            }
        }

        internal static Embed NoTable(string league)
        {
            EmbedBuilder builder = null;
            builder = new EmbedBuilder()
                .WithTitle("Couldn't find Standings Table for the current season.")
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
