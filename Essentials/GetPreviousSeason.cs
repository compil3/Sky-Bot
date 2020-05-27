using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LGFA.Properties;
using Newtonsoft.Json;
using Serilog;

namespace LGFA.Essentials
{
    public class GetPreviousSeason
    {
        public static string GetPrevious(string system)
        {
            var configLocation = "";

            if (system == "psn")
            {
                configLocation = @"Configuration/Season/psn.json";
            }
            else if (system == "xbox")
            {
                configLocation = @"Configuration/Season/xbox.json";
            }

            string JSON = "";
            //var previousSeason = "";

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

            Season seasonID = JsonConvert.DeserializeObject<Season>(JSON);
            //WebSettings.currentSeason = seasonID.currentSeason;
            return seasonID.previousSeason;

        }
    }
}
