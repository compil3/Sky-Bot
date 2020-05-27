using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using HtmlAgilityPack;
using LGFA.Modules.News;
using LGFA.Properties;
using LiteDB;
using Quartz;
using Serilog;


namespace LGFA.Modules.News
{

    public class Trades : Registry
    {
        public Trades(IMessageChannel chnl)
        {
            Action trades = new Action(async () =>
            {
                var web = new HtmlWeb();
                new HtmlWeb();
                List<string> leagueList = new List<string>() {"53", "73"};

                foreach (var id in leagueList)
                {
                    var feed = web.Load(
                        $"https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=team_news&leagueid=" + id + "&typeid=7");
                    await News.Feed.RunTrades(feed, id, chnl);
                    Log.Logger.Information("Trades ran.");
                }
            });
            this.Schedule(trades).ToRunEvery(2).Seconds();
        }
    }
}

