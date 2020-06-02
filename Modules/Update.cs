using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Database;
using LGFA.Essentials;
using LGFA.Extensions;
using LGFA.Schedule;
using Serilog;
using ShellProgressBar;

namespace LGFA.Modules
{
    [RequireUserPermission(GuildPermission.ViewAuditLog)]
    public class Update :ModuleBase
    {
        [Command("update")]
        [Summary("Forces a database update.")]
        public async Task Renew()
        {
            var delOptions = new RequestOptions { Timeout = 2 };
            await Context.Message.DeleteAsync(delOptions);

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
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        foreach (var t in seasonCount)
                        {
                            var currentSeason = LeagueInfo.GetSeason(t.System);

                            await Context.Channel.SendMessageAsync($"Starting update for {t.System.ToUpper()}.").ConfigureAwait(false);
                            foreach (var leagueProp in currentSeason)
                            {
                                for (var j = int.Parse(leagueProp.Season); j >= 1; j--)
                                {
                                    Get.GetPlayerIds(t.System, "player", j);
                                    //await chnl.SendMessageAsync(
                                    //    $"Ran Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{t.NumberOfSeasons}");
                                }
                                await Context.Channel.SendMessageAsync($"Update for {t.System.ToUpper()} completed.");
                                break;
                            }
                        }
                    }
                    else
                    {

                        foreach (var t in seasonCount)
                        {
                            await Context.Channel.SendMessageAsync($"Starting update for {t.System.ToUpper()}.").ConfigureAwait(false);

                            using var pbar = new ProgressBar(t.NumberOfSeasons, $"Running Database Update {t.System}", options);
                            var currentSeason = LeagueInfo.GetSeason(t.System);

                            foreach (var leagueProp in currentSeason)
                            {
                                for (var j = int.Parse(leagueProp.Season); j >= 1; j--)
                                {
                                    pbar.EstimatedDuration = TimeSpan.FromMilliseconds(t.NumberOfSeasons * 5000);

                                    pbar.Tick(
                                        $"Running Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{leagueProp.Season}");
                                    Get.GetPlayerIds(t.System, "player", j, pbar);
                                    //await chnl.SendMessageAsync(
                                    //    $"Ran Season {j} Update for {t.System.ToUpper()}.  Remaining: {j}/{t.NumberOfSeasons}");
                                    Thread.Sleep(250);
                                }
                                await Context.Channel.SendMessageAsync($"Update for {t.System.ToUpper()} completed.");

                                var estimatedDuration = TimeSpan.FromSeconds(60 * seasonCount.Count) +
                                                        TimeSpan.FromSeconds(30 * t.NumberOfSeasons);
                                pbar.Tick(estimatedDuration, $"Completed {t.System} updated");
                                break;
                            }
                            
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
        }
    }
}
