using System;
using System.Collections.Generic;
using Discord;
using FluentScheduler;
using HtmlAgilityPack;

namespace LGFA.Modules.News
{
    public class WaiverNews : Registry
    {
        public WaiverNews(IMessageChannel channel)
        {
            Action waivers = async () =>
            {
                var web = new HtmlWeb();
                var leagueList = new List<string> {"53", "73"};

                foreach (var id in leagueList)
                {
                    var feed = web.Load(
                        "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_news&leagueid=" +
                        id + "&typeid=9");

                    await Feed.RunWaivers(feed, id, channel);
                    //Log.Logger.Information("Waivers ran.");
                }
            };
            Schedule(waivers).ToRunEvery(2).Seconds();
        }
    }
}