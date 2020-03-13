﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Serilog;
using Sky_Bot.Properties;

namespace Sky_Bot.Essentials
{
    public class RetrieveUrl
    {
        public static string GetUrl(string System, string Trigger, int SeasonId, string SeasonType)
        {
            var configLocation = "";

            if (System == "psn")
            {
                configLocation = @"Configs\Urls\psn.json";
            }
            else if (System == "xbox")
            {
                configLocation = @"Configs\Urls\xbox.json";
            }

            string JSON = "";
            var xboxURL = "";
            var psnURL = "";

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

            UrlSettings settings = JsonConvert.DeserializeObject<UrlSettings>(JSON);
            if (SeasonType == "pre") SeasonType = "0";
            else if (SeasonType == "reg") SeasonType = "1";

            var url = "";
            var sTypeTemp = "&seasontypeid=";
            var seasonTypeID = string.Concat(string.Empty, sTypeTemp, SeasonType);


            if (System == "xbox")
            {
                if (Trigger == "playerstats" || Trigger == "goaliestats")
                {
                    Web.XboxPlayerStatsURL = settings.xboxPlayerStatsURL;
                    return Web.XboxPlayerStatsURL + SeasonId + seasonTypeID;
                }
                else if (Trigger == "draftlist")
                {
                    return Web.XboxDraftListURL + SeasonId;
                }
                else if (Trigger == "teamstats")
                {
                    Web.XboxTeamStandingsURL = settings.xboxStandingsURL;
                    return Web.XboxTeamStandingsURL + SeasonId + seasonTypeID;
                }
            }
            else if (System == "psn")
            {
                if (Trigger == "playerstats" || Trigger == "goaliestats")
                {
                    Web.PSNPlayerStatsURL = settings.psnPlayerStatsURL;
                    return Web.PSNPlayerStatsURL + SeasonId + seasonTypeID;
                }
                else if (Trigger == "draftlist")
                {
                    return Web.PSNDraftListURL + SeasonId;
                }
                else if (Trigger == "teamstats")
                {
                    Web.PSNTeamStandingsURL = settings.psnStandingsURL;
                    return Web.PSNTeamStandingsURL + SeasonId + seasonTypeID;
                }
            }
            return null;
        }

    }

}

