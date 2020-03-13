using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using Serilog;
using Sky_Bot.Essentials;
using Sky_Bot.Essentials.Writer;

namespace Sky_Bot.Engines
{
    class Team
    {
        public static bool GetTeam(string system, string trigger, int seasonID, string seasonTypeID, string command)
        {
            var web = new HtmlWeb();
            var doc = web.Load(RetrieveUrl.GetUrl(system, trigger, seasonID, seasonTypeID));

            var findRankNodes = doc.DocumentNode.SelectNodes("//*[@style = 'overflow:hidden;padding:0px;']").ToList();

            #region team variables

            var rank = "";
            var teamName = "";
            var teamIndex = 1;
            var gamesPlayed = "";
            var gamesWon = "";
            var gamesDrawn = "";
            var gamesLost = "";
            var points = "";
            var streak = "";
            var goalsFor = "";
            var goalsAgainst = "";
            var cleanSheets = "";
            var lastTenGames = "";
            var homeRecord = "";
            var awayRecord = "";
            var oneGoalGames = "";
            var teamIconUrl = "";
            var teamUrl = "";
            var teamID = "";
            
            #endregion

            try
            {
                foreach (var team in findRankNodes)
                {
                    #region Split team rank & name into 2 variables

                    var tempRank = team.InnerText;
                    string[] splitRank = tempRank.Split(' ');
                    if (splitRank.Length > 3)
                    {
                        var split = tempRank.Split(new[] {' '}, 3);
                        rank = split[1];
                        teamName = split[2];
                    }
                    else
                    {
                        rank = splitRank[1];
                        teamName = splitRank[2];
                    }

                    #endregion

                    gamesPlayed = team.NextSibling.NextSibling.InnerText;
                    gamesWon = team.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    gamesDrawn = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    gamesLost = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.InnerText;
                    points = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.InnerText;
                    streak = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    goalsFor = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    goalsAgainst = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.InnerText;
                    cleanSheets = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    lastTenGames = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    homeRecord = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .InnerText;
                    awayRecord = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.InnerText;
                    oneGoalGames = team.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling
                        .NextSibling.NextSibling.NextSibling.NextSibling.InnerText;
                    teamIconUrl = team
                        .SelectSingleNode(
                            $"//*[@id='content']/div/div/div/div/div/div/div[1]/table/tbody[{teamIndex}]/tr/td[1]/div/img")
                        .Attributes["src"].Value;
                    teamUrl = team
                        .SelectSingleNode(
                            $"//*[@id='content']/div/div/div/div/div/div/div[1]/table/tbody[{teamIndex}]/tr/td[1]/div/a")
                        .Attributes["href"].Value;

                    var iconLarge = teamIconUrl.Replace("p38", "p100");
                    teamIconUrl = string.Join(string.Empty, "https://www.leaguegaming.com" + iconLarge);
                    teamUrl = string.Join(string.Empty, "https://www.leaguegaming.com/forums/" + teamUrl);
                    var parameters = HttpUtility.ParseQueryString(new Uri(teamUrl).Query);
                    teamID = parameters.Get("teamid");
                    var tempStrip = rank.Replace(")", "").Trim();
                    rank = tempStrip;

                    if (seasonTypeID == "pre") seasonTypeID = "pre-season";
                    else if (seasonTypeID == "reg") seasonTypeID = "regular";

                    TeamWriter.SaveTeam(int.Parse(teamID), int.Parse(rank), teamName, gamesPlayed, gamesWon, gamesDrawn,
                        gamesLost, points, streak, goalsFor, goalsAgainst, cleanSheets, lastTenGames, homeRecord,
                        awayRecord, oneGoalGames, teamIconUrl, teamUrl, system, seasonID, seasonTypeID, command);
                    teamIndex++;
                    GC.Collect();
                }
                return true;
            }
            catch (Exception ex)
            {
             Log.Fatal($"Error while saving {teamName} ({system}) statistics. {ex}");
             GC.Collect();
             return false;
            }
        }
    }
}
