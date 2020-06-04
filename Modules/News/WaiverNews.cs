using System;
using System.Collections.Generic;
using Discord;
using FluentScheduler;
using HtmlAgilityPack;
using Serilog;

namespace LGFA.Modules.News
{
    public class WaiverNews : Registry
    {
        public WaiverNews(IMessageChannel channel, IMessageChannel log)
        {
            Action waivers = async () =>
            {
                var web = new HtmlWeb();
                var leagueList = new List<string> {"53", "73"};
                foreach (var id in leagueList)
                {
                    try
                    {
                        var feed = web.Load(
                            "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_news&leagueid=" +
                            id + "&typeid=9");

                        await Feed.RunWaivers(feed, id, channel);
                    }
                    catch (HtmlWebException ex)
                    {
                        Log.Logger.Warning($"WaiverNews schedule failed: {ex}");
                        await log.SendMessageAsync($"WaiverNews schedule failed {ex}").ConfigureAwait(false);
                        throw;
                    }
                    
                    //Log.Logger.Information("Waivers ran.");
                }
            };
            Schedule(waivers).ToRunEvery(5).Minutes();
        }
    }
}