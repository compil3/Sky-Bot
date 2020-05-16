using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace LGFA.Database
{
    public class Writer
    {
        internal static bool SaveInformation(int playerId, string playerName, string playerUrl,string system)
        {
            using var database = new LiteDatabase(@"Filename=Database/LGFA.db;connect=shared");
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
    }
}
