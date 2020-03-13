using System;
using System.Collections.Generic;
using System.Text;
using FluentScheduler;
using Serilog;
using Sky_Bot.Engines;
using Sky_Bot.Essentials;

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
                        Player.GetPlayer("psn", "playerstats", i, "reg", "uh");
                        Log.Fatal($"PSN Players regular season statistics updated for Season: {i}.");

                        Goalie.GetGoalie("psn", "goaliestats", i, "reg", "uh");
                        Log.Fatal($"PSN Goalie regular season statistics updated for Season: {i}.");

                    //Team.GetTeam("psn","teamstats",i,"reg","uh");
                    Log.Fatal($"PSN Team regular season statistics updated for Season: {i}.");
                        savePrevious = i.ToString();
                    }
                    Log.Fatal(!GetUrl.SavePrevious("psn", savePrevious) ? "Failed to updated PSN Previous Season setting." : "Successfully updated PSN Previous Season setting.");
                }
                catch (Exception e)
                {
                    Log.Error($"Fatal error running historical stats update.\n{ex}");
                }
            });
            this.Schedule(updates).ToRunNow().AndEvery(1).Months().OnTheSecond(DayOfWeek.Friday);
        }
    }
}
