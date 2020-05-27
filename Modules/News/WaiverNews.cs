using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using FluentScheduler;
using HtmlAgilityPack;
using Quartz;
using Serilog;

namespace LGFA.Modules.News
{
    public class WaiverNews : Registry
    {
        public WaiverNews(IMessageChannel channel)
        {
            Action waivers = new Action(async () =>
            {

                var web = new HtmlWeb();
                List<string> leagueList = new List<string>() { "53", "73" };

                foreach (var id in leagueList)
                {
                    var feed = web.Load(
                        $"https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_news&leagueid=" +id +"&typeid=9");

                    await Feed.RunWaivers(feed, id, channel);
                    Log.Logger.Information("Waivers ran.");
                }
            });
            this.Schedule(waivers).ToRunEvery(5).Seconds();
        }
    }
}
