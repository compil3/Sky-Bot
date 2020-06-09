using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using HtmlAgilityPack;
using LGFA.Extensions;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace LGFA.Engines.Current.Goalie
{
    internal class CurrentSeason
    {
        private static string playerFound;

        public static (List<GoalieProperties>, string playerFound) SeasonStats(string playerLookup)
        {
            var webPage = new HtmlWeb();

            var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(dbPath, dbFolder);

            using var goalieDb = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var goalie = goalieDb.GetCollection<PlayerProperties.PlayerInfo>("Players");

            goalie.EnsureIndex(x => x.playerName);

            var result = goalie.Query()
                .Where(x => x.playerName.Contains(playerLookup))
                .ToList();
            var systemIcon = "";
            var statsPage = "";
            List<GoalieProperties> gStat = null;
            foreach (var found in result)
            {
                playerFound = found.playerName;
                var currentSeason = LeagueInfo.GetSeason(found.System);
                foreach (var sInfo in currentSeason)
                {
                    if (found.System.Contains("xbox"))
                    {
                        statsPage =
                            $"https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_memberstats&leagueid=53&seasonid={sInfo.Season}";
                        systemIcon =
                            "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                    }
                    else if (found.System.Contains("psn"))
                    {
                        statsPage =
                            $"https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_memberstats&leagueid=73&seasonid={sInfo.Season}";
                        systemIcon =
                            "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                    }
                    var goalieTable = webPage.Load(statsPage);
                    try
                    {
                        gStat = goalieTable.DocumentNode
                            .SelectSingleNode("//*[@id='lgtable_goaliestats51']/tbody")
                            .Descendants("tr")
                            .Skip(0)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .Where(tr => tr[1] == found.playerName)
                            .Select(tr => new GoalieProperties
                            {
                                teamIcon = GetTeamIcon(found.playerUrl),
                                gamesPlayed = tr[3],
                                record = tr[4],
                                goalsAgainst = tr[5],
                                shotsAgainst = tr[6],
                                saves = tr[7],
                                savePercentage = tr[8],
                                goalsAgainstAvg = tr[9],
                                cleanSheets = tr[10],
                                manOfTheMatch = tr[11],
                                avgMatchRating = tr[12],
                                playerURL = found.playerUrl,
                                playerName = found.playerName,
                                userSystem = systemIcon,
                            }).ToList();
                        return (gStat, found.playerName);
                    }
                    catch (NullReferenceException ex)
                    {
                        Log.Logger.Error($"Error processing goalie: {ex}");
                        return (null, playerFound);
                    }

                }

            }
            return (null, playerFound);
        }

        private static string GetTeamIcon(string playerUrl)
        {
            var playerPage = new HtmlWeb();
            var tempPage = playerPage.Load(playerUrl);

            var iconTemp = tempPage.DocumentNode
                .SelectSingleNode("//*[@id='content']/div/div/div[3]/div[1]/div/table/thead/tr/th/div/a/img")
                .Attributes["src"].Value;
            var icon = "http://www.leaguegaming.com" + iconTemp.Replace("p16", "p100");
            return icon;
        }
    }
}
