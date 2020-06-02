using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using LGFA.Properties;
using Serilog;

namespace LGFA.Engines.Career
{
    internal class Official
    {
        public static (List<CareerProperties>, string playerUrl, string playerName) OfficialParse(
            HtmlDocument playerDoc, string foundPlayerUrl, string foundPlayerName, int system)
        {
            try
            {
                var num = 5;
                if (playerDoc.DocumentNode.SelectSingleNode($"//*[@id='lg_team_user_leagues-{system}']/div[{num}]/table") == null)
                {
                    num = 4;
                } 
                var table = playerDoc.DocumentNode
                    .SelectSingleNode($"//*[@id='lg_team_user_leagues-{system}']/div[{num}]/table")
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
                        PlayerName = foundPlayerName,
                        PlayerUrl = foundPlayerUrl,
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
                        Discipline = "0"
                    })
                    .ToList();

                return (table, foundPlayerUrl, foundPlayerName);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString());
            }

            return (null, null, null);
        }
    }
}