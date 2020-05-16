using System;
using System.Collections.Generic;
using System.Text;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace LGFA.Essentials.Writer
{
    class GoalieWriter
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
                    var currentSeasonId = int.Parse(GetCurrentSeason.GetSeason(System));
                    if (currentSeasonId == SeasonId)
                    {
                        if (SeasonTypeId == "regular") tableName = "CRS_Goalie";
                        else if (SeasonTypeId == "pre-season") tableName = "CPS_Goalie";
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
                        tableName = "HRS_Goalie" + SeasonId;
                        dbName = "Historical_Reg_Goalie.db";
                    }
                    else if (SeasonTypeId == "pre-season")
                    {
                        tableName = "HPS_Goalie" + SeasonId;
                        dbName = "Historical_Pre_Goalie.db";
                    }
                }
            }

            using (var database = new LiteDatabase(dbName))
            {

                var goalieCollection = database.GetCollection<PlayerProperties.GoalieProperties>(tableName);

                goalieCollection.EnsureIndex(x => x.SeasonId);
                var goalieStats = new PlayerProperties.GoalieProperties
                {
                    SeasonId = SeasonId,
                    SeasonTypeId = SeasonTypeId,
                    Id = ID,
                    userSystem = System,
                    playerName = playerName,
                    gamesPlayed = gamesPlayed,
                    record = record,
                    goalsAgainst = goalsAgainst,
                    goalsAgainstAvg = goalsAgainstAvg,
                    shotsAgainst = shotsAgainst,
                    saves = saves,
                    savePercentage = savePercentage,
                    cleanSheets = cleanSheets,
                    manOfTheMatch = manOfTheMatch,
                    avgMatchRating = avgMatchRating,
                    playerURL = playerURL,
                    teamIcon = iconURL
                };
                try
                {
                    if (goalieCollection.FindById(ID) != null)
                    {
                        goalieCollection.Update(goalieStats);
                        return true;
                    }
                    else
                    {
                        goalieCollection.Insert(goalieStats);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Error saving goalie stats to database");
                }
            }
            return false;
        }
    }
}
