using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

                          
                            List<SeasonProperties> table = playerDoc.DocumentNode
                                .SelectSingleNode($"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody")
                                .Descendants("tr")
                                .Skip(0)
                                // Up to here is your code. Here you select all rows from the table.
                                // Each row is presented as List<string>.
                                .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                                // Here we filter table rows by "seasonId" and "Reg".
                                .Where(tr => tr[0] == "Reg") 
                                // Here we create objects CareerProperties from filtered rows.
                                .Select(tr => new SeasonProperties
                                {
                                    Record = tr[1],
                                    AvgMatchRating = tr[2],
                                    Goals = Convert.ToDouble(tr[3]),
                                    Assists = tr[4],
                                    ShotsOnTarget = tr[5],
                                    ShotAttempts = tr[6],
                                    PassesCompleted = tr[7],
                                    PassesAttempted = tr[8],
                                    KeyPasses = tr[9],
                                    Interceptions = tr[10],
                                    Tackles = tr[11],
                                    TackleAttempts = tr[12],
                                    PossW = tr[13],
                                    PossL = tr[14],
                                    Blocks = tr[15],
                                    RedCards = tr[16],
                                    YellowCards = tr[17]

                                })
                                .ToList();


                            var tempTeamIcon = "";
                            var position = "";
                            foreach (var teamFive in findStatRow)
                            {
                                tempTeamIcon = teamFive
                                    .SelectSingleNode(
                                        $"//*[@id='content']/div/div/div[3]/div[1]/div/table/thead/tr/th/div/a/img")
                                    .Attributes["src"].Value;
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
                            }

                            foreach (var pStat in table)
                            {

                                var playerName = found.playerName;

                               
                                var teamIcon = string.Join(String.Empty, "http://www.leaguegaming.com" + tempTeamIcon);
                                
                                
                                #region Nodes

                                //var tempTeamIcon = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='content']/div/div/div[3]/div[1]/div/table/thead/tr/th/div/a/img")
                                //    .Attributes["src"].Value); //https://www.leaguegaming.com/images/team/p100/team1236.png
                                //var record = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[2]")
                                //    .InnerText);
                                //var amr = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[3]")
                                //    .InnerText);
                                //var goals = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[4]")
                                //    .InnerText);
                                //var assists = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[5]")
                                //    .InnerText);
                                //var sot = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[6]")
                                //    .InnerText);
                                //var shots = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[7]")
                                //    .InnerText);
                                //var passC = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[8]")
                                //    .InnerText);
                                //var passA = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[9]")
                                //    .InnerText);
                                //var key = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[10]")
                                //    .InnerText);
                                //var interceptions = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[11]")
                                //    .InnerText);
                                //var tac = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[12]")
                                //    .InnerText);
                                //var tacA = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[13]")
                                //    .InnerText);
                                //var blks = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[14]")
                                //    .InnerText);
                                //var rc = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[15]")
                                //    .InnerText);
                                //var yc = WebUtility.HtmlDecode(playerStat
                                //    .SelectSingleNode(
                                //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]/td[16]")
                                //    .InnerText);
                                //GC.Collect();

                                //var teamIcon = string.Join(String.Empty, "http://www.leaguegaming.com" + tempTeamIcon);

                                #endregion

                                stopWatch.Stop();
                                Log.Logger.Warning($"Time taken: {stopWatch.Elapsed}");
                                return SeasonHelper.SeasonEmbed(found.playerName, playerSystem, systemIcon, pStat.Record, pStat.AvgMatchRating,
                                    pStat.Goals.ToString(), pStat.Assists, pStat.ShotsOnTarget, pStat.ShotAttempts, pStat.PassesCompleted, pStat.PassesAttempted, pStat.KeyPasses,
                                    pStat.Interceptions, pStat.Tackles, pStat.TackleAttempts, pStat.PossW, pStat.PossL, pStat.Blocks, pStat.RedCards, pStat.YellowCards, seasonId, found.playerUrl,
                                    teamIcon, position);
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Logger.Error($"Error processing stats. {e}");
                            return null;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error: {e}");
                throw;
            }
            return null;
        }
    }
}
