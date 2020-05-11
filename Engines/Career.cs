using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Discord;
using HtmlAgilityPack;
using LiteDB;
using Serilog;
using Sky_Bot.Essentials.Writer;
using Sky_Bot.Modules;
using Sky_Bot.Properties;

namespace Sky_Bot.Engines
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
                using (var playerDatabase = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared"))
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

                            #region Parsing Nodes

                            HtmlNodeCollection findCareerNode = null;
                            if (seasonId == null)
                            {
                                //find the career table on the players profile.
                                findCareerNode =
                                    playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody");
                                var count = findCareerNode.Count;

                                if (findCareerNode == null
                                ) //if the table isn't found above due to a season in progress (table will show), set /div to 4
                                {
                                    findCareerNode = playerDoc.DocumentNode.SelectNodes(
                                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr[1]");
                                    div = 4;
                                }
                                else div = 5;
                                foreach (var careerStats in findCareerNode)
                                {
                                    var record = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[2]")
                                        .InnerText);
                                    var amr = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[3]")
                                        .InnerText);
                                    var goals = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[4]")
                                        .InnerText);
                                    var assists = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[5]")
                                        .InnerText);
                                    var sot = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[6]")
                                        .InnerText);
                                    var shots = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[7]")
                                        .InnerText);
                                    var passC = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[8]")
                                        .InnerText);
                                    var passA = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[9]")
                                        .InnerText);
                                    var key = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[10]")
                                        .InnerText);
                                    var interceptions = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[11]")
                                        .InnerText);
                                    var tac = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[12]")
                                        .InnerText);
                                    var tacA = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[13]")
                                        .InnerText);
                                    var blk = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[14]")
                                        .InnerText);
                                    var rc = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[15]")
                                        .InnerText);
                                    var yc = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[16]")
                                        .InnerText);
                                    GC.Collect();

                                    #endregion

                                    return EmbedHelpers.CareerEmbed(found.playerName, found.playerUrl, record, amr,
                                        goals, assists, sot, shots, passC,
                                        passA, key, interceptions, tac, tacA, blk, rc, yc);
                                }
                            }
                            else //if a season is entered
                            {
                                return CareerSeason.CareerSeasonEmbed(playerDoc, found.playerUrl, found.playerName, seasonId, leagueId);
                                #region Season is entered
                                ////check Type against user input, if nothing entered use Reg else match.
                                //if (playerDoc.DocumentNode.InnerHtml.ToString().Contains(seasonId))
                                //{
                                //    findCareerNode = playerDoc.DocumentNode.SelectNodes(
                                //        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table/tbody/tr[1]");
                                //    if (findCareerNode == null)
                                //    {
                                //        findCareerNode = playerDoc.DocumentNode.SelectNodes(
                                //            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr[1]");
                                //        div = 4;
                                //    }
                                //    else div = 3;

                                //    foreach (var careerStats in findCareerNode)
                                //    {
                                //        var season = WebUtility.HtmlDecode(careerStats
                                //       .SelectSingleNode(
                                //           $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[1]")
                                //       .InnerText);
                                      
                                //        var type = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[2]")
                                //            .InnerText);
                                //        var record = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[3]")
                                //            .InnerText);
                                //        var amr = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[4]")
                                //            .InnerText) ?? "0.0";
                                //        var goals = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[5]")
                                //            .InnerText);
                                //        var assists = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[6]")
                                //            .InnerText);
                                //        var sot = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[7]")
                                //            .InnerText);
                                //        var shots = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[8]")
                                //            .InnerText);
                                //        var passC = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[9]")
                                //            .InnerText);
                                //        var passA = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[10]")
                                //            .InnerText);
                                //        var keypass = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[11]")
                                //            .InnerText);
                                //        var interceptions = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[12]")
                                //            .InnerText);
                                //        var tac = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[13]")
                                //            .InnerText);
                                //        var tacA = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[14]")
                                //            .InnerText);
                                //        var blk = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[15]")
                                //            .InnerText);
                                //        var rc = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[16]")
                                //            .InnerText);
                                //        var yc = WebUtility.HtmlDecode(careerStats
                                //            .SelectSingleNode(
                                //                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[17]")
                                //            .InnerText);
                                //        GC.Collect();



                                //        return EmbedHelpers.CareerEmbed(found.playerName, found.playerUrl, record, amr,
                                //            goals, assists, sot, shots, passC,
                                //            passA, keypass, interceptions, tac, tacA, blk, rc, yc);
                                //        #endregion
                                //    }
                                #endregion
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
