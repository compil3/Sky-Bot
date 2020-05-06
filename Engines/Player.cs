using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using Discord;
using HtmlAgilityPack;
using LiteDB;
using Serilog;
using ShellProgressBar;
using Sky_Bot.Essentials;
using Sky_Bot.Essentials.Writer;
using Sky_Bot.Modules;
using Sky_Bot.Properties;
using Sky_Bot.Schedule;
using static Sky_Bot.Extras.Tick.TickComplete;

namespace Sky_Bot.Engines
{
    class Player
    {
        //public static bool GetPlayer(string lookUpName, string PcnOrLg, string trigger, int histSeasonID, string seasonTypeID,
        //    string command, ProgressBar pbar)
        public static Embed GetPlayer(string lookUpName, string seasonType, string seasonId)
        {
            Embed message = null;
            var systemIcon = "";

            if (seasonType == null || seasonId == null)
            {
                try
                {
                    using (var playerDatabase = new LiteDatabase(@"LGFA.db"))
                    {
                        var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");
                        var result = player.Find(x =>
                            x.playerName.StartsWith(lookUpName) || x.playerName.ToLower().StartsWith(lookUpName));

                        foreach (var found in result)
                        {
                            var playerHtml = found.playerUrl;
                            var playerSystem = found.System;
                            var system = 0;
                            var type = 0;
                            var web = new HtmlWeb();

                            if (seasonType == "pre") type = 1;
                            else if (seasonType == "reg") type = 2;
                            else if (seasonType == "qual") type = 3;

                            if (playerSystem == "xbox")
                            {
                                system = 53;
                            }
                            else if (playerSystem == "psn") system = 73;

                            if (found.System == "psn")
                            {
                                systemIcon =
                                    "https://media.playstation.com/is/image/SCEA/navigation_home_ps-logo-us?$Icon$";
                            }
                            else if (found.System == "xbox")
                            {
                                systemIcon =
                                    "http://www.logospng.com/images/171/black-xbox-icon-171624.png";
                            }

                            var playerDoc = web.Load(playerHtml);
                            var playerStatsRows =
                                playerDoc.DocumentNode.SelectNodes(
                                    $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr");


                            var countRows = playerStatsRows.Count;

                            try
                            {
                                var findStatRow = playerDoc.DocumentNode.SelectNodes(
                                    $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]");
                                if (findStatRow == null)
                                {
                                    return message = Helpers.NotFound(found.playerName, found.System, found.playerUrl);
                                }

                                ;
                                foreach (var playerStat in findStatRow)
                                {
                                    #region Nodes

                                    HtmlNodeCollection lastFiveGames;
                                    var position = "";
                                    lastFiveGames =
                                        playerDoc.DocumentNode.SelectNodes(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[2]/table/tbody/tr");
                                    foreach (var recent in lastFiveGames)
                                    {
                                        position = WebUtility.HtmlDecode(recent
                                            .SelectSingleNode(
                                                $"//*[@id='lg_team_user_leagues-{system}']/div[2]/table/tbody/tr[1]/td[2]")
                                            .InnerText);
                                    }

                                    var playerName = found.playerName;
                                    var teamIcon = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[1]/img")
                                        .Attributes["src"].Value);
                                    var record = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[2]")
                                        .InnerText);
                                    var amr = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[3]")
                                        .InnerText);
                                    var goals = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[4]")
                                        .InnerText);
                                    var assists = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[5]")
                                        .InnerText);
                                    var sot = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[6]")
                                        .InnerText);
                                    var shots = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[7]")
                                        .InnerText);
                                    var passC = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[8]")
                                        .InnerText);
                                    var passA = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[9]")
                                        .InnerText);
                                    var key = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[10]")
                                        .InnerText);
                                    var interceptions = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[11]")
                                        .InnerText);
                                    var tac = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[12]")
                                        .InnerText);
                                    var tacA = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[13]")
                                        .InnerText);
                                    var blks = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[14]")
                                        .InnerText);
                                    var rc = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[15]")
                                        .InnerText);
                                    var yc = WebUtility.HtmlDecode(playerStat
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[16]")
                                        .InnerText);
                                    GC.Collect();

                                    #endregion


                                    return Helpers.BuildEmbed(found.System, playerSystem, systemIcon, record, amr,
                                        goals, assists,
                                        sot, shots, passC, passA, key, interceptions, tac, tacA,
                                        blks, rc, yc, seasonId, found.playerUrl, teamIcon, position);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Error processing stats. {e}");
                                return null;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else if (seasonType != null)
            {

            }
            
            #region Old functionality
            //if (PcnOrLg == "LG")
            //{
            //    var st = new StackTrace();
            //    var sf = st.GetFrame(0);
            //    var currentMethod = sf.GetMethod();
            //    var thisFile = new System.Diagnostics.StackTrace(true).GetFrame(0)?.GetFileName();

            //    var web = new HtmlWeb();
            //    histSeasonID = 2;
            //    var doc = web.Load(RetrieveUrl.GetUrl(league, trigger, histSeasonID, seasonTypeID));

            //    var countPlayers = doc.DocumentNode.SelectNodes("//*[@id='lgtable_memberstats51']/tbody/tr").Count;
            //    if (countPlayers == null) return false;

            //    //var playerCount = countPlayers;
            //    var childOptions = new ProgressBarOptions()
            //    {
            //        ForegroundColor = ConsoleColor.Green,
            //        BackgroundColor = ConsoleColor.DarkGreen,
            //        ProgressCharacter = '\u2593',
            //        CollapseWhenFinished = false,
            //        DisplayTimeInRealTime = false
            //    };

            //    try
            //    {
            //        using (var child = pbar.Spawn(countPlayers, $"Updating {league} Player Stats", childOptions))
            //        {
            //            //pbar.EstimatedDuration = TimeSpan.FromMilliseconds(playerCount * 500);  //if playerCount = 253 Estimated Duration should = 15,180 mins

            //            for (int i = 1; i < countPlayers; i++)
            //            {
            //                var findPlayerNodes =
            //                    doc.DocumentNode.SelectNodes($"//*[@id='lgtable_memberstats51']/tbody/tr[{i}]");
            //                if (findPlayerNodes == null) break;

            //                #region Nodes - Database storage method

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



            //                    var playerUrl = string.Join(string.Empty,
            //                        "https://www.leaguegaming.com/forums/" + playerShortURL);
            //                    var iconEnlarge = teamIconTemp.Replace("p16", "p100");
            //                    var iconURL = string.Join(string.Empty, "https://leaguegaming.com" + iconEnlarge);
            //                    var temp = HttpUtility.ParseQueryString(new Uri(playerUrl).Query);
            //                    int playerId = int.Parse(temp.Get("userid"));

            //                    if (shotPercentage == string.Empty) shotPercentage = "0";
            //                    else if (tacklePercentage == string.Empty) tacklePercentage = "0";
            //                    else if (passingPercentage == string.Empty) passingPercentage = "0";

            //                    if (seasonTypeID == "pre") seasonTypeID = "pre-season";
            //                    else if (seasonTypeID == "reg") seasonTypeID = "regular";
            //                    #endregion
            //                    #region Saving to DB
            //                    SavePlayerInfo.SavePlayerUrl(playerId, playerName, playerUrl);
            //                    Career.GetCareer(playerId, playerName, league);

            //                    LGWriter.SavePlayer(playerId, histSeasonID, seasonTypeID, position,
            //                        playerName, gamesPlayed, record, avgMatchRating, goals, assists, cleanSheets,
            //                        shotsOnGoal,
            //                        shotsOnTarget, shotPercentage, tackles, tackleAttempts, tacklePercentage,
            //                        passingPercentage,
            //                        keyPasses,
            //                        interceptions, blocks, yellowCards, redCards, manOfTheMatch, playerUrl, league, iconURL,
            //                        command, position, PcnOrLg);
            //                    #endregion

            //                    Thread.Sleep(500);
            //                    var estimatedDuration = TimeSpan.FromMilliseconds(500 * countPlayers) +
            //                                            TimeSpan.FromMilliseconds(300 * i);
            //                    child.Tick(estimatedDuration, $"Updated {playerName}: {i+1} of {countPlayers}");

            //                    //TickToCompletion(child,i,sleep:500, playerName, playerCount);

            //                }
            //                GC.Collect();

            //            }
            //        }
            //    }

            //    catch (Exception e)
            //    {
            //        Log.Fatal(e, $"Error processing LG Player stats {currentMethod}");
            //    }
            //}
            //else if (PcnOrLg == "PCN")
            //{
            //    var st = new StackTrace();
            //    var sf = st.GetFrame(0);
            //    var currentMethod = sf.GetMethod();
            //    var thisFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();

            //    var web = new HtmlWeb();
            //    var doc = web.Load(RetrieveUrl.GetUrl(league, trigger, histSeasonID, seasonTypeID));

            //    var seasonSelected = doc.DocumentNode.SelectNodes("//*[@id='playerStatLeadersPageContent.seasonID']");
            //}
            #endregion
            return message;
        }
    }
}
