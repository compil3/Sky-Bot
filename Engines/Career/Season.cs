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

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var table = playerDoc.DocumentNode
                    .SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody")
                    .Descendants("tr")
                    .Skip(0)
                    .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                    .Where(tr => tr[0] == seasonId && tr[1] == "Reg")
                    .Select(tr => new CareerProperties
                    {
                        SeasonType = tr[0],
                        GamesPlayed = tr[1],
                        Record = tr[1],
                        MatchRating = tr[2],
                        AvgMatchRating = "0",
                        Goals = tr[3],
                        GoalsPerGame = "0",
                        Assists = tr[4],
                        ShotsOnTarget = tr[5],
                        ShotAttempts = tr[6],
                        ShotPercentage = "0",
                        ShotPerGame = "0",
                        ShotPerGoal = "0",
                        ShotSot = "0",
                        PassesCompleted = tr[7],
                        PassesAttempted = tr[8],
                        PassRecord = "0",
                        KeyPasses = tr[9],
                        KeyPassPerGame = "0",
                        PassingPercentage = "0",
                        PassPerGame = "0",
                        AssistPerGame = "0",

                        Interceptions = tr[10],
                        Tackles = tr[11],
                        TackleAttempts = tr[12],
                        PossW = tr[13],
                        PossL = tr[14],
                        Poss = "0",

                        Blocks = tr[15],
                        Wall = "0",
                        Tackling = "0",
                        TacklePercent = "0",
                        TacklesPerGame = "0",
                        InterPerGame = "0",
                        BlocksPerGame = "0",
                        RedCards = tr[16],
                        YellowCards = tr[17],
                        Discipline = "0",

                    }).ToList();
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
