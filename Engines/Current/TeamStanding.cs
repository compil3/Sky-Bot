using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using LGFA.Essentials;
using LGFA.Properties;
using Serilog;

namespace LGFA.Engines
{
    internal class TeamStanding
    {
        public static (List<string>, List<string>, string season, string leagueUrl, string system) GetStandings(
            string system)
        {
            var web = new HtmlWeb();
            var standingsUrlTemp =
                "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=standing&leagueid=";
            var currentSeason = GetCurrentSeason.GetSeason(system);
            var leagueid = "";
            var tempUrl = new string[4];
            var standingsUrl = "";
            try
            {
                HtmlDocument teamDoc;
                if (system == "xbox" || system == "Xbox")
                {
                    tempUrl[0] = standingsUrlTemp;
                    tempUrl[1] = leagueid;
                    tempUrl[2] = "53&seasonid=";
                    tempUrl[3] = currentSeason;
                    standingsUrl = string.Join("", tempUrl);

                    teamDoc = web.Load(standingsUrl);
                    var tempDoc =
                        teamDoc.DocumentNode.SelectSingleNode(
                            "//*[@id='content']/div/div/div[3]/div/div/div/div[2]/table/tbody");

                    var standings = teamDoc.DocumentNode
                        .SelectSingleNode("//*[@id='content']/div/div/div[3]/div/div/div/div[2]/table")
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
                    var teamStandings = new List<string>();
                    var teamPoints = new List<string>();


                    foreach (var team in standings)
                    {
                        teamStandings.Add(team.Rank + ". " + team.TeamName);
                        teamPoints.Add(team.Points);
                    }

                    return (teamStandings, teamPoints, currentSeason, standingsUrl, system.ToUpper());
                }

                if (system == "psn" || system == "PSN")
                {
                    tempUrl[0] = standingsUrlTemp;
                    tempUrl[1] = leagueid;
                    tempUrl[2] = "73&seasonid=";
                    tempUrl[3] = currentSeason;
                    standingsUrl = string.Join("", tempUrl);

                    teamDoc = web.Load(standingsUrl);

                    var standings = teamDoc.DocumentNode
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
                    var teamStandings = new List<string>();
                    var teamPoints = new List<string>();


                    foreach (var team in standings)
                    {
                        teamStandings.Add(team.Rank + ". " + team.TeamName);
                        teamPoints.Add(team.Points);
                    }

                    return (teamStandings, teamPoints, currentSeason, standingsUrl, system.ToUpper());
                }

                return (null, null, null, null, null);
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Exception thrown: {e}");
                return (null, null, null, null, null);
            }
        }
    }
}