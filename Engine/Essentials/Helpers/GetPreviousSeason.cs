using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Serilog;
using Engine.Properties;

namespace Engine.Essentials.Helpers
{
    class GetPreviousSeason
    {
        public static string GetPrevious(string system)
        {
            var configLocation = system switch
            {
                "psn" => @"Configuration/Season/psn.json",
                "xbox" => @"Configuration/Season/xbox.json",
                _ => ""
            };

            var JSON = "";
            var sFile = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var configFile = Path.Combine(sFile, configLocation);

            if (!File.Exists(configFile)) Log.Logger.Fatal($"{configFile} does not exist or there is no access.");

            try
            {
                using var stream = new FileStream(configFile, FileMode.Open, FileAccess.Read);
                using var readSettings = new StreamReader(stream);
                JSON = readSettings.ReadToEnd();
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, $"Error reading {configLocation}");
                throw;
            }

            var seasonId = JsonConvert.DeserializeObject<Season>(JSON) ?? throw new ArgumentNullException($"GetPreviousSeason.cs JsonConvert.DeserializeObject<Season>(JSON)");
            return seasonId.PreviousSeason;
        }
    }
}
