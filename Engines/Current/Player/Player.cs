﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Discord;
using HtmlAgilityPack;
using LGFA.Modules;
using LGFA.Modules.Helpers;
using LGFA.Properties;
using LiteDB;
using Serilog;
using Missing = LGFA.Modules.Helpers.Missing;

namespace LGFA.Engines
{
    class Player
    {
        //public static bool GetPlayer(string lookUpName, string PcnOrLg, string trigger, int histSeasonID, string seasonTypeID,
        //    string command, ProgressBar pbar)
        public static List<SeasonProperties> GetPlayer(string lookUpName, string seasonType, string seasonId)
        {
            Embed message = null;
            var systemIcon = "";
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            try
            {
                var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var dbFolder = "Database/";
                var dbDir = Path.Combine(dbPath, dbFolder);

                using var playerDatabase = new LiteDatabase($@"Filename={dbDir}LGFA.db;connection=shared");
                var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");

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
                    var web = new HtmlWeb();

                    //if (seasonType == "pre") type = 1;
                    //else if (seasonType == "reg") type = 2;
                    //else if (seasonType == "qual") type = 3;

                    if (playerSystem == "xbox")system = 53;
                    else if (playerSystem == "psn") system = 73;

                    if (found.System == "psn")
                        systemIcon =
                            "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                    else if (found.System == "xbox") systemIcon =
                        "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";


                    var playerDoc = web.Load(playerHtml);
                    var tableHeadinng =
                        playerDoc.DocumentNode.SelectNodes($"//*[@id='lg_team_user_leagues-{system}']/h3[1]");
                    //foreach (var heading in tableHeadinng)
                    //{
                    //    var lgfa = WebUtility.HtmlDecode(heading
                    //        .SelectSingleNode($"//*[@id='lg_team_user_leagues-{system}']/h3[1]").InnerText);
                    //    if (lgfa.Contains("Last") || lgfa.Contains("Season Stats") || lgfa.Contains("CareerBuilder")) return Missing.NotFound(lookUpName, systemIcon, playerHtml);
                    //    if (lgfa.Contains("LGFA - Season") || lgfa.Contains("LGFA PSN - Season"))
                    //    {
                    //        var seasonNumber = EmbedHelpers.Splitter(lgfa);
                    //        seasonId = seasonNumber.Replace(" ", "");
                    //    }
                    //}
                    try
                    {
                        #region remove me
                        //HtmlNodeCollection findStatRow = null;
                        ////find all the season types in the table.
                        //if (seasonType == null)
                        //{
                        //    if (playerDoc.DocumentNode.SelectNodes(
                        //            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]") != null &&
                        //        playerDoc.DocumentNode.SelectNodes(
                        //            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]") == null &&
                        //        playerDoc.DocumentNode.SelectNodes(
                        //            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]") == null)
                        //    {
                        //        findStatRow = playerDoc.DocumentNode.SelectNodes(
                        //            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]");
                        //        type = 1;
                        //        seasonId = $"{seasonId} Pre-Season";
                        //    }
                        //    else if (playerDoc.DocumentNode.SelectNodes(
                        //                 $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]") !=
                        //             null &&
                        //             playerDoc.DocumentNode.SelectNodes(
                        //                 $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]") !=
                        //             null &&
                        //             playerDoc.DocumentNode.SelectNodes(
                        //                 $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]") ==
                        //             null)
                        //    {
                        //        findStatRow = playerDoc.DocumentNode.SelectNodes(
                        //            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]");
                        //        type = 2;
                        //        seasonId = $"{seasonId} Regular Season";
                        //    }
                        //    else if (playerDoc.DocumentNode.SelectNodes(
                        //                 $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[1]") !=
                        //             null &&
                        //             playerDoc.DocumentNode.SelectNodes(
                        //                 $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[2]") !=
                        //             null &&
                        //             playerDoc.DocumentNode.SelectNodes(
                        //                 $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]") !=
                        //             null)
                        //    {
                        //        findStatRow = playerDoc.DocumentNode.SelectNodes(
                        //            $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[3]");
                        //        type = 3;
                        //        seasonId = $"{seasonId} Qualifier";
                        //    }
                        //}
                        //else
                        //{
                        //    findStatRow = playerDoc.DocumentNode.SelectNodes(
                        //        $"//*[@id='lg_team_user_leagues-{system}']/div[1]/table/tbody/tr[{type}]");
                        //}
                        //if (findStatRow == null)
                        //{
                        //    return message = Missing.NotFound(found.playerName, found.System, found.playerUrl);
                        //}
                        #endregion

                        var position = "";
                        var teamIcon = playerDoc.DocumentNode
                            .SelectSingleNode("//*[@id='content']/div/div/div[3]/div[1]/div/table/thead/tr/th/div/a/img").Attributes["src"].Value;
                        var teamIconLink = string.Join(string.Empty, "http://leaguegaming.com" + teamIcon);

                        var lastFiveGames = playerDoc.DocumentNode.SelectNodes($"//*[@id='lg_team_user_leagues-{system}']/div[2]/table/tbody/tr");
                        foreach (var recent in lastFiveGames)
                        {
                            position = recent.SelectSingleNode($"//*[@id='lg_team_user_leagues-{system}']/div[2]/table/tbody/tr[1]/td[2]").InnerText;
                            break;
                        }


                        var table = playerDoc.DocumentNode
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
                                PlayerName = found.playerName,
                                PlayerUrl = found.playerUrl,
                                System = found.System,
                                SystemIcon = systemIcon,
                                TeamIcon = playerDoc.DocumentNode
                                    .SelectSingleNode("//*[@id='content']/div/div/div[3]/div[1]/div/table/thead/tr/th/div/a/img").Attributes["src"].Value,
                                Position = position,
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

                        return table;

                        //foreach (var pStat in table)
                        //{
                        //    var playerName = found.playerName;
                        //    var teamIcon = string.Join(String.Empty, "http://www.leaguegaming.com" + tempTeamIcon);

                        //    return SeasonHelper.SeasonEmbed(found.playerName, playerSystem, systemIcon, pStat.Record, pStat.AvgMatchRating,
                        //        pStat.Goals.ToString(), pStat.Assists, pStat.ShotsOnTarget, pStat.ShotAttempts, pStat.PassesCompleted, pStat.PassesAttempted, pStat.KeyPasses,
                        //        pStat.Interceptions, pStat.Tackles, pStat.TackleAttempts, pStat.PossW, pStat.PossL, pStat.Blocks, pStat.RedCards, pStat.YellowCards, seasonId, found.playerUrl,
                        //        teamIcon, position);
                        //}
                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error($"Error processing stats. {e}");
                        return null;
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
