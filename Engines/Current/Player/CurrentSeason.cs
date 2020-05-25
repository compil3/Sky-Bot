using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HtmlAgilityPack;
using LGFA.Extensions;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace LGFA.Engines.Current.Player
{
    class CurrentSeason
    {
        public static List<PlayerProperties> SeasonStats(string playerLookup)
        {
            var web = new HtmlWeb();
            var season = "";
            var seasonNumber = LeagueInfo.GetSeason();
            var leagueId = "";
            var systemIcon = "";
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
            var div = 0;
            player.EnsureIndex(x => x.playerName);

            var result = player.Query()
                .Where(x => x.playerName.Contains(playerLookup))
                .ToList();

            foreach (var found in result)
            {
                var playerDoc = web.Load(found.playerUrl);
                if (found.System == "psn") leagueId = "73";
                else if (found.System == "xbox") leagueId = "53";

                if (found.System == "psn") systemIcon =
                        "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                else if (found.System == "xbox") systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";

                try
                {
                    var findCareerNode = playerDoc.DocumentNode.SelectNodes(
                        $"//*[@id='lg_team_user_leagues-{leagueId}']/div[3]/table/tbody/tr");

                    div = 0;
                    if (findCareerNode == null)
                    {
                        findCareerNode = playerDoc.DocumentNode.SelectNodes(
                            $"//*[@id='lg_team_user_leagues-{leagueId}']/div[4]/table/tbody/tr");
                        div = 4;
                    }
                    else div = 3;

                }
                catch (Exception e)
                {
                    Log.Logger.Error($"Error processing stats. {e}");
                    return null;
                }

            }
            

            return null;
        }
    }
}
