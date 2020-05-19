﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LGFA.Properties;
using Newtonsoft.Json;
using Serilog;

namespace LGFA.Database
{
    class Fetch
    {
        internal static string GetUrl(string system, string trigger, int seasonId, int seasonType)
        {
            var configLocation = "";

            if (system == "psn")
            {
                configLocation = @"Configuration/Url/psn.json";
            }
            else if (system == "xbox")
            {
                configLocation = @"Configuration/Url/xbox.json";
            }

            string JSON = "";

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

            General.UrlSettings settings = JsonConvert.DeserializeObject<General.UrlSettings>(JSON);
            var sTypeTemp = "&seasontypeid=";
            var seasonTypeID = string.Concat(string.Empty, sTypeTemp, seasonType);


            switch (system)
            {
                case "xbox":
                    if (trigger == "player" || trigger == "goaliestats")
                    {
                        Web.XboxPlayerStatsURL = settings.XboxPlayerStatsUrl;
                        return Web.XboxPlayerStatsURL + seasonId + seasonTypeID;
                    } else if (trigger == "draftlist") return Web.XboxDraftListURL + seasonId;
                    else if (trigger == "teamstats") return Web.XboxTeamStandingsURL + seasonId + seasonTypeID;

                    break;
                case "psn":
                    if (trigger == "player" || trigger == "goalistats")
                    {
                        Web.PSNPlayerStatsURL = settings.PsnPlayerStatsUrl;
                        return Web.PSNPlayerStatsURL + seasonId + seasonTypeID;
                    } else if (trigger == "draftlist") return Web.PSNDraftListURL + seasonId;
                    else if (trigger == "teamstats") return Web.PSNTeamStandingsURL + seasonId + seasonTypeID;

                    break;
            }

            return null;
        }
    }
}
