using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using HtmlAgilityPack;
using LiteDB;
using Sky_Bot.Modules;
using Sky_Bot.Properties;

namespace Sky_Bot.Engines
{
    public class CareerSeason
    {
        public static Embed CareerSeasonEmbed(HtmlDocument playerDoc, string foundPlayerUrl, string foundPlayerName, string seasonId, int leagueId)
        {
            HtmlNodeCollection findCareerNode = null;
            var div = 0;

            if (!seasonId.Contains("S")) seasonId = "S" + seasonId;

            var web = new HtmlWeb();
            try
            {
                using (var playerDatabase = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared"))
                {
                    var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");
                    player.EnsureIndex(x => x.playerName);

                    var result = player.Query()
                        .Where(x => x.playerName.Contains(foundPlayerName))
                        .ToList();
                    var playerVirutalCareer = new HtmlDocument();

                    foreach (var found in result)
                    {
                        playerVirutalCareer = web.Load(found.playerUrl);
                    }
                    findCareerNode =
                         playerDoc.DocumentNode.SelectNodes($"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table/tbody/tr");

                    if (findCareerNode == null)
                    {
                        findCareerNode = playerDoc.DocumentNode.SelectNodes(
                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr");
                        div = 4;
                    }
                    else div = 3;
                    var index = 1;


                    //this works to collect all the rows, but it's multidimensional.
                    //need to add Season = "S16", Type = "Reg", GP = "20" etc to the list to make accessing data faster than using foreach loop.
                    List<List<string>> table = playerDoc.DocumentNode
                        .SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody")
                        .Descendants("tr")
                        .Skip(1)
                        .Where(tr => tr.Elements("td").Count() > 1)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                        .ToList();

                   

                    var count = findCareerNode.Count;
                    foreach (var careerStats in findCareerNode)
                    {
                        if (WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[1]").InnerText).Trim() != seasonId)
                        {
                            index++;
                            continue;
                        }else if (WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[2]")
                            .InnerText).Trim() != "Reg")
                        {
                            index++;
                            continue;
                        }
                        var type = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[2]")
                            .InnerText);
                        var record = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[3]")
                            .InnerText);
                        var amr = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[4]")
                            .InnerText) ?? "0.0";
                        var goals = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[5]")
                            .InnerText);
                        var assists = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[6]")
                            .InnerText);
                        var sot = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[7]")
                            .InnerText);
                        var shots = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[8]")
                            .InnerText);
                        var passC = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[9]")
                            .InnerText);
                        var passA = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[10]")
                            .InnerText);
                        var keypass = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[11]")
                            .InnerText);
                        var interceptions = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[12]")
                            .InnerText);
                        var tac = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[13]")
                            .InnerText);
                        var tacA = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[14]")
                            .InnerText);
                        var blk = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[15]")
                            .InnerText);
                        var rc = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[16]")
                            .InnerText);
                        var yc = WebUtility.HtmlDecode(careerStats
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody/tr[{index}]/td[17]")
                            .InnerText);
                        GC.Collect();

                        return EmbedHelpers.CareerEmbed(foundPlayerName, foundPlayerUrl, record, amr,
                            goals, assists, sot, shots, passC,
                            passA, keypass, interceptions, tac, tacA, blk, rc, yc, type, seasonId);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return EmbedHelpers.NotFound(foundPlayerName, foundPlayerUrl);

        }

        internal static void CareerSeasonEmbed(List<PlayerProperties.PlayerInfo> result)
        {
            throw new NotImplementedException();
        }
    }
}

