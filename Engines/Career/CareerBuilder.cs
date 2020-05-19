using Discord;
using HtmlAgilityPack;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using LGFA.Engines.Career;
using LGFA.Properties;
using LiteDB;

namespace LGFA.Engines
{
    public class CareerBuilder
    {
        public static (List<CareerProperties>, string playerUrl, string playerFound) GetCareerNoSeason(
            string lookUpPlayer, string seasonId)
        {
            Embed message = null;
            var leagueId = 0;
            var web = new HtmlWeb();
            var playerLink = "";
            var playerName = "";
            try
            {
                var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var dbFolder = "Database/";
                var dbDir = Path.Combine(dbPath, dbFolder);
                using var playerDatabase = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
                var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");
                var div = 1;
                player.EnsureIndex(x => x.playerName);

                var result = player.Query()
                    .Where(x => x.playerName.Contains(lookUpPlayer))
                    .ToList();



                foreach (var found in result)
                {
                    if (found.System == "psn") leagueId = 73;
                    else if (found.System == "xbox") leagueId = 53;
                    try
                    {
                        var playerDoc = web.Load(found.playerUrl);
                        playerLink = found.playerUrl;
                        playerName = found.playerName;
                        var (table, playerUrl, playerFound) = Official.OfficialParse(playerDoc, found.playerUrl,
                            found.playerName, leagueId);
                        return (table, playerUrl, playerFound);

                    }
                    catch (Exception e)
                    {
                        Log.Logger.Warning($"{found.playerName} in GetCareerNoSeason");
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                return (null, null, null);
            }
            return (null, null, null);
        }

        public static (List<CareerProperties>, string playerUrl, string playerFound, string seasonNumber)
            GetCareerSeason(string lookupPlayer, string seasonId)
        {
            Embed message = null;
            var leagueId = 0;
            var web = new HtmlWeb();
            HtmlDocument playerDoc;
            try
            {
                var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                var dbFolder = "Database/";
                var dbDir = Path.Combine(dbPath, dbFolder);
                using var playerDatabase = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
                var player = playerDatabase.GetCollection<PlayerProperties.PlayerInfo>("Players");
                var div = 1;
                player.EnsureIndex(x => x.playerName);

                var result = player.Query()
                    .Where(x => x.playerName.Contains(lookupPlayer))
                    .ToList();

                
                foreach (var found in result)
                {
                    if (found.System == "psn") leagueId = 73;
                    else if (found.System == "xbox") leagueId = 53;
                    try
                    {
                        playerDoc = web.Load(found.playerUrl);

                        var (table, playerUrl, playerFound, season) = Career.Season.SeasonParse(playerDoc,
                            found.playerUrl, found.playerName, seasonId, leagueId);
                        return (table, playerUrl, playerFound, season);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return (null, null, null, null);
        }
    }
}

