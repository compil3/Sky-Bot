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

                var currentSeason = "";
                var savePrevious = "";
                var system = new string[] {"xbox", "psn"};

                
                var options = new ProgressBarOptions()
                {
                    ForegroundColor = ConsoleColor.Yellow,
                    ForegroundColorDone = ConsoleColor.DarkGreen,
                    BackgroundColor = ConsoleColor.DarkGray,
                    BackgroundCharacter = '\u2593'
                };
                foreach (var t in system)
                {
                    try
                    {
                        var previousSeason = GetPreviousSeason.GetPrevious(t);
                        currentSeason = GetCurrentSeason.GetSeason(t);
                        for (int j = int.Parse(previousSeason); j < int.Parse(currentSeason); j++)
                        {
                            Player.GetPlayer(t, "LG", "playerstats", j, "reg", "uh");
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
}
