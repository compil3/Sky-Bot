using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Discord;
using HtmlAgilityPack;
using LGFA.Extensions;
using LGFA.Properties;
using Serilog;

namespace LGFA.Engines.Current
{
    internal abstract class TeamStanding
    {
        public static void //(List<string>, List<string>, string season, string leagueUrl, string system) 
            GetStandings(string system, ref EmbedBuilder embed, [CallerMemberName] string memberName = "")
        {

            var web = new HtmlWeb();
            var standingsUrlTemp =
                "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=standing&leagueid=";
            var leagueInfo = LeagueInfo.GetSeason(system);
            var currentSeason = "";
            var leagueid = "";
            var tempUrl = new string[4];
            var standingsUrl = "";
            var systemIcon = "";

            HtmlDocument teamDoc;
            List<TeamProperties> standings;
            var teamStandings = new List<string>();
            var teamPoints = new List<string>();
            var divisionEast = new List<string>();
            var divisionEastPoints = new List<string>();
            var divisionWest = new List<string>();
            var divisionWestPoints = new List<string>();
            var eastDivCheck = "";
            var westDivCheck = "";
            var _ranking = "";
            var _points = "";

            if (system == "xbox" || system == "Xbox")
            {
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";

                foreach (var info in leagueInfo)
                {
                    tempUrl[0] = standingsUrlTemp;
                    tempUrl[1] = leagueid;
                    tempUrl[2] = "53&seasonid=";
                    tempUrl[3] = info.Season;
                    currentSeason = info.Season;
                    standingsUrl = string.Join("", tempUrl);
                }

                teamDoc = web.Load(standingsUrl);
                try
                {
                    //check for divisions.
                    //if division exist run code block between Division region.
                    eastDivCheck = teamDoc.DocumentNode
                        .SelectSingleNode(
                            "//*[@id='content']/div/div/div[3]/div/div/div/div[2]/table/thead[1]/tr[1]/th[1]")
                        .InnerText;
                    westDivCheck = teamDoc.DocumentNode
                        .SelectSingleNode(
                            "//*[@id='content']/div/div/div[3]/div/div/div/div[2]/table/thead[2]/tr[1]/th[1]")
                        .InnerText;
                    if (eastDivCheck.Contains("Conference") || westDivCheck.Contains("Conference"))
                    {
                        var teamCount = teamDoc.DocumentNode
                            .SelectNodes("//*[@id='content']/div/div/div[3]/div/div/div/div[2]/table")
                            .Descendants("tbody").Count();

                        standings = teamDoc.DocumentNode
                            .SelectNodes("//*[@id='content']/div/div/div[3]/div/div/div/div[2]/table/tbody/tr")
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .Where(tr => tr[0] != null)
                            .Select(tr => new TeamProperties
                            {
                                TeamName = tr[0],
                                GamesPlayed = tr[1],
                                GamesWon = tr[2],
                                GamesDrawn = tr[3],
                                GamesLost = tr[4],
                                Points = tr[5],
                                Streak = tr[6],
                                GoalsFor = tr[7],
                                GoalsAgainst = tr[8],
                                CleanSheets = tr[9],
                                LastTenGames = tr[10],
                                HomeRecord = tr[11],
                                AwayRecord = tr[12],
                                OneGoalGames = tr[13]
                            }).ToList();

                        var splits = standings
                            .Select((x, i) => new {Index = i, Value = x})
                            .GroupBy(x => x.Index / 4)
                            .Select(x => x.Select(v => v.Value).ToList())
                            .ToList();
                        foreach (var eastDiv in splits[0])
                        {
                            divisionEast.Add(eastDiv.Rank + ". " + eastDiv.TeamName);
                            divisionEastPoints.Add(eastDiv.Points);
                        }

                        foreach (var westDiv in splits[1])
                        {
                            divisionWest.Add(westDiv.Rank + ". " + westDiv.TeamName);
                            divisionWestPoints.Add(westDiv.Points);
                        }

                        embed.Title = $"Current Standings - S{currentSeason}";
                        embed.WithUrl(standingsUrl);
                        embed.Author = new EmbedAuthorBuilder()
                        {
                            Name = $"[Table provided by Leaguegaming.com\nLeague: {system.ToUpper()}",
                            IconUrl = systemIcon
                        };
                        embed.Footer = new EmbedFooterBuilder()
                        {
                            Text = "leaguegaming.com",
                            IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                        };
                        var _eastRanking = string.Join(Environment.NewLine, divisionEast);
                        var _eastPoints = string.Join(Environment.NewLine, divisionEastPoints);
                        var _westRanking = string.Join(Environment.NewLine, divisionWest);
                        var _westPoints = string.Join(Environment.NewLine, divisionWestPoints);
                        embed.AddField("\u200B", eastDivCheck);
                        embed.AddField("Ranking", _eastRanking, true);
                        embed.AddField("Pts", _eastPoints, true);
                        embed.AddField("\u200B", westDivCheck);
                        embed.AddField("Ranking", _westRanking, true);
                        embed.AddField("Pts", _westPoints, true);
                    }
                    else
                    {
                        standings = teamDoc.DocumentNode
                            .SelectSingleNode("//*[@id='content']/div/div/div[3]/div/div/div/div[2]")
                            .Descendants("tr")
                            .Skip(2)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .SkipWhile(tr => tr[0].Contains("Conference"))
                            .Where(tr => tr[0] != null)
                            .Select(tr => new TeamProperties
                            {
                                TeamName = tr[0],
                                GamesPlayed = tr[1],
                                GamesWon = tr[2],
                                GamesDrawn = tr[3],
                                GamesLost = tr[4],
                                Points = tr[5],
                                Streak = tr[6],
                                GoalsFor = tr[7],
                                GoalsAgainst = tr[8],
                                CleanSheets = tr[9],
                                LastTenGames = tr[10],
                                HomeRecord = tr[11],
                                AwayRecord = tr[12],
                                OneGoalGames = tr[13]
                            }).ToList();



                        foreach (var team in standings)
                        {
                            teamStandings.Add(team.Rank + ". " + team.TeamName);
                            teamPoints.Add(team.Points);
                        }

                        embed.Title = $"Current Standings - S{currentSeason}";
                        embed.WithUrl(standingsUrl);
                        embed.Author = new EmbedAuthorBuilder()
                        {
                            Name = $"[Table provided by Leaguegaming.com\nLeague: {system.ToUpper()}",
                            IconUrl = systemIcon
                        };
                        embed.Footer = new EmbedFooterBuilder()
                        {
                            Text = "leaguegaming.com",
                            IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                        };
                        _ranking = string.Join(Environment.NewLine, teamStandings);
                        _points = string.Join(Environment.NewLine, teamPoints);
                        embed.AddField("Ranking", _ranking, true);
                        embed.AddField("Pts", _points, true);
                    }
                }

                catch (Exception e)
                {
                    Log.Logger.Error($"Exception thrown: {e}");
                    embed.Title = $"Current Season - S{currentSeason} Unavailable.";
                    embed.WithUrl(standingsUrl);
                    embed.Author = new EmbedAuthorBuilder()
                    {
                        Name = $"[Table provided by Leaguegaming.com]\nLeague: {system.ToUpper()}",
                        IconUrl = systemIcon
                    };
                }
            }

            if (system == "psn" || system == "PSN")
            {
                systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                foreach (var info in leagueInfo)
                {
                    tempUrl[0] = standingsUrlTemp;
                    tempUrl[1] = leagueid;
                    tempUrl[2] = "73&seasonid=";
                    tempUrl[3] = info.Season;
                    currentSeason = info.Season;
                    standingsUrl = string.Join("", tempUrl);
                }

                teamDoc = web.Load(standingsUrl);
                try
                {
                    standings = teamDoc.DocumentNode
                    .SelectSingleNode("//*[@id='content']/div/div/div[3]/div/div/div/div/table")
                    .Descendants("tr")
                    .Skip(2)
                    .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                    .Where(tr => tr[0] != null)
                    .Select(tr => new TeamProperties
                    {
                        TeamName = tr[0],
                        GamesPlayed = tr[1],
                        GamesWon = tr[2],
                        GamesDrawn = tr[3],
                        GamesLost = tr[4],
                        Points = tr[5],
                        Streak = tr[6],
                        GoalsFor = tr[7],
                        GoalsAgainst = tr[8],
                        CleanSheets = tr[9],
                        LastTenGames = tr[10],
                        HomeRecord = tr[11],
                        AwayRecord = tr[12],
                        OneGoalGames = tr[13]
                    }).ToList();


                    foreach (var team in standings)
                    {
                        teamStandings.Add(team.Rank + ". " + team.TeamName);
                        teamPoints.Add(team.Points);
                    }
                    embed.Title = $"Current Standings - S{currentSeason}";
                    embed.WithUrl(standingsUrl);
                    embed.Author = new EmbedAuthorBuilder()
                    {
                        Name = $"[Table provided by Leaguegaming.com\nLeague: {system.ToUpper()}",
                        IconUrl = systemIcon
                    };
                    embed.Footer = new EmbedFooterBuilder()
                    {
                        Text = "leaguegaming.com",
                        IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                    };
                    _ranking = string.Join(Environment.NewLine, teamStandings);
                    _points = string.Join(Environment.NewLine, teamPoints);
                    embed.AddField("Ranking", _ranking, true);
                    embed.AddField("Pts", _points, true);
                }
                catch (Exception e)
                {
                    Log.Logger.Error($"Exception thrown: {e}");
                    embed.Title = $"Current Season - S{currentSeason} Unavailable.";
                    embed.WithUrl(standingsUrl);
                    embed.Author = new EmbedAuthorBuilder()
                    {
                        Name = $"[Table provided by Leaguegaming.com]\nLeague: {system.ToUpper()}",
                        IconUrl = systemIcon
                    };
                }

            }
        }

    }
}
