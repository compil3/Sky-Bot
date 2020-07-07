using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Embed NoTable(string league)
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

        public static void StandingsBuilder(List<string> standings, List<string> points, string league, ref EmbedBuilder embed)
        {
            var systemIcon = "";
            var standingsUri = "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=standing&";
            var lgInfo = LeagueInfo.GetSeason(league);
            foreach (var lg in lgInfo)
            {
                if (league.Contains("psn"))
                {
                    systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                    standingsUri = standingsUri + "&leagueid=73&seasonid=" + lg.Season;
                }
                else if (league.Contains("xbox"))
                {
                    systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                    standingsUri = standingsUri + "&leagueid=53&seasonid=" + lg.Season;
                }

                embed.Title = $"Current Standings - S{lg.Season}";
                embed.WithUrl(standingsUri);

                embed.Author = new EmbedAuthorBuilder()
                {
                    Name = $"[Table provided by Leaguegaming.com\n League: {league.ToUpper()}",
                    IconUrl = systemIcon
                };
                embed.Footer = new EmbedFooterBuilder()
                {
                    Text = "leaguegaming.com",
                    IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                };

                var _ranking = string.Join(Environment.NewLine, standings);
                var _points = string.Join(Environment.NewLine, points);

                embed.AddField("Ranking", _ranking, true);
                embed.AddField("Pts", _points, true);
                embed.
            }
        }
    }
}