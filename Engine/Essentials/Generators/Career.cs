using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Engine.Properties;
using HtmlAgilityPack;
using LiteDB;
using Serilog;
using static Engine.Essentials.Write.PlayWriter;

namespace Engine.Essentials.Generators
{
    internal class Career
    {
        public static bool GetStatistics(int id, string playerName, string system)
        {
            var web = new HtmlWeb();
            var doc = web.Load(FindUrl(playerName));
            var leagueId = "";
            double matchRating;

            if (system == "xbox") leagueId = "53";
            else if (system == "psn") leagueId = "73";

            var careerPage =
                doc.DocumentNode.SelectNodes(
                    $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[1]");

            try
            {
                var divNum = 0;
                double passPercentage;
                double shotPercentage;
                if (careerPage == null)
                {
                    careerPage =
                        doc.DocumentNode.SelectNodes(
                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr[1]/td[1]");
                    divNum = 4;

                    #region Document Nodes
                    foreach (var careerStats in careerPage)
                    {
                        var careerRecord = WebUtility.HtmlDecode(careerStats
                             .SelectSingleNode(
                                 $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[2]")
                             .InnerText);
                        string[] splitRecord = careerRecord.Split('-');
                        int wins = int.Parse(splitRecord[0]);
                        int draws = int.Parse(splitRecord[1]);
                        int loses = int.Parse(splitRecord[2]);
                        double officalGames = wins + draws + loses;

                        var amr = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[3]")
                            .InnerText));
                        matchRating = amr == null ? 0 : Math.Round((amr / officalGames * 100), 2);

                        var goals = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[4]")
                            .InnerText));
                        var assists = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[5]")
                            .InnerText);

                        var shotOnTarget = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[6]")
                            .InnerText);
                        var shotAttempts = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[7]")
                            .InnerText));
                        //shotPercentage = double.Round((goals / shotAttempts) * 100, 1);
                        shotPercentage = Math.Round((goals / shotAttempts * 100), 2);

                        var passesCompleted = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[8]")
                            .InnerText));
                        var passAttempts = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[9]")
                            .InnerText));
                        if (passesCompleted == 0 && passAttempts == 0) passPercentage = 0;
                        else passPercentage = Math.Round((passesCompleted / passAttempts * 100), 2);
                        //passPercentage = decimal.Round((passesCompleted / passAttempts)*100, 1);
                        var keyPasses = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[10]")
                            .InnerText);

                        var interceptions = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[11]")
                            .InnerText);
                        var tackles = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[12]")
                            .InnerText));
                        var tackleAttempts = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[13]")
                            .InnerText));
                        //var tacklePercentage = decimal.Round((tackles / tackleAttempts) *100 , 1);
                        var tacklePercentage = Math.Round((tackles / tackleAttempts * 100), 2);

                        var blocks = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[14]")
                            .InnerText);

                        var redCards = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[15]")
                            .InnerText);
                        var yellowCards = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[16]")
                            .InnerText);

                        SaveCareer(id, playerName, careerRecord, officalGames, amr,
                            goals,
                            assists, shotOnTarget, shotAttempts, shotPercentage, passesCompleted, passAttempts,
                            passPercentage, keyPasses, interceptions, tackles, tackleAttempts, tacklePercentage, blocks,
                            redCards, yellowCards);
                        GC.Collect();
                    }
                    return true;
                }
                else
                {
                    careerPage = doc.DocumentNode.SelectNodes(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[5]/table/tbody/tr[1]/td[3]");
                    divNum = 5;
                    foreach (var careerStats in careerPage)
                    {
                        //type = Offical or Pre-Season etc
                        //var type = WebUtility.HtmlDecode(careerStats.SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[1]").InnerText);
                        var careerRecord = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[2]")
                            .InnerText);
                        string[] splitRecord = careerRecord.Split('-');
                        int wins = int.Parse(splitRecord[0]);
                        int draws = int.Parse(splitRecord[1]);
                        int loses = int.Parse(splitRecord[2]);
                        double officalGames = wins + draws + loses;

                        var amr = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[3]")
                            .InnerText));
                        matchRating = amr == null ? 0 : Math.Round((amr / officalGames * 100), 2);

                        var goals = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[4]")
                            .InnerText));
                        var assists = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[5]")
                            .InnerText);

                        var shotOnTarget = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[6]")
                            .InnerText);
                        var shotAttempts = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[7]")
                            .InnerText));
                        //shotPercentage = double.Round((goals / shotAttempts) * 100, 1);
                        shotPercentage = Math.Round((goals / shotAttempts * 100), 2);

                        var passesCompleted = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[8]")
                            .InnerText));
                        var passAttempts = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[9]")
                            .InnerText));
                        if (passesCompleted == 0 && passAttempts == 0) passPercentage = 0;
                        else passPercentage = Math.Round((passesCompleted / passAttempts * 100), 2);
                        //passPercentage = decimal.Round((passesCompleted / passAttempts)*100, 1);
                        var keyPasses = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[10]")
                            .InnerText);

                        var interceptions = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[11]")
                            .InnerText);
                        var tackles = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[12]")
                            .InnerText));
                        var tackleAttempts = double.Parse(WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[13]")
                            .InnerText));
                        //var tacklePercentage = decimal.Round((tackles / tackleAttempts) *100 , 1);
                        var tacklePercentage = Math.Round((tackles / tackleAttempts * 100), 2);

                        var blocks = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[14]")
                            .InnerText);

                        var redCards = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[15]")
                            .InnerText);
                        var yellowCards = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{divNum}]/table/tbody/tr[1]/td[16]")
                            .InnerText);

                        SaveCareer(id, playerName, careerRecord, officalGames, amr,
                            goals,
                            assists, shotOnTarget, shotAttempts, shotPercentage, passesCompleted, passAttempts,
                            passPercentage, keyPasses, interceptions, tackles, tackleAttempts, tacklePercentage, blocks,
                            redCards, yellowCards);
                        GC.Collect();
                    }
                    return true;
                    #endregion
                }
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, $"Saving {playerName} to database failed.");
                return false;
            }
        }

        private static string FindUrl(string lookupName)
        {
            using var playerUrl = new LiteDatabase(@"LGFA.db");
            var player = playerUrl.GetCollection<PlayerProperties.URL>("URLS");
            var result = player.Find(x => x.PlayerName.StartsWith(lookupName));

            return result.Select(url => url.PlayerUrl).FirstOrDefault();
        }
    }
}
