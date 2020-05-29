using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Discord;
using FluentScheduler;
using LGFA.Database;
using LGFA.Essentials;
using Serilog;
using ShellProgressBar;

namespace LGFA.Schedule
{
    public class WeekUpdate : Registry
    {
        public WeekUpdate(IMessageChannel chnl)
        {
            var update = new Action(async () =>
            {
                var system = new[] { "xbox", "psn" };

                var seasonCount = system.Select(s => new ConsoleInformation
                {
                    System = s,
                    CurrentSeason = int.Parse(GetCurrentSeason.GetSeason(s)),
                    PreviousSeason = int.Parse(GetPreviousSeason.GetPrevious(s)),
                    NumberOfSeasons = Math.Abs(int.Parse(GetCurrentSeason.GetSeason(s)) -
                        int.Parse(GetPreviousSeason.GetPrevious(s)) + 1)
                }).ToList();

                var options = new ProgressBarOptions
                {
                    ForegroundColorDone = ConsoleColor.DarkBlue,
                    ForegroundColor = ConsoleColor.Yellow,
                    BackgroundColor = ConsoleColor.Gray,
                    BackgroundCharacter = '\u2593',
                    ShowEstimatedDuration = true,
                    DisplayTimeInRealTime = false,
                    CollapseWhenFinished = false
                };
                var childOptions = new ProgressBarOptions
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
                    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        foreach (var t in seasonCount)
                        {
                            await chnl.SendMessageAsync("Starting update.").ConfigureAwait(false);
                            for (var j = t.PreviousSeason; j < t.NumberOfSeasons; j++)
                            {
                                Get.GetPlayerIds(t.System, "player", j);
                                await chnl.SendMessageAsync(
                                    $"Ran Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{t.NumberOfSeasons}");
                            }
                        }
                    }
                    else
                    {

                        foreach (var t in seasonCount)
                        {
                            await chnl.SendMessageAsync("Starting update.").ConfigureAwait(false);
                            using var pbar = new ProgressBar(t.NumberOfSeasons,
                                $"Running Database Update {t.System}", options);


                            for (var j = t.PreviousSeason; j < t.NumberOfSeasons; j++)
                            {
                                pbar.EstimatedDuration = TimeSpan.FromMilliseconds(t.NumberOfSeasons * 5000);

                                pbar.Tick(
                                    $"Running Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{t.NumberOfSeasons}");
                                Get.GetPlayerIds(t.System, "player", j, pbar);
                                await chnl.SendMessageAsync(
                                    $"Ran Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{t.NumberOfSeasons}");

                                Thread.Sleep(250);
                            }

                            var estimatedDuration = TimeSpan.FromSeconds(60 * seasonCount.Count) +
                                                    TimeSpan.FromSeconds(30 * t.NumberOfSeasons);
                            pbar.Tick(estimatedDuration, $"Completed {t.System} updated");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });
            Schedule(update).ToRunNow().AndEvery(1).Weeks().On(DayOfWeek.Friday).At(3, 0);
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