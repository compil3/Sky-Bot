using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Discord;
using HtmlAgilityPack;
using LiteDB;
using Microsoft.VisualBasic;
using Serilog;
using Sky_Bot.Modules;
using Sky_Bot.Properties;

namespace Sky_Bot.Engines
{
    public class CareerSeason
    {
        public object SeasonId { get; private set; }

        public static Embed CareerSeasonEmbed(HtmlDocument playerDoc, string foundPlayerUrl, string foundPlayerName, string seasonId, int leagueId)
        {
            HtmlNodeCollection findCareerNode = null;
            var div = 0;

            if (!seasonId.Contains("S")) seasonId = "S" + seasonId;

            var web = new HtmlWeb();
            try
            {

                var playerVirutalCareer = new HtmlDocument();



                findCareerNode =
                    playerDoc.DocumentNode.SelectNodes(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table/tbody/tr");

                if (findCareerNode == null)
                {
                    findCareerNode = playerDoc.DocumentNode.SelectNodes(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr");
                    div = 4;
                }
                else div = 3;

                var index = 1;


                //this works to collect all the rows, but it's multidimensional.
                //need to add Season = "S16", Type = "Reg", GP = "20" etc
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                // Now the return type is a List of CareerProperties.
                List<CareerProperties> table = playerDoc.DocumentNode
                    .SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody")
                    .Descendants("tr")
                    .Skip(0)
                    // Up to here is your code. Here you select all rows from the table.
                    // Each row is presented as List<string>.
                    .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                    // Here we filter table rows by "seasonId" and "Reg".
                    .Where(tr => tr[0] == seasonId && tr[1] == "Reg")
                    // Here we create objects CareerProperties from filtered rows.
                    .Select(tr => new CareerProperties
                    {
                        Record = tr[2],
                        MatchRating = tr[3],
                        Goals = tr[4],
                        Assists = tr[5],
                        ShotsOnTarget = tr[6],
                        ShotAttempts = tr[7],
                        PassesCompleted = tr[8],
                        PassesAttempted = tr[9],
                        KeyPasses = tr[10],
                        Interceptions = tr[11],
                        Tackles = tr[12],
                        TackleAttempts = tr[13],
                        Blocks = tr[14],
                        RedCards = tr[15],
                        YellowCards = tr[16]

                    })
                    .ToList();

                stopWatch.Stop();
                Log.Logger.Warning($"Elapsed: {stopWatch.Elapsed}");

                var temp = "";

                //foreach (var godDamn in data)
                //{
                //    Log.Logger.Warning(godDamn.Season);
                //}
                var count = findCareerNode.Count;
                foreach (var careerStats in table)
                {
                    return EmbedHelpers.CareerEmbed(foundPlayerName, foundPlayerUrl, careerStats.Record, careerStats.AvgMatchRating,
                        careerStats.Goals, careerStats.Assists, careerStats.ShotsOnTarget, careerStats.ShotAttempts, careerStats.PassesCompleted,
                        careerStats.PassesAttempted, careerStats.KeyPasses, careerStats.Interceptions, careerStats.Tackles, careerStats.TackleAttempts,
                        careerStats.Blocks, careerStats.RedCards, careerStats.YellowCards, "Reg", seasonId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return EmbedHelpers.NotFound(foundPlayerName, foundPlayerUrl);

        }

    }
}

