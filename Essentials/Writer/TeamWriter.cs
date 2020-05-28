namespace LGFA.Essentials.Writer
{
    //class TeamWriter
    //{
    //      public static bool SaveTeam(int Id, int rank, string teamName, string gamesPlayed, string gamesWon,
    //        string gamesDrawn, string gamesLost, string Points, string Streak, string goalsFor, string goalsAgainst,
    //        string cleanSheets,
    //        string lastTenGames, string homeRecord, string awayRecord, string oneGoalGames, string teamIcon,
    //        string teamUrl, string system, int seasonId, string seasonType, string Command)
    //    {
    //        var dbName = "";
    //        var tableName = "";

    //        if (Command == "schedule")
    //        {
    //            dbName = "LGFA_Current.db";
    //            if (system == "xbox" || system == "psn")
    //            {
    //                var currentSeasonId = int.Parse(GetCurrentSeason.GetSeason(system));
    //                if (currentSeasonId == seasonId)
    //                {
    //                    if (seasonType == "regular") tableName = "CRS_Team";
    //                    else if (seasonType == "pre-season") tableName = "CPS_Team";
    //                }
    //            }
    //        }
    //        else if (Command == "uh")
    //        {
    //            if (system == "xbox" || system == "psn")
    //            {
    //                var historicalSeason = seasonId;
    //                if (seasonType == "regular")
    //                {
    //                    tableName = "Regular" + seasonId;
    //                    dbName = "Historical_Reg_Team.db";
    //                }
    //                else if (seasonType == "pre-season")
    //                {
    //                    {
    //                        tableName = "Pre-season" + seasonId;
    //                        dbName = "Historical_Pre_Team.db";
    //                    }
    //                }
    //            }
    //        }

    //        using (var database = new LiteDatabase(dbName))
    //        {
    //            var teamCollection = database.GetCollection<Team>(tableName);
    //            var teamStats = new Team()
    //            {
    //                Id = Id,
    //                Rank = rank,
    //                SeasonId = seasonId,
    //                SeasonTypeId = seasonType,
    //                System = system,
    //                TeamName = teamName,
    //                GamesPlayed = gamesPlayed,
    //                GamesWon = gamesWon,
    //                GamesDrawn = gamesDrawn,
    //                GamesLost = gamesLost,
    //                Points = Points,
    //                Streak = Streak,
    //                GoalsFor = goalsFor,
    //                GoalsAgainst = goalsAgainst,
    //                CleanSheets = cleanSheets,
    //                LastTenGames = lastTenGames,
    //                HomeRecord = homeRecord,
    //                AwayRecord = awayRecord,
    //                OneGoalGames = oneGoalGames,
    //                TeamIconUrl = teamIcon,
    //                TeamURL = teamUrl
    //            };
    //            try
    //            {
    //                if (teamCollection.FindById(Id) != null)
    //                {
    //                    teamCollection.Update(teamStats);
    //                    return true;
    //                }
    //                else
    //                {
    //                    teamCollection.Insert(teamStats);
    //                    return true;
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Log.Logger.Error(ex,"Error saving team stats.");
    //            }
    //        }
    //        return false;
    //    }
    //}
}