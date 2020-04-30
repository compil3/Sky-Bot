using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using FluentScheduler;
using Serilog;
using ShellProgressBar;
using Sky_Bot.Engines;
using Sky_Bot.Essentials;
using Sky_Bot.Extras.Spinner;

namespace Sky_Bot.Schedule
{
    
    class Weekly : Registry
    {
        public Weekly(IMessageChannel channel)
        {
            Action update = new Action(async () =>
            {
                Log.Logger.Warning("Running Weekly Update.");

         

                await channel.SendMessageAsync("Starting Weekly Updates.");
                //var chnl = _client.GetGuild(689119429375819951).GetTextChannel(705197391984197683) as ISocketMessageChannel;
                //await chnl.SendMessageAsync("Starting Weekly Stats Update for PSN and XBOX leagues").ConfigureAwait(false);

                
                var savePrevious = "";
                var seasonCount = new List<ConsoleInformation>();
                var numberOf = new List<SeasonDiff>();
                var system = new string[] {"xbox", "psn"};
                var previousSeason = new List<PreviousSeason>();
                var currentSeason = new List<CurrentSeason>();

                foreach (var season in system)
                {
                    seasonCount.Add(new ConsoleInformation
                    {
                        System = season,
                        CurrentSeason = int.Parse(GetCurrentSeason.GetSeason(season)),
                        PreviousSeasons = int.Parse(GetPreviousSeason.GetPrevious(season)),
                        NumberOfSeason = Math.Abs(int.Parse(GetCurrentSeason.GetSeason(season)) - int.Parse(GetPreviousSeason.GetPrevious(season)) + 1)
                    });
                }
                //foreach (var output in seasonCount)
                //{
                //    Console.WriteLine($"System: {output.System}\n Current Season: {output.CurrentSeason}\n Previous Season:{output.PreviousSeasons}\n Season Count: {output.NumberOfSeason}");
                //}


                
                var options = new ProgressBarOptions()
                {
                    ForegroundColor = ConsoleColor.Yellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    BackgroundCharacter = '\u2593'
                };
                foreach (var t in seasonCount)
                {
                    try
                    {
                        for (int j = t.PreviousSeasons; j < t.NumberOfSeason; j++)
                        {
                             Player.GetPlayer(t.System, "LG", "playerstats", j, "reg", "uh");
                            //using (var pbar = new ProgressBar(j,"Update in progress.",options))
                            //{
                            //    pbar.Tick();
                            //    pbar.Tick($"Last updated: {}");
                            //}

                        }
                        Log.Logger.Information($"{t} Updated.");
                    }


                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }

                await channel.SendMessageAsync("Weekly Update Completed.").ConfigureAwait(false);
            });
            this.Schedule(update).ToRunNow().AndEvery(5).Days();
        }

    }

    internal class PreviousSeason
    {
        public int Previous { get; set; }
        public string System { get; set; }
    }

    internal class CurrentSeason
    {
        public int Current { get; set; }
        public string System { get; set; }
    }

    internal class ConsoleInformation
    {
        public string System { get; set; }
        public int PreviousSeasons { get; set; }
        public int CurrentSeason { get; set; }
        public int NumberOfSeason { get; set; }

    }

    internal class SeasonDiff
    {
    }
}
