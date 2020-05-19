using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using LGFA.Essentials;
using LGFA.Essentials.Writer;
using Serilog;

namespace LGFA.Engines
{
    class Goalie
    {
        public static bool GetGoalieLG(string league, string trigger, int histSeasonID, string seasonTypeID,
            string command)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(0);
            var currentMethod = sf.GetMethod();

            var thisFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();

            var web = new HtmlWeb();
            var playerStatsDoc = web.Load(RetrieveUrl.GetUrl(league, trigger, histSeasonID, seasonTypeID));

            var countPlayers = playerStatsDoc.DocumentNode.SelectNodes(("//*[@id='lgtable_goaliestats51']/tbody/tr"));
            if (countPlayers == null) return false;
            var playerTotal = countPlayers.Count;

            try
            {
                for (int i = 1; i < playerTotal; i++)
                {
                    var findPlayerNode =
                        playerStatsDoc.DocumentNode.SelectNodes($"//*[@id='lgtable_goaliestats51']/tbody/tr[1]");
                    if (findPlayerNode == null) break;

                    foreach (var player in findPlayerNode)
                    {
                        #region parse variables

                        var position = "";
                        var lgRank = "";
                        var teamIconShort = "";
                        var playerName = "";
                        var gamesPlayed = "";
                        var record = "";
                        var goalsAgainst = "";
                        var shotsAgainst = "";
                        var saves = "";
                        var savePercentage = "";
                        var goalsAgainstAvg = "";
                        var passingPercentage = "";
                        var cleanSheets = "";
                        var manOfTheMatch = "";
                        var avgMatchRating = "";
                        var playerShortURL = "";

                        #endregion

                        position = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/span").InnerText);
                        teamIconShort = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[2]/img")
                            .Attributes["src"].Value);
                        playerName = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[2]/a").InnerText);
                        gamesPlayed = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[4]").InnerText);
                        record = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[5]").InnerText);
                        goalsAgainst = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[6]").InnerText);
                        shotsAgainst = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[7]").InnerText);
                        saves = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[8]").InnerText);
                        savePercentage = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[9]").InnerText);
                        goalsAgainstAvg = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[10]").InnerText);
                        cleanSheets = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[11]").InnerText);
                        manOfTheMatch = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[12]").InnerText);
                        avgMatchRating = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[13]").InnerText);
                        playerShortURL = WebUtility.HtmlDecode(player
                            .SelectSingleNode($"//*[@id='lgtable_goaliestats51']/tbody/tr[{i}]/td[2]/a")
                            .Attributes["href"].Value);

                        var playerURL = string.Join(string.Empty,
                            "https://www.leaguegaming.com/forums/" + playerShortURL);
                        var temp = HttpUtility.ParseQueryString(new Uri(playerURL).Query);
                        var playerID = int.Parse(temp.Get("userid"));
                        var iconEnlarge = teamIconShort.Replace("p16", "p100");
                        var iconURL = string.Join(string.Empty, "https://www.leaguegaming.com" + iconEnlarge);

                        if (seasonTypeID == "pre") seasonTypeID = "pre-season";
                        else if (seasonTypeID == "reg") seasonTypeID = "regular";
                        if (savePercentage == String.Empty) savePercentage = "0";
                        SavePlayerInfo.SavePlayerUrl(playerID, playerName, playerURL);

                        //CareerBuilder.GetCareer(playerID, playerName, league);
                        LGWriter.SaveGoalie(playerID, league, playerName,
                            gamesPlayed, record, goalsAgainst, shotsAgainst, saves, savePercentage, goalsAgainstAvg,
                            cleanSheets, manOfTheMatch, avgMatchRating, playerURL, iconURL, command, histSeasonID,
                            seasonTypeID);
                        GC.Collect();
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Log.Fatal(e,$"Error processing Goalie stats {currentMethod}");
                return false;
            }
        }
    }
}
