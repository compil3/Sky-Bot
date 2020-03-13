using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using Serilog;
using Sky_Bot.Engines;
using Sky_Bot.Properties;
using Sky_Bot.Schedule;
using Player = Sky_Bot.Engines.Player;

namespace Sky_Bot.Essentials
{
    class PlayerWriter
    {
        public static bool Save(int ID, int SeasonId, string SeasonTypeId, string Position, string PlayerName,
            string GamesPlayed, string Record, string AvgMatchRating, string Goals, string Assists, string CleanSheets,
            string ShotsOnGoal,
            string ShotsOnTarget, string ShotPercentage, string Tackles, string TackleAttempts, string TacklePercentage,
            string PassPercentage, string KeyPasses, string Interceptions, string Blocks,
            string YellowCards, string RedCards, string ManOfTheMatch, string PlayerURL, string System,
            string TeamIconURL, string Command)
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
                    PlayerSystem = System,
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
                    Log.Fatal(ex,$"Error saving Player Statistics to {dbName}");
                }
            }
            return false;
        }
    }
}
