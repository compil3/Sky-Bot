using System;
using System.Collections.Generic;
using System.Text;
using LGFA.Properties;
using LiteDB;
using Serilog;
using LGFA.Engines;
using LGFA.Schedule;
using Player = LGFA.Engines.Player;

namespace LGFA.Essentials
{
    class LGWriter
    {
        public static bool SaveGoalie(int ID, string System, string playerName, string gamesPlayed, string record,
            string goalsAgainst, string shotsAgainst, string saves, string savePercentage, string goalsAgainstAvg,
            string cleanSheets, string manOfTheMatch, string avgMatchRating, string playerURL, string iconURL, string Command, int SeasonId, string SeasonTypeId)
        {
            var dbName = "";
            var tableName = "";

            if (Command == "schedule")
            {
                dbName = "LGFA_Current.db";
                if (System == "xbox" || System == "psn")
                {
                    var currentSeasonID = int.Parse(GetCurrentSeason.GetSeason(System));
                    if (currentSeasonID == SeasonId)
                    {
                        if (SeasonTypeId == "regular") tableName = "CurrentSeason_Player";
                        else if (SeasonTypeId == "pre-season") tableName = "CurrentPreseason_Player";
                    }
                }
            }
            else if (Command == "uh")
            {
                if (System == "xbox" || System == "PSN")
                {
                    var histSeason = SeasonId;
                    if (SeasonTypeId == "regular")
                    {
                        tableName = "RegularSeason_Player" + SeasonId;
                        dbName = "Historical.db";
                    }
                    else if (SeasonTypeId == "pre-season")
                    {
                        tableName = "PreSeason_Player" + SeasonId;
                        dbName = "Historical.db";
                    }
                }
            }

            using (var database =new LiteDatabase(dbName) {})
            {
                var playerCollection = database.GetCollection<PlayerProperties.StatProperties>(tableName);
                playerCollection.EnsureIndex(x => x.SeasonId);

                var playerStats = new PlayerProperties.StatProperties
                {
                    SeasonId = SeasonId,
                    SeasonTypeId = SeasonTypeId,
                    Id = ID,
                    UserSystem = System,
                    PlayerName = playerName,
                    GamesPlayed = gamesPlayed,
                    Record = record,
                    GoalsAgainst = goalsAgainst,
                    GoalsAgainstAvg = goalsAgainstAvg,
                    ShotsAgainst = shotsAgainst,
                    Saves = saves,
                    SavePercentage = savePercentage,
                    CleanSheets = cleanSheets,
                    ManOfTheMatch = manOfTheMatch,
                    AvgMatchRating = avgMatchRating,
                    PlayerUrl = playerURL,
                    TeamIcon = iconURL
                };
                try
                {
                    var playerFound = playerCollection.FindById(ID);
                    if (playerFound != null)
                    {
                        playerCollection.Update(playerStats);
                        return true;
                    }
                    else
                    {
                        playerCollection.Insert(playerStats);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex,$"Error saving Player Statistics to {dbName}");
                }
            }
            return false;
        }

        public static bool SavePlayer(int ID, int SeasonId, string SeasonTypeId, string Position, string PlayerName,
            string GamesPlayed, string Record, string AvgMatchRating, string Goals, string Assists, string CleanSheets,
            string ShotsOnGoal,
            string ShotsOnTarget, string ShotPercentage, string Tackles, string TackleAttempts, string TacklePercentage,
            string PassPercentage, string KeyPasses, string Interceptions, string Blocks,
            string YellowCards, string RedCards, string ManOfTheMatch, string PlayerURL, string System,
            string TeamIconURL, string Command,string position, string pcnOrLG)
        {
            var dbName = "";
            var tableName = "";

            if (Command == "schedule")
            {
                dbName = "LGFA_Current.db";
                if (System == "xbox" || System == "psn")
                {
                    var currentSeasonId = int.Parse(GetCurrentSeason.GetSeason(System));
                    if (currentSeasonId == SeasonId)
                    {
                        if (SeasonTypeId == "regular") tableName = "CRS_Player";
                        else if (SeasonTypeId == "pre-season") tableName = "CPS_Player";
                    }
                }
            }
            else if (Command == "uh")
            {
                if (System == "xbox" || System == "psn")
                {
                    var historicalSeason = SeasonId;
                    if (SeasonTypeId == "regular")
                    {
                        tableName = "HRS_Player" + SeasonId;
                        dbName = "Historical_Reg_Player.db";
                    }
                    else if (SeasonTypeId == "pre-season")
                    {
                        tableName = "HPS_Player" + SeasonId;
                        dbName = "Historical_Pre_Player.db";
                    }
                }
            }


            using (var database = new LiteDatabase(dbName))
            {
                var playerCollection = database.GetCollection<PlayerProperties.StatProperties>(tableName);
                playerCollection.EnsureIndex(x => x.SeasonId);

                var playerStats = new PlayerProperties.StatProperties
                {
                    SeasonId = SeasonId,
                    Id = ID,
                    TeamIcon = TeamIconURL,
                    Position = Position,
                    PlayerName = PlayerName,
                    GamesPlayed = GamesPlayed,
                    Record = Record,
                    AvgMatchRating = AvgMatchRating,
                    Goals = Goals,
                    Assists = Assists,
                    CleanSheets = CleanSheets,
                    ShotsOnGoal = ShotsOnGoal,
                    ShotsOnTarget = ShotsOnTarget,
                    ShotPercentage = ShotPercentage,
                    Tackles = Tackles,
                    TackleAttempts = TackleAttempts,
                    TacklePercentage = TacklePercentage,
                    PassingPercentage = PassPercentage,
                    KeyPasses = KeyPasses,
                    Interceptions = Interceptions,
                    Blocks = Blocks,
                    YellowCards = YellowCards,
                    RedCards = RedCards,
                    ManOfTheMatch = ManOfTheMatch,
                    PlayerUrl = PlayerURL,
                    UserSystem = System,
                    SeasonTypeId = SeasonTypeId,
                };
                try
                {
                    var playerFound = playerCollection.FindById(ID);
                    if (playerFound != null)
                    {
                        playerCollection.Update(playerStats);
                        return true;
                    }
                    else
                    {
                        playerCollection.Insert(playerStats);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error saving Historical Stats to database");
                }
            }

            return false;
        }
    }
}
