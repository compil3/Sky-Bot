using System;
using System.Diagnostics;
using System.Net;
using Discord;
using HtmlAgilityPack;
using LiteDB;
using Serilog;
using Sky_Bot.Modules;
using Sky_Bot.Modules.Helpers;
using Sky_Bot.Properties;

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
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            try
            {
                using (var playerDatabase = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared"))
                {
                    var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");

                    //var result = player.Find(x =>
                    //    x.playerName.StartsWith(lookUpName) || x.playerName.ToLower().StartsWith(lookUpName));
                    player.EnsureIndex(x => x.playerName);
                    var result = player.Query()
                        .Where(x => x.playerName.Contains(lookUpName))
                        .ToList();

                    foreach (var found in result)
                    {
                        var playerHtml = found.playerUrl;
                        var playerSystem = found.System;
                        var system = 0;
                        var type = 0;
                        var seasonNumber = "";
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
                                "https://www.leaguegaming.com/images/league/icon/l73.png";
                        }
                        else if (found.System == "xbox")
                        {
                            systemIcon =
                                "https://www.leaguegaming.com/images/league/icon/l53.png";
                        }


                        var playerDoc = web.Load(playerHtml);
                        var currentTableHeadinng =
                             playerDoc.DocumentNode.SelectNodes($"//*[@id='lg_team_user_leagues-{system}']/h3[1]");
                        foreach (var heading in currentTableHeadinng)
                        {
                            var lgfa = WebUtility.HtmlDecode(heading
                                .SelectSingleNode($"//*[@id='lg_team_user_leagues-{system}']/h3[1]").InnerText);
                            if (lgfa.Contains("Last") || lgfa.Contains("Season Stats") || lgfa.Contains("Career")) return EmbedHelpers.NotFound(lookUpName, systemIcon, playerHtml);
                            if (lgfa.Contains("LGFA - Season") || lgfa.Contains("LGFA PSN - Season"))
                            {
                                seasonNumber = EmbedHelpers.Splitter(lgfa);
                                seasonId = seasonNumber.Replace(" ", "");
                            }
                        }
                        try
                        {

                            HtmlNodeCollection findStatRow = null;
                            //find all the season types in the table.
                            if (seasonType == null)
                            {
                                if (playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]") != null &&
                                    playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]") == null &&
                                    playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]") == null)
                                {
                                    findStatRow = playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]");
                                    type = 1;
                                    seasonId = $"{seasonId} Pre-Season";
                                }
                                else if (playerDoc.DocumentNode.SelectNodes(
                                             $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]") !=
                                         null &&
                                         playerDoc.DocumentNode.SelectNodes(
                                             $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]") !=
                                         null &&
                                         playerDoc.DocumentNode.SelectNodes(
                                             $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]") ==
                                         null)
                                {
                                    findStatRow = playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]");
                                    type = 2;
                                    seasonId = $"{seasonId} Regular Season";
                                }
                                else if (playerDoc.DocumentNode.SelectNodes(
                                             $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]") !=
                                         null &&
                                         playerDoc.DocumentNode.SelectNodes(
                                             $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]") !=
                                         null &&
                                         playerDoc.DocumentNode.SelectNodes(
                                             $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]") !=
                                         null)
                                {
                                    findStatRow = playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]");
                                    type = 3;
                                    seasonId = $"{seasonId} Qualifier";
                                }
                            }
                            else
                            {
                                findStatRow = playerDoc.DocumentNode.SelectNodes(
                                    $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]");
                            }
                            if (findStatRow == null)
                            {
                                return message = EmbedHelpers.NotFound(found.playerName, systemIcon, found.playerUrl);
                            }

                            foreach (var playerStat in findStatRow)
                            {
                                #region Nodes

                                var position = "";
                                var lastFiveGames = playerDoc.DocumentNode.SelectNodes(
                                    $"//*[@id='lg_team_user_leagues-{system}']/div[2]/table/tbody/tr");
                                foreach (var recent in lastFiveGames)
                                {
                                    position = WebUtility.HtmlDecode(recent
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{system}']/div[2]/table/tbody/tr[1]/td[2]")
                                        .InnerText);
                                    break;
                                }

                                var playerName = found.playerName;
                                var tempTeamIcon = WebUtility.HtmlDecode(playerStat
                                    .SelectSingleNode(
                                        $"//*[@id='content']/div/div/div[3]/div[1]/div/table/thead/tr/th/div/a/img")
                                    .Attributes["src"].Value); //https://www.leaguegaming.com/images/team/p100/team1236.png
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

                                var teamIcon = string.Join(String.Empty, "http://www.leaguegaming.com" + tempTeamIcon);

                                #endregion

                                stopWatch.Stop();
                                Log.Logger.Warning($"Time taken: {stopWatch.Elapsed}");
                                return SeasonHelper.SeasonEmbed(found.playerName, playerSystem, systemIcon, record, amr,
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
