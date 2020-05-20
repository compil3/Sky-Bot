using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;
using LGFA.Properties;
using Serilog;

namespace LGFA.Engines.Career
{
    class Season
    {
        public static (List<CareerProperties>, string foundPlayerUrl, string foundPlayerName, string seasonId)
            SeasonParse(HtmlDocument playerDoc, string foundPlayerUrl, string foundPlayerName, string seasonId, int leagueId)
        {
            var stopWatch = new Stopwatch();
            if (!seasonId.Contains("S")) seasonId = "S" + seasonId;

            var web = new HtmlWeb();
            try
            {
                var findCareerNode = playerDoc.DocumentNode.SelectNodes(
                    $"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table/tbody/tr");

                var div = 0;
                if (findCareerNode == null)
                {
                    findCareerNode = playerDoc.DocumentNode.SelectNodes(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr");
                    div = 4;
                }
                else div = 3;

                stopWatch.Start();

                var table = playerDoc.DocumentNode
                    .SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody")
                    .Descendants("tr")
                    .Skip(0)
                    .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                    .Where(tr => tr[0] == seasonId && tr[1] == "Reg")
                    .Select(tr => new CareerProperties
                    {
                        PlayerName = foundPlayerName,
                        PlayerUrl = foundPlayerUrl,
                        SeasonId = seasonId,
                        something = tr[0],
                        GamesPlayed = tr[2],
                        Record = tr[2],
                        MatchRating = tr[3],
                        AvgMatchRating = "0",

                        Goals = tr[4],
                        GoalsPerGame = "0",
                        Assists = tr[5],
                        ShotsOnTarget = tr[6],
                        ShotAttempts = tr[7],
                        ShotPercentage = "0",
                        ShotPerGame = "0",
                        ShotPerGoal = "0",
                        ShotSot = "0",
                        PassesCompleted = tr[8],
                        PassesAttempted = tr[9],
                        PassRecord = "0",
                        KeyPasses = tr[10],
                        KeyPassPerGame = "0",
                        PassingPercentage = "0",
                        PassPerGame = "0",
                        AssistPerGame = "0",
                        Interceptions = tr[11],
                        Tackles = tr[12],
                        TackleAttempts = tr[13],
                        PossW =tr[14],
                        PossL = tr[15],
                        Poss = "0",
                        Blocks = tr[16],
                        Wall = "0",
                        Tackling = "0",
                        TacklePercent = "0",
                        TacklesPerGame = "0",
                        InterPerGame = "0",
                        RedCards = tr[17],
                        YellowCards = tr[18],
                        Discipline = "0"

                    }).ToList();
                stopWatch.Stop();
                Log.Logger.Warning($"SeasonParse: {stopWatch.Elapsed}");
                return (table, foundPlayerUrl, foundPlayerName, seasonId);

                //foreach (var careerStats in table)
                //{
                //    return CareerEmbeds.CareerSeasonalEmbed(foundPlayerName, foundPlayerUrl, careerStats.Record, careerStats.AvgMatchRating,
                //        careerStats.Goals, careerStats.Assists, careerStats.ShotsOnTarget, careerStats.ShotAttempts, careerStats.PassesCompleted,
                //        careerStats.PassesAttempted, careerStats.KeyPasses, careerStats.Interceptions, careerStats.Tackles, careerStats.TackleAttempts,careerStats.PossW, careerStats.PossL,
                //        careerStats.Blocks, careerStats.RedCards, careerStats.YellowCards, "Reg", seasonId);
                //}
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString()); ;
            }
            return (null, null, null, null);

        }
    }
}
