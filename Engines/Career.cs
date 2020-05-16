using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Discord;
using HtmlAgilityPack;
using LGFA.Modules;
using LGFA.Properties;
using LiteDB;
using Serilog;
using LGFA.Essentials.Writer;

namespace LGFA.Engines
{
    class Career
    {
        public static Embed GetCareer(string lookUpPlayer, string seasonId)
        {
            Embed message = null;
            var leagueId = 0;
            var web = new HtmlWeb();
            try
            {
                var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var dbFolder = "Database/";
                var dbDir = Path.Combine(dbPath, dbFolder);
                using (var playerDatabase = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared"))
                {
                    var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");
                    var div = 1;
                    player.EnsureIndex(x => x.playerName);

                    var result = player.Query()
                        .Where(x => x.playerName.Contains(lookUpPlayer))
                        .ToList();


                    foreach (var found in result)
                    {
                        if (found.System == "psn") leagueId = 73;
                        else if (found.System == "xbox") leagueId = 53;
                        try
                        {
                            var playerDoc = web.Load(found.playerUrl);
                            //var testNode =
                            //    playerDoc.DocumentNode.SelectNodes("//*[@id='lg_team_user_leagues-53']/div[3]/table/tbody");
                            //var sCount = testNode.Count;

                            

                            HtmlNodeCollection findCareerNode = null;
                            if (seasonId == null)
                            {
                                #region Parsing Nodes
                                //find the career table on the players profile.
                                //findCareerNode =
                                //    playerDoc.DocumentNode.SelectNodes(
                                //        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody");
                                //var count = findCareerNode.Count;

                                //if (findCareerNode == null) //if the table isn't found above due to a season in progress (table will show), set /div to 4
                                //{
                                //    findCareerNode = playerDoc.DocumentNode.SelectNodes(
                                //        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr[1]");
                                //    div = 4;
                                //}
                                //else div = 5;
                                //foreach (var careerStats in findCareerNode)
                                //{
                                //    var record = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[2]")
                                //        .InnerText);
                                //    var amr = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[3]")
                                //        .InnerText);
                                //    var goals = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[4]")
                                //        .InnerText);
                                //    var assists = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[5]")
                                //        .InnerText);
                                //    var sot = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[6]")
                                //        .InnerText);
                                //    var shots = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[7]")
                                //        .InnerText);
                                //    var passC = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[8]")
                                //        .InnerText);
                                //    var passA = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[9]")
                                //        .InnerText);
                                //    var key = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[10]")
                                //        .InnerText);
                                //    var interceptions = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[11]")
                                //        .InnerText);
                                //    var tac = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[12]")
                                //        .InnerText);
                                //    var tacA = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[13]")
                                //        .InnerText);
                                //    var blk = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[14]")
                                //        .InnerText);
                                //    var rc = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[15]")
                                //        .InnerText);
                                //    var yc = WebUtility.HtmlDecode(careerStats
                                //        .SelectSingleNode(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[16]")
                                //        .InnerText);
                                //    GC.Collect();

                                    #endregion

                                   
                                return CareerSeason.CareerEmbed(playerDoc, found.playerUrl, found.playerName, leagueId);
                            }
                            else //if a season is entered
                            {
                                return CareerSeason.CareerSeasonEmbed(playerDoc, found.playerUrl, found.playerName, seasonId, leagueId);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return message;
        }
    }
}
