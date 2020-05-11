using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Engine.Properties;
using Newtonsoft.Json;
using Serilog;


namespace Engine.Essentials.Helpers
{
    public class RetrieveUrl
    {
        //public static string GetUrl(string System, string trigger, int SeasonId, string SeasonType)
        //{
        //    var configLocation = "";

        //    if (System == "psn")
        //    {
        //        configLocation = @"Configs\Urls\psn.json";
        //    }
        //    else if (System == "xbox")
        //    {
        //        configLocation = @"Configs\Urls\xbox.json";
        //    }

        //    string JSON = "";
        //    var xboxURL = "";
        //    var psnURL = "";

        //    var sFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    var configFile = Path.Combine(sFile, configLocation);
        //    if (!File.Exists(configFile)) Log.Fatal($"{configFile} does not exist");
        //    try
        //    {
        //        using (var stream = new FileStream(configFile, FileMode.Open, FileAccess.Read))
        //        using (var readSettings = new StreamReader(stream))
        //        {
        //            JSON = readSettings.ReadToEnd();
        //        }
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        Log.Fatal(ex, $"Error reading {configLocation}");
        //        throw;
        //    }

        //    UrlSettings settings = JsonConvert.DeserializeObject<UrlSettings>(JSON);
        //    if (SeasonType == "pre") SeasonType = "0";
        //    else if (SeasonType == "reg") SeasonType = "1";

        //    var url = "";
        //    var sTypeTemp = "&seasontypeid=";
        //    var seasonTypeID = string.Concat(string.Empty, sTypeTemp, SeasonType);


        //    if (System == "xbox")
        //    {
        //        if (trigger == "player" || trigger == "goaliestats")
        //        {
        //            Web.XboxPlayerStatsURL = settings.xboxPlayerStatsURL;
        //            return Web.XboxPlayerStatsURL + SeasonId + seasonTypeID;
        //        }
        //        else if (trigger == "draftlist")
        //        {
        //            return Web.XboxDraftListURL + SeasonId;
        //        }
        //        else if (trigger == "teamstats")
        //        {
        //            Web.XboxTeamStandingsURL = settings.xboxStandingsURL;
        //            return Web.XboxTeamStandingsURL + SeasonId + seasonTypeID;
        //        }
        //    }
        //    else if (System == "psn")
        //    {
        //        if (trigger == "player" || trigger == "goaliestats")
        //        {
        //            Web.PSNPlayerStatsURL = settings.psnPlayerStatsURL;
        //            return Web.PSNPlayerStatsURL + SeasonId + seasonTypeID;
        //        }
        //        else if (trigger == "draftlist")
        //        {
        //            return Web.PSNDraftListURL + SeasonId;
        //        }
        //        else if (trigger == "teamstats")
        //        {
        //            Web.PSNTeamStandingsURL = settings.psnStandingsURL;
        //            return Web.PSNTeamStandingsURL + SeasonId + seasonTypeID;
        //        }
        //    }
        //    return null;
        //}

        internal static string GetUrlTwo(string system, string trigger, int seasonId, int seasonType)
        {
            var configLocation = "";

            if (system == "psn")
            {
                configLocation = @"Configuration/Urls/psn.json";
            }
            else if (system == "xbox")
            {
                configLocation = @"Configuration/Urls/xbox.json";
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

            PlayerProperties.UrlSettings settings = JsonConvert.DeserializeObject<PlayerProperties.UrlSettings>(JSON);
            var url = "";
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

        //public static string GetNewsUrl()
        //{
        //    var JSON = "";
        //    var sFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    var configLocation = "@Configs\\Urls\news.json";
        //    var configFile = Path.Combine(sFile, configLocation);

        //    if (!File.Exists(configFile)) Log.Error($"{configFile} does not exist.");
        //    try
        //    {
        //        using (var stream = new FileStream(configFile, FileMode.Open, FileAccess.Read))
        //        using (var readSetting = new StreamReader(stream))
        //        {
        //            JSON = readSetting.ReadToEnd();
        //        }
        //    }
        //    catch (UnauthorizedAccessException ex)
        //    {
        //        Log.Logger.Error(ex, $"Error reading websettings.json ({configFile})");
        //        throw;
        //    }

        //    PlayerProperties.UrlSettings settings = JsonConvert.DeserializeObject<PlayerProperties.UrlSettings>(JSON);
        //    var url = "";
        //    Web.NewsUrl = settings.newsUrl;
        //    url = Web.NewsUrl;
        //    return url;
        //}

    }
}
