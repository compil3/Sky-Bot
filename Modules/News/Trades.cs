using System;
using System.Collections.Generic;
using System.Net;
using Discord;
using FluentScheduler;
using HtmlAgilityPack;
using Serilog;

namespace LGFA.Modules.News
{
    public class Trades : Registry
    {
        public Trades(IMessageChannel chnl, IMessageChannel log)
        {
            async void Action()
            {
                var web = new HtmlWeb();
                var leagueList = new List<string> {"53", "73"};

                foreach (var id in leagueList)
                {
                    try
                    {
                        var feed = web.Load("https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_news&leagueid=" + id + "&typeid=7");
                        await Feed.RunTrades(feed, id, chnl);
                    }
                    catch (HtmlWebException ex)
                    {
                        Log.Logger.Warning($"Trade schedule failed: {ex}");
                        await log.SendMessageAsync($"Trade schedule failed {ex}").ConfigureAwait(false);
                        throw;
                    }

                    //Log.Logger.Information("Trades ran.");
                }
            }

            Schedule((Action) Action).ToRunEvery(5).Minutes();
        }
    }
}