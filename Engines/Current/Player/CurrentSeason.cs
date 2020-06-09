using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using HtmlAgilityPack;
using LGFA.Extensions;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace LGFA.Engines.Current.Player
{
    internal class CurrentSeason
    {
        public static (List<CareerProperties>, string playerName) SeasonStats(string playerLookup)
        {
            var web = new HtmlWeb();
            var season = "";
            var leagueId = "";
            var systemIcon = "";
            List<LeagueProperties> currentSeason = null;


            var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(dbPath, dbFolder);
            using var playerDatabase = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");
            var div = 0;
            player.EnsureIndex(x => x.playerName);

            var result = player.Query()
                .Where(x => x.playerName.Contains(playerLookup))
                .ToList();

            foreach (var found in result)
            {
                var playerDoc = web.Load(found.playerUrl);
                leagueId = found.System switch
                {
                    "psn" => "73",
                    "xbox" => "53",
                    _ => leagueId
                };

                switch (found.System)
                {
                    case "psn":
                        systemIcon =
                            "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                        currentSeason = LeagueInfo.GetSeason("psn");
                        break;
                    case "xbox":
                        systemIcon =
                            "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                        currentSeason = LeagueInfo.GetSeason("xbox");
                        break;
                }

                if (currentSeason != null)
                    foreach (var leagueProp in currentSeason)
                    {
                        if (!leagueProp.Season.Contains("S")) season = "S" + leagueProp.Season;
                        break;
                    }

                try
                {
                    var findCareerNode = playerDoc.DocumentNode.SelectNodes(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table/tbody/tr");

                    div = 0;
                    if (findCareerNode == null)
                    {
                        findCareerNode = playerDoc.DocumentNode.SelectNodes(
                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr");
                        div = 4;
                    }
                    else
                    {
                        div = 3;
                    }

                    var position = "";
                    var lastFiveGames =
                        playerDoc.DocumentNode.SelectNodes(
                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[2]/table/tbody/tr");
                    foreach (var recent in lastFiveGames)
                    {
                        position = recent
                            .SelectSingleNode(
                                $"//*[@id='lg_team_user_leagues-{leagueId}']/div[2]/table/tbody/tr[1]/td[2]").InnerText;
                        break;
                    }

                    var table = playerDoc.DocumentNode
                        .SelectSingleNode($"//*[@id='lg_team_user_leagues-{leagueId}']/div[{div}]/table/tbody")
                        .Descendants("tr")
                        .Skip(0)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                        .Where(tr => tr[0] == season && tr[1] == "Reg")
                        .Select(tr => new CareerProperties
                        {
                            PlayerName = found.playerName,
                            PlayerUrl = found.playerUrl,
                            System = found.System,
                            SystemIcon = systemIcon,
                            TeamIcon = GetTeamIcon(found.playerUrl),
                            Position = position,
                            SeasonId = season,

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
                            AssistPerGame = "0",

                            PassRecord = "0",
                            KeyPasses = tr[10],
                            KeyPassPerGame = "0",
                            PassingPercentage = "0",
                            PassPerGame = "0",

                            Interceptions = tr[11],
                            Tackles = tr[12],
                            TackleAttempts = tr[13],
                            PossW = tr[14],
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
                    return (table, found.playerName);
                }
                catch (NullReferenceException e)
                {
                    Log.Logger.Error($"Error processing stats. {e}");
                    return (null, found.playerName);
                }
            }

            return (null, null);
        }

        private static string GetTeamIcon(string foundPlayerUrl)
        {
            var playerPage = new HtmlWeb();
            var tempPage = playerPage.Load(foundPlayerUrl);

            var iconTemp = tempPage.DocumentNode.SelectSingleNode(
                "//*[@id='content']/div/div/div[3]/div[1]/div/table/thead/tr/th/div/a/img")
                    .Attributes["src"].Value;
            var icon = "http://www.leaguegaming.com" + iconTemp.Replace("p16", "p100");
            return icon;

        }
    }
}