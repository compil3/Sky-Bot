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
    class SavePreviousSeason
    {
        public static bool SavePrevious(string system, string previous)
        {
            var configLocation = "";

            if (system == "psn")
            {
                configLocation = @"Configuration\Season\psn.json";
            }
            else if (system == "xbox")
            {
                configLocation = @"Configuration\Season\xbox.json";
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

            Season obj = JsonConvert.DeserializeObject<Season>(JSON);
            try
            {
                obj.previousSeason = previous;
                string content = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(configFile, content);
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error saving previous season to {configLocation}");
            }
            return false;
        }
    }
}
