using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HtmlAgilityPack;
using LGFA.Extensions;
using LGFA.Properties;
using LiteDB;

namespace LGFA.Engines.Current.Player
{
    class CurrentSeason
    {
        public static List<PlayerProperties> SeasonStats(string playerLookup)
        {
            var web = new HtmlWeb();
            var doc = web.Load()
            var season = "";
            var seasonNumber = LeagueInfo.GetSeason();

            foreach (var leagueProp in seasonNumber)
            {
                if (!leagueProp.Season.Contains("S")) season = "S" + leagueProp.Season;
                break;
            }
            var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(dbPath, dbFolder);
            using var playerDatabase = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");
            var div = 1;
            player.EnsureIndex(x => x.playerName);

            var result = player.Query()
                .Where(x => x.playerName.Contains(playerLookup))
                .ToList();
            foreach (var found in result)
            {
                try
                {
                    var findCareerNode = playerDoc.DocumentNode.SelectNodes(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table/tbody/tr");

                    var div = 0;
                    if (findCareerNode == null)
                    {
                        findCareerNode = playerDoc.DocumentNode.SelectNodes(
                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr");
                        div = 4;
                    }
                    else div = 3;

                }

            }
            

            return null;
        }
    }
}
