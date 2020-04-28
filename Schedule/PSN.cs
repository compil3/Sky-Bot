using System;
using System.Collections.Generic;
using System.Text;
using FluentScheduler;
using Serilog;
using Sky_Bot.Engines;
using Sky_Bot.Essentials;
using Sky_Bot.Extras.Spinner;

namespace Sky_Bot.Schedule
{
    public class PSN : Registry
    {
        public PSN()
        {
            Action updates = new Action(() =>
            {
                var previousSeason = "";
                var currentSeason = "";
                var savePrevious = "";

                try
                {
                    previousSeason = GetPreviousSeason.GetPrevious("psn");
                    currentSeason = GetCurrentSeason.GetSeason("psn");
                    savePrevious = "";
                    Log.Warning("Updating PSN Historical Statistics");

                    for (int i = int.Parse(previousSeason); i < int.Parse(currentSeason); i++)
                    {
                        Log.Information("Starting database update.");
                        Console.WriteLine();
                        Spinner.Start();
                        Player.GetPlayer("psn", "LG","playerstats", i, "reg", "uh");
                        Spinner.Stop();
                        

                        //Player.GetPlayer("psn", "LG","playerstats", i, "reg", "uh");
                        //Log.Warning($"PSN Players regular season statistics updated for Season: {i}.");

                        //Goalie.GetGoalie("psn", "goaliestats", i, "reg", "uh");
                        //Log.Warning($"PSN Goalie regular season statistics updated for Season: {i}.");

                        //Team.GetTeam("psn","teamstats",i,"reg","uh");
                        //Log.Warning($"PSN Team regular season statistics updated for Season: {i}.");
                        savePrevious = i.ToString();
                    }
                    Log.Fatal(!SavePreviousSeason.SavePrevious("psn", savePrevious) ? "Failed to updated PSN Previous Season setting." : "Successfully updated PSN Previous Season setting.");
                }
                catch (Exception e)
                {
                    Log.Error($"Fatal error running historical stats update.\n{e}");
                }
            });
            this.Schedule(updates).ToRunNow().AndEvery(1).Months().OnTheSecond(DayOfWeek.Friday);
        }
    }
}
