using System;
using System.Collections.Generic;
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
        public static Embed GetCareer(string lookUpPlayer)
         {
            Embed message = null;
            var leagueId = 0;
            var web = new HtmlWeb();
            try
            {
                using (var playerDatabase = new LiteDatabase(@"Database\LGFA.db"))
                {
                    var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");

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

                            
                            var findCareerNode =
                                playerDoc.DocumentNode.SelectNodes(
                                    $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]");
                                foreach (var careerStats in findCareerNode)
                                {
                                    
                                    var record = WebUtility.HtmlDecode(careerStats.SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[2]").InnerText);
                                    var amr = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[3]")
                                        .InnerText);
                                    var goals = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[4]")
                                        .InnerText);
                                    var assists = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[5]")
                                        .InnerText);
                                    var sot = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[6]")
                                        .InnerText);
                                    var shots = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[7]")
                                        .InnerText);
                                    var passC = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[8]")
                                        .InnerText);
                                    var passA = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[9]")
                                        .InnerText);
                                    var key = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[10]")
                                        .InnerText);
                                    var interceptions = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[11]")
                                        .InnerText);
                                    var tac = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[12]")
                                        .InnerText);
                                    var tacA = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[13]")
                                        .InnerText);
                                    var blk = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[14]")
                                        .InnerText);
                                    var rc = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[15]")
                                        .InnerText);
                                    var yc = WebUtility.HtmlDecode(careerStats
                                        .SelectSingleNode(
                                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[16]")
                                        .InnerText);
                                    GC.Collect();

                                    return EmbedHelpers.CareerEmbed(found.playerName, found.playerUrl,record, amr, goals, assists, sot, shots, passC,
                                        passA, key, interceptions, tac, tacA, blk, rc, yc);
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

        private static string PlayerUrl(string lookup)
        {
            using (var playerUrl = new LiteDatabase(@"LGFA.db"))
            {
                var player = playerUrl.GetCollection<PlayerProperties.PlayerInfo>("URLS");
                var result = player.Find(x => x.playerName.StartsWith(lookup));
                foreach (var url in result)
                {
                    var playerId = url.Id;
                    var name = url.playerName;
                    var webUrl = url.playerUrl;
                    return webUrl;
                }
            }
            return null;
        }
    }
}
