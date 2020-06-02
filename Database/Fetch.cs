using System;
using System.IO;
using System.Reflection;
using LGFA.Extensions;
using LGFA.Properties;
using Newtonsoft.Json;
using Serilog;

namespace LGFA.Database
{
    internal class Fetch
    {
        internal static string GetUrl(string system, string trigger, int seasonId, int seasonType)
        {
            var configLocation = "";

            if (system == "psn")
                configLocation = @"Configuration/Url/psn.json";
            else if (system == "xbox") configLocation = @"Configuration/Url/xbox.json";

            var JSON = "";

            var sFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var configFile = Path.Combine(sFile, configLocation);
            if (!File.Exists(configFile)) Log.Fatal($"{configFile} does not exist");
            try
            {
                using (var stream = new FileStream(configFile, FileMode.Open, FileAccess.Read))
                using (var readSettings = new StreamReader(stream))
                {
                    JSON = readSettings.ReadToEnd();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Fatal(ex, $"Error reading {configLocation}");
                throw;
            }

            var settings = JsonConvert.DeserializeObject<General.UrlSettings>(JSON);
            var sTypeTemp = "&seasontypeid=";
            var seasonTypeID = string.Concat(string.Empty, sTypeTemp, seasonType);
            var leagueInfo = LeagueInfo.GetSeason(system);
            var seasonNumber = "";
            foreach (var lgInfo in leagueInfo)
            {
                switch (system)
                {
                    case "xbox":
                        if (trigger == "player" || trigger == "goaliestats")
                        {
                            Web.XboxPlayerStatsURL = settings.XboxPlayerStatsUrl;
                            if (lgInfo.Games == null || lgInfo.Games == "/0")
                                seasonNumber = (int.Parse(lgInfo.Season) - 1).ToString();
                            else seasonNumber = seasonId.ToString();
                            return Web.XboxPlayerStatsURL + seasonNumber + seasonTypeID;
                        }
                        else if (trigger == "draftlist")
                        {
                            return Web.XboxDraftListURL + seasonId;
                        }
                        else if (trigger == "teamstats")
                        {
                            return Web.XboxTeamStandingsURL + seasonId + seasonTypeID;
                        }

                        break;
                    case "psn":
                        if (trigger == "player" || trigger == "goalistats")
                        {
                            Web.PSNPlayerStatsURL = settings.PsnPlayerStatsUrl;
                            if (lgInfo.Games == null || lgInfo.Games == "/0")
                                seasonNumber = (int.Parse(lgInfo.Season) - 1).ToString();
                            else seasonNumber = seasonId.ToString();
                            return Web.PSNPlayerStatsURL + seasonNumber + seasonTypeID;
                        }
                        else if (trigger == "draftlist")
                        {
                            return Web.PSNDraftListURL + seasonId;
                        }
                        else if (trigger == "teamstats")
                        {
                            return Web.PSNTeamStandingsURL + seasonId + seasonTypeID;
                        }

                        break;
                } 
            }
            

            return null;
        }
    }
}