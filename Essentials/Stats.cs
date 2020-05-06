using System;
using System.Collections.Generic;
using System.Text;
using LiteDB;
using Sky_Bot.Properties;

namespace Sky_Bot.Modules.Essentials
{
    class Stats
    {
        public static string PlayerStats(string playerLookup)
        {
            using (var playerDatabase = new LiteDatabase(@"LGFA.db"))
            {
                var player = playerDatabase.GetCollection<PlayerProperties.StatProperties>("Players");
                var result = player.Find(x =>
                    x.PlayerName.StartsWith(playerLookup) || x.PlayerName.ToLower().StartsWith(playerLookup));

                //find player, go to their link, scrape the data, post to channel.  no need for database.
                foreach (var found in result)
                {
                    return found.PlayerUrl;
                }
            }
            
            return null;
        }
    }
}
