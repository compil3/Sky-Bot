using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using HtmlAgilityPack;
using Serilog;
using Engine.Properties;
using Newtonsoft.Json;

namespace Engine.Essentials.Helpers
{
    class GetCurrentSeason
    {
        public static string GetSeason(string system)
        {
            var configLocation = system switch
            {
                "psn" => @"Configuration/Season/psn.json",
                "xbox" => @"Configuration/Season/xbox.json",
                _ => ""
            };

            var JSON = "";
            var sFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var configFile = Path.Combine(sFile, configLocation);

            if (!File.Exists(configFile)) Log.Logger.Fatal($"{configFile} does not exist or there is no access.");

            try
            {
                using var stream = new FileStream(configFile, FileMode.Open, FileAccess.Read);
                using var readSetting = new StreamReader(stream);
                JSON = readSetting.ReadToEnd();
            }
            catch (UnauthorizedAccessException e)
            {
                Log.Logger.Fatal(e,$"Error reading {configLocation}");
                throw;
            }

            var seasonId = JsonConvert.DeserializeObject<Season>(JSON) ?? throw new ArgumentNullException($"GetCurrentSeason.cs JsonConvert.DeserializeObject<Season>(JSON)");
            return seasonId.CurrentSeason;
        }
    }
}
