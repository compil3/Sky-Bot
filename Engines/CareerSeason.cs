using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Discord;
using HtmlAgilityPack;
using Sky_Bot.Modules;

namespace Sky_Bot.Engines
{
    public class CareerSeason
    {
        public static Embed CareerSeasonEmbed(HtmlDocument playerDoc, string foundPlayerUrl, string foundPlayerName,
            string seasonId, int leagueId)
        {
            HtmlNodeCollection findCareerNode = null;
            var div = 0;

            if (!seasonId.Contains("S"))
            {
                seasonId = "S" + seasonId;
            }
            findCareerNode = playerDoc.DocumentNode.SelectNodes(
                    $"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table");
            if (findCareerNode == null)
            {
                findCareerNode = playerDoc.DocumentNode.SelectNodes(
                    $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table");
                div = 4;
            }
            else div = 3;

            
            var count = findCareerNode.Count;
            foreach (var careerStats in findCareerNode)
            {
                var index = 1;
                var season = WebUtility.HtmlDecode(careerStats
               .SelectSingleNode(
                   $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[1]")
               .InnerText);
                if (season != seasonId)
                {
                    index++;
                    continue;
                }

                var type = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[2]")
                    .InnerText);
                var record = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[3]")
                    .InnerText);
                var amr = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[4]")
                    .InnerText) ?? "0.0";
                var goals = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[5]")
                    .InnerText);
                var assists = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[6]")
                    .InnerText);
                var sot = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[7]")
                    .InnerText);
                var shots = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[8]")
                    .InnerText);
                var passC = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[9]")
                    .InnerText);
                var passA = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[10]")
                    .InnerText);
                var keypass = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[11]")
                    .InnerText);
                var interceptions = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[12]")
                    .InnerText);
                var tac = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[13]")
                    .InnerText);
                var tacA = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[14]")
                    .InnerText);
                var blk = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[15]")
                    .InnerText);
                var rc = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[16]")
                    .InnerText);
                var yc = WebUtility.HtmlDecode(careerStats
                    .SelectSingleNode(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[1]/td[17]")
                    .InnerText);
                GC.Collect();
                
                return EmbedHelpers.CareerEmbed(foundPlayerName, foundPlayerUrl, record, amr,
                    goals, assists, sot, shots, passC,
                    passA, keypass, interceptions, tac, tacA, blk, rc, yc);
            }

            return EmbedHelpers.NotFound(foundPlayerName, foundPlayerUrl);
        }
    }
}

