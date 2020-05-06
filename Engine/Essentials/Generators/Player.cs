using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using Serilog;
using ShellProgressBar;
using RetrieveUrl = Engine.Essentials.Helpers.RetrieveUrl;
using PlayerWriter = Engine.Essentials.Write.PlayWriter;
namespace Engine.Essentials.Generators
{
    //class Player
    //{
    //    public static bool GetInformation(string system, string trigger, int seasonNumber, string seasonType, string command, ProgressBar pbar)
    //    {
    //        #region method info
    //        var usage = false;
    //        if (!usage)
    //        {
    //            var st = new StackTrace();
    //            var sf = st.GetFrame(0);
    //            var currentMethod = sf.GetMethod();
    //            var thisFile = new System.Diagnostics.StackTrace(true).GetFrame(0)?.GetFileName();
    //        }
    //        #endregion

    //        var web = new HtmlWeb();
    //        var doc = web.Load(RetrieveUrl.GetUrl(system, trigger, seasonNumber, seasonType));

    //        var draftPlayers = doc.DocumentNode.SelectNodes("//*[@id='lgtable_memberstats51']/tbody/tr");
    //        if (draftPlayers == null) return false;

    //        var totalPlayers = draftPlayers.Count;

    //        var childOptions = new ProgressBarOptions()
    //        {
    //            ForegroundColor = ConsoleColor.Green,
    //            BackgroundColor = ConsoleColor.DarkGreen,
    //            ProgressCharacter = '\u2593',
    //            CollapseWhenFinished = false,
    //            DisplayTimeInRealTime = false
    //        };


    //       try
    //        {
    //            using var child = pbar.Spawn(totalPlayers, $"Updating {system} Player Stats", childOptions);
    //            for (var i = 1; i < totalPlayers; i++)
    //            {
    //                var findPlayerNodes =
    //                    doc.DocumentNode.SelectNodes($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]");
    //                if (findPlayerNodes == null) break;

    //                #region Nodes

    //                foreach (var player in findPlayerNodes)
    //                {
    //                    var teamIconTemp = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/img")
    //                        .Attributes["src"].Value);
    //                    var position = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/span")
    //                        .InnerText);
    //                    var playerName = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/a").InnerText);
    //                    var gamesPlayed = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[4]").InnerText);
    //                    var record = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[5]").InnerText);
    //                    var avgMatchRating = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[6]").InnerText);
    //                    var goals = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[7]").InnerText);
    //                    var assists = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[8]").InnerText);
    //                    var cleanSheets = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[9]").InnerText);
    //                    var shotsOnGoal = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[10]").InnerText);
    //                    var shotsOnTarget = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[11]").InnerText);
    //                    var shotPercentage = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[12]").InnerText);
    //                    var tackles = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[13]").InnerText);
    //                    var tackleAttempts = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[14]").InnerText);
    //                    var tacklePercentage = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[15]").InnerText);
    //                    var passingPercentage = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[16]").InnerText);
    //                    var keyPasses = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[17]").InnerText);
    //                    var interceptions = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[18]").InnerText);
    //                    var blocks = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[19]").InnerText);
    //                    var yellowCards = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[20]").InnerText);
    //                    var redCards = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[21]").InnerText);
    //                    var manOfTheMatch = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[22]").InnerText);
    //                    var playerShortURL = WebUtility.HtmlDecode(player
    //                        .SelectSingleNode($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]/td[2]/a")
    //                        .Attributes["href"].Value);
    //                    #endregion

    //                    #region Saves to Database
    //                    var playerUrl = string.Join(string.Empty,
    //                        "https://www.leaguegaming.com/forums/" + playerShortURL);
    //                    var iconEnlarge = teamIconTemp.Replace("p16", "p100");
    //                    var iconURL = string.Join(string.Empty, "https://leaguegaming.com" + iconEnlarge);
    //                    var temp = HttpUtility.ParseQueryString(new Uri(playerUrl).Query);
    //                    int playerId = int.Parse(temp.Get("userid"));

    //                    if (shotPercentage == string.Empty) shotPercentage = "0";
    //                    else if (tacklePercentage == string.Empty) tacklePercentage = "0";
    //                    else if (passingPercentage == string.Empty) passingPercentage = "0";

    //                    if (seasonType == "pre") seasonType = "pre-season";
    //                    else if (seasonType == "reg") seasonType = "regular";

    //                    PlayerWriter.SaveUrl(playerId, playerName, playerUrl);
    //                    Essentials.Generators.Career.GetStatistics(playerId, playerName, system);

    //                    SavePlayer(playerId, seasonNumber, seasonType, position,
    //                        playerName, gamesPlayed, record, avgMatchRating, goals, assists, cleanSheets,
    //                        shotsOnGoal,
    //                        shotsOnTarget, shotPercentage, tackles, tackleAttempts, tacklePercentage,
    //                        passingPercentage,
    //                        keyPasses,
    //                        interceptions, blocks, yellowCards, redCards, manOfTheMatch, playerUrl, system, iconURL,
    //                        command, position);
    //                    #endregion

    //                    Thread.Sleep(500);
    //                    var estimatedDuration = TimeSpan.FromMilliseconds(500 * totalPlayers) +
    //                                            TimeSpan.FromMilliseconds(300 * i);
    //                    child.Tick(estimatedDuration, $"Updated {playerName}: {i}/{totalPlayers}");
    //                }
    //                GC.Collect();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Log.Logger.Fatal(e, $"Error processing Player stats.");
    //            throw;
    //        }
    //        return false;
    //    }
    //}
}
