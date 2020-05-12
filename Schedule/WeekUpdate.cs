using System;
using System.Linq;
using System.Threading;
using Discord;
using FluentScheduler;
using Serilog;
using ShellProgressBar;
using Sky_Bot.Database;
using Sky_Bot.Essentials;

namespace Sky_Bot.Schedule
{
    public class WeekUpdate : Registry
    {
        public WeekUpdate(IMessageChannel chnl)
        {
            Action update = new Action(async () =>
            {
                var system = new string[] {"xbox", "psn"};

                var seasonCount = system.Select(s => new ConsoleInformation
                {
                    System = s,
                    CurrentSeason = int.Parse(GetCurrentSeason.GetSeason(s)),
                    PreviousSeason = int.Parse(GetPreviousSeason.GetPrevious(s)),
                    NumberOfSeasons = Math.Abs(int.Parse(GetCurrentSeason.GetSeason(s)) -
                        int.Parse(GetPreviousSeason.GetPrevious(s)) + 1)
                }).ToList();

                var options = new ProgressBarOptions()
                {
                    ForegroundColorDone = ConsoleColor.DarkBlue,
                    ForegroundColor = ConsoleColor.Yellow,
                    BackgroundColor = ConsoleColor.Gray,
                    BackgroundCharacter = '\u2593',
                    ShowEstimatedDuration = true,
                    DisplayTimeInRealTime = false,
                    CollapseWhenFinished = false
                };
                var childOptions = new ProgressBarOptions()
                {
                    ForegroundColor = ConsoleColor.Green,
                    BackgroundColor = ConsoleColor.DarkGreen,
                    ProgressCharacter = '\u2593',
                    CollapseWhenFinished = false,
                    DisplayTimeInRealTime = false
                };

                Log.Logger.Warning("Running Weekly Update.");

                try
                {
                    foreach (var t in seasonCount)
                    {
                        await chnl.SendMessageAsync("Starting update.").ConfigureAwait(false);
                        using var pbar = new ProgressBar(t.NumberOfSeasons,
                            $"Running Database Update {t.System.ToString()}", options);


                        for (var j = t.PreviousSeason; j < t.NumberOfSeasons; j++)
                        {
                            pbar.EstimatedDuration = TimeSpan.FromMilliseconds(t.NumberOfSeasons * 5000);

                            pbar.Tick(
                                $"Running Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{t.NumberOfSeasons}");
                            Get.GetPlayerIds(t.System, "player", j, pbar);
                            Thread.Sleep(250);
                        }

                        var estimatedDuration = TimeSpan.FromSeconds(60 * seasonCount.Count) +
                                                TimeSpan.FromSeconds(30 * t.NumberOfSeasons);
                        pbar.Tick(estimatedDuration, $"Completed {t.System} updated");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
            this.Schedule(update).ToRunNow().AndEvery(1).Weeks().On(DayOfWeek.Sunday).At(1, 0);
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