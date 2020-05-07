using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Engine.Essentials.Generators;
using LiteDB;
using Engine.Properties;
using Serilog;

namespace Engine.Essentials.Write
{
    public class PlayWriter
    {
        public static bool SaveUrl(int id, string playerName, string playerUri)
        {
           using var database = new LiteDatabase(@"LGFA.db");
            var playerCollection = database.GetCollection<PlayerProperties.URL>("URLS");
            playerCollection.EnsureIndex(x => x.Id);

            var playerInfo = new PlayerProperties.URL
            {
                Id = id,
                PlayerName = playerName,
                PlayerUrl = playerUri
            };

            try
            {
                var playerFound = playerCollection.FindById(id);
                if (playerFound != null)
                {
                    playerCollection.Update(playerInfo);
                    return true;
                }
                else
                {
                    playerCollection.Insert(playerInfo);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e,$"Error saving player information.");
                return false;
            }
        }

        public static bool SaveCareer(int id, string playerName, string careerRecord, double gamesPlayed, double amr,
            double goals, string assists, string SOT, double shotAttempts, double shotPercentage, double passesC,
            double passesA, double passPercentage,
            string keypass, string interceptions, double tackles, double tackleAttempts, double tacklePercentage,
            string blocks, string red, string yellow)
        {
            using var database = new LiteDatabase("LGFA.db");
            var playerCareer = database.GetCollection<PlayerProperties.Career>("Career");
            playerCareer.EnsureIndex(x => x.Id);

            var playerStats = new PlayerProperties.Career
            {
                Id = id,
                PlayerName = playerName,
                Record = careerRecord,
                GamesPlayed = gamesPlayed,
                AvgMatchRating = amr,
                Goals = goals,
                Assists = assists,
                ShotAttempts = shotAttempts,
                ShotsOnTarget = SOT,
                ShotPercentage = shotPercentage,
                PassesCompleted = passesC,
                PassesAttempted = passesA,
                PassingPercentage = passPercentage,
                KeyPasses = keypass,
                Interceptions = interceptions, 
                Tackles = tackles,
                TackleAttempts = tackleAttempts,
                TacklePercentage = tacklePercentage,
                Blocks = blocks,
                RedCards = red,
                YellowCards = yellow
            };
            try
            {
                var playerFound = playerCareer.FindById(id);
                if (playerFound != null)
                {
                    playerCareer.Update(playerStats);
                    return true;
                }
                else
                {
                    playerCareer.Insert(playerStats);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e,$"Error saving player career information");
                return false;
            }
        }

        internal static bool SaveInformation(int playerId, string playerName, string playerUrl,string system)
        {
            var databaseName = @"LGFA.db";
            var rootDir = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\.."))  + @"\Database";
            var db = Path.Combine(rootDir, databaseName);

            using var database = new LiteDatabase(db);
            var playerCollection = database.GetCollection<PlayerProperties.URL>("Players");
            playerCollection.EnsureIndex(x => x.Id);

            var playerInfo = new PlayerProperties.URL()
            {
                Id = playerId,
                System = system,
                PlayerName = playerName,
                PlayerUrl = playerUrl
            };

            try
            {
                var playerFound = playerCollection.FindById(playerId);
                if (playerFound != null)
                {
                    playerCollection.Update(playerInfo);
                    return true;
                }
                else
                {
                    playerCollection.Insert(playerInfo);
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e,$"Error processing player information");
                return false;
            }
        }

        public static bool SavePlayer(int ID, int SeasonId, string SeasonTypeId, string Position, string PlayerName,
            string GamesPlayed, string Record, string AvgMatchRating, string Goals, string Assists, string CleanSheets,
            string ShotsOnGoal,
            string ShotsOnTarget, string ShotPercentage, string Tackles, string TackleAttempts, string TacklePercentage,
            string PassPercentage, string KeyPasses, string Interceptions, string Blocks,
            string YellowCards, string RedCards, string ManOfTheMatch, string PlayerURL, string System,
            string TeamIconURL, string Command, string position)
        {
            var dbname = "";
            var tableName = "";

            if (Command == "schedule")
            {
                if (System == "xbox" || System == "psn")
                {
                    var currentSeasonId = int.Parse(Helpers.GetCurrentSeason.GetSeason(System));
                    if (currentSeasonId == SeasonId)
                    {
                        if (SeasonTypeId == "regular") tableName = "Current";
                    }
                }
            }

            return false;
        }
    }
}
