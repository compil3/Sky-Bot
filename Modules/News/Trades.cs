using System;
using System.Collections.Generic;
using System.Net;
using Discord;
using FluentScheduler;
using HtmlAgilityPack;

namespace LGFA.Modules.News
{
    public class Trades : Registry
    {
        public Trades(IMessageChannel chnl)
        {
            Action trades = async () =>
            {
                var web = new HtmlWeb();
                var leagueList = new List<string> {"53", "73"};

                foreach (var id in leagueList)
                {

                    try
                    {
                        var feed = web.Load(
                            "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_news&leagueid=" +
                            id + "&typeid=7");
                        await Feed.RunTrades(feed, id, chnl);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    //Log.Logger.Information("Trades ran.");
                }
            };
            Schedule(trades).ToRunEvery(5).Minutes();
        }
    }
}