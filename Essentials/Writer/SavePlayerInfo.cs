using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using LiteDB;
using Sky_Bot.Properties;

namespace Sky_Bot.Essentials.Writer
{
    class SavePlayerInfo
    {
        public static bool SavePlayerUrl(int id, string playerName, string playerUrl)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(0);
            var currentMethod = sf.GetMethod();
            using (var database = new LiteDatabase(@"LGFA.db"))
            {
                var playerCollection = database.GetCollection<PlayerProperties.PlayerInfo>("URLS");
                playerCollection.EnsureIndex(x => x.Id);
                var playerInfo = new PlayerProperties.PlayerInfo
                {
                    Id = id,
                    playerName = playerName,
                    playerUrl = playerUrl
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
                    Console.WriteLine($"Error processing Player Information {currentMethod} {e}");
                    return false;
                }
            }
        }

        public static bool SaveCareer(int id, string playerName, string careerRecord, int gamesPlayed, double amr,
            double goals, string assists, string SOT, double shotAttempts, double shotPercentage, double passesC, double passesA,double passPercentage,
            string keypass, string interceptions, double tackles, double tackleAttempts, double tacklePercentage, string blocks, string red, string yellow)
        {
            var st = new StackTrace();
            var sf = st.GetFrame(0);
            var currentMethod = sf.GetMethod();

            using (var database = new LiteDatabase(@"LGFA.db"))
            {
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
                    Console.WriteLine($"Error saving Player Career Information {currentMethod} {e}");
                    return false;
                }
            }
        }
    }
}
