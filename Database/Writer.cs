﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LiteDB;
using Serilog;
using Sky_Bot.Properties;

namespace Sky_Bot.Database
{
    public class Writer
    {
        internal static bool SaveInformation(int playerId, string playerName, string playerUrl,string system)
        {
            using var database = new LiteDatabase(@"/Database");
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