using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Discord;
using FluentScheduler;
using Serilog;
using ShellProgressBar;
using Sky_Bot.Properties;


namespace Engine.Schedule
{
    class WeekUpdate : Registry
    {
        public WeekUpdate(IMessageChannel channel)
        {
            Action update = new Action(async () =>
            {
                var seasonCount = new List<ConsoleInformation>();
                var system = new string[] { "xbox", "psn" };

                foreach (var s in system)
                {
                    seasonCount.Add(new ConsoleInformation
                    {
                        System = s,
                        CurrentSeason = int.Parse(GetCurrentSeason.GetSeason(s)),
                        PreviousSeason = int.Parse(GetPreviousSeason.GetPrevious(s)),
                        NumberOfSeasons = Math.Abs(int.Parse(GetCurrentSeason.GetSeason(s)) - int.Parse(GetPreviousSeason.GetPrevious(s)) + 1)
                    });
                }

                var options = new ProgressBarOptions()
                {
                    ForegroundColorDone = ConsoleColor.DarkBlue,
                    ForegroundColor = ConsoleColor.Yellow,
                    BackgroundColor = ConsoleColor.Gray,
                    BackgroundCharacter = '\u2593',
                    ShowEstimatedDuration = true,
                    DisplayTimeInRealTime = true
                };

                Log.Logger.Warning("Running Weekly Update.");
                await channel.SendMessageAsync("Starting weekly updates.").ConfigureAwait(false);

                try
                {
                    foreach (var t in seasonCount)
                    {
                        using (var pbar = new ProgressBar(t.NumberOfSeasons,
                            $"Running Database Update {t.System.ToString()}", options))
                        {
                            for (int j = t.PreviousSeason; j < t.NumberOfSeasons; j++)
                            {
                                pbar.Tick(
                                    $"Running Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{t.NumberOfSeasons}");
                                Player.GetInformation(t.System, "player", j, "regular", "career", pbar);
                                Thread.Sleep(500);

                                var estimatedDuration = TimeSpan.FromMinutes(500 * seasonCount.Count) +
                                                        TimeSpan.FromMilliseconds(300 * j);
                                pbar.Tick(estimatedDuration, $"Completed {t.System} updated");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    await channel.SendMessageAsync($"Error with **WeekUpdate** schedule.\n Error: {e}");
                    Console.WriteLine(e);
                    throw;
                }

                await channel.SendMessageAsync("Week update completed").ConfigureAwait(false);
            });
            this.Schedule(update).ToRunNow().AndEvery(5).Days();

        }
    }

    internal class ConsoleInformation
    {
        public string System { get; set; }
        public int PreviousSeason { get; set; }
        public int CurrentSeason { get; set; }
        public int NumberOfSeasons { get; set; }
    }
}
