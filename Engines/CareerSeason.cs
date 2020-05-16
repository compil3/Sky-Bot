using Discord;
using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LGFA.Modules;
using LGFA.Modules.Helpers;
using LGFA.Properties;

namespace LGFA.Engines
{
    public class CareerSeason
    {
        public static Embed CareerSeasonEmbed(HtmlDocument playerDoc, string foundPlayerUrl, string foundPlayerName, string seasonId, int leagueId)
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

                // Now the return type is a List of CareerProperties.
                var table = playerDoc.DocumentNode
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
                        something = tr[0],
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
                        PossW =tr[14],
                        PossL = tr[15],
                        Blocks = tr[16],
                        RedCards = tr[17],
                        YellowCards = tr[18]

                    })
                    .ToList();

                foreach (var careerStats in table)
                {
                    return CareerEmbeds.CareerSeasonalEmbed(foundPlayerName, foundPlayerUrl, careerStats.Record, careerStats.AvgMatchRating,
                        careerStats.Goals, careerStats.Assists, careerStats.ShotsOnTarget, careerStats.ShotAttempts, careerStats.PassesCompleted,
                        careerStats.PassesAttempted, careerStats.KeyPasses, careerStats.Interceptions, careerStats.Tackles, careerStats.TackleAttempts,careerStats.PossW, careerStats.PossL,
                        careerStats.Blocks, careerStats.RedCards, careerStats.YellowCards, "Reg", seasonId);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString());;
            }
            return Missing.NotFound(foundPlayerName, foundPlayerUrl);

        }

        public static Embed CareerEmbed(HtmlDocument playerDoc, string foundPlayerUrl, string foundPlayerName, int system)
        {
            try
            {
                var table = playerDoc.DocumentNode
                    .SelectSingleNode($"//*[@id='lg_team_user_leagues-{system}']/div[5]/table")
                    .Descendants("tr")
                    .Skip(1)
                    // Up to here is your code. Here you select all rows from the table.
                    // Each row is presented as List<string>.
                    .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                    // Here we filter table rows by "seasonId" and "Reg".
                    .Where(tr => tr[0] == "Official")
                    // Here we create objects CareerProperties from filtered rows.
                    .Select(tr => new CareerProperties
                    {
                        SeasonType = tr[0],
                        Record = tr[1],
                        MatchRating = tr[2],
                        Goals = tr[3],
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
                foreach (var player in table)
                {
                    return CareerEmbeds.CareerSeasonEmbed(foundPlayerName, foundPlayerUrl, player.Record,
                        player.AvgMatchRating,
                        player.Goals, player.Assists, player.ShotsOnTarget, player.ShotAttempts, player.PassesCompleted,
                        player.PassesAttempted, player.KeyPasses, player.Interceptions, player.Tackles,
                        player.TackleAttempts, player.PossW, player.PossL,
                        player.Blocks, player.RedCards, player.YellowCards);
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString());
            }
            return Missing.NotFound(foundPlayerName, foundPlayerUrl);
        }

    }
}

