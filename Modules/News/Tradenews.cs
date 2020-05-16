using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using HtmlAgilityPack;
using LGFA.Essentials;
using LiteDB;
using LGFA.Properties;
using static LGFA.Modules.News.RunNews;

namespace LGFA.Modules.News
{
    public class NewsWire : ModuleBase<SocketCommandContext>
    {
        public static async Task TradeNewsAsync(IMessageChannel channel, string system)
        {
            var web = new HtmlWeb();
            var url = RetrieveUrl.GetNewsUrl();
            HtmlNodeCollection nodes;
            HtmlDocument feed = null;

            var feedString = "//*[@id='newsfeed_page']/ol/li[1]";

            var xbox = "53" + "&typeid=7";
            var psn = "73" + "&typeid=7";

           if (system == "xbox")
            {
                feed = web.Load(url + xbox);
                nodes = feed.DocumentNode.SelectNodes(feedString);
                await RunTask(nodes, channel, system, "trade");
            }
            else if (system == "psn")
            {
                feed = web.Load(url + psn);
                nodes = feed.DocumentNode.SelectNodes(feedString);
                await RunTask(nodes, channel, system, "trade");
            }
        }

        //private static async Task WaiverNews(IMessageChannel channel, string system)
        //{
        //    var waiverNodes = new HtmlDocument();
        //    HtmlNodeCollection nodes;
        //   // await RunFeed()
        //}


        //private static async Task RunFeed(HtmlNodeCollection nodes, IMessageChannel channel, string system, string newsType)
        //{
        //    var tempDateTime = "";
        //    var teamTwoName = "";
        //    var systemIcon = "";

        //    if (system == "psn") systemIcon = "https://media.playstation.com/is/image/SCEA/navigation_home_ps-logo-us?$Icon$";
        //    else if (system == "xbox") systemIcon = "http://www.logospng.com/images/171/black-xbox-icon-171624.png";

        //    foreach (var item in nodes)
        //    {
        //        tempDateTime = item.SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/div/abbr").InnerText;
        //        var line = item.SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/div/h3").InnerText;

        //        var str = line;
        //        var removeThe = new string[] { "The ", "the " };

        //        foreach (var c in removeThe)
        //        {
        //            str = str.Replace(c, String.Empty);
        //        }

        //        if (newsType == "7")
        //        {
        //            var splitStr = str.Split(new string[] {"to "}, StringSplitOptions.None);
        //            splitStr[1] = splitStr[1].Replace("  ", " ");

        //            var iconTemp = WebUtility.HtmlDecode(item
        //                .SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/a[2]/img").Attributes["src"].Value);

        //            var lastNews = DateTime.Parse(tempDateTime);

        //            if (!SaveNews(lastNews, splitStr[0], splitStr[1], system))
        //            {
        //                break;
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    var table = "";
        //                    table = (system == "xbox") ? "XboxTrade" : "PsnTrade";

        //                    using (var newsDb = new LiteDatabase(@"News.db"))
        //                    {
        //                        var news = newsDb.GetCollection<NewsProperties.News>(table);
        //                        var result = news.Find(x => x.date.Equals(lastNews));
        //                        foreach (var headline in result)
        //                        {
        //                            var builder = new EmbedBuilder()
        //                                .WithColor(new Color(0xFF0019))
        //                                .WithTimestamp(lastNews)
        //                                .WithFooter(footer =>
        //                                {
        //                                    footer
        //                                        .WithText("leaguegaming.com/fifa")
        //                                        .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
        //                                })
        //                                .WithThumbnailUrl(
        //                                    "https://cdn0.iconfinder.com/data/icons/trading-outline/32/trading_outline_2._Location-512.png")
        //                                .WithAuthor(author =>
        //                                {
        //                                    author
        //                                        .WithName("Sky Sports Trade Center")
        //                                        .WithIconUrl(systemIcon);
        //                                })
        //                                .AddField(splitStr[0], ":arrow_right:", false)
        //                                .AddField(splitStr[1], ":white_check_mark:", false);

        //                            var embed = builder.Build();
        //                            await channel.SendMessageAsync(
        //                                    null,
        //                                    embed: embed)
        //                                .ConfigureAwait(false);
        //                        }
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    Console.WriteLine(e);
        //                    throw;
        //                }
        //            }
        //        }
        //        else if (newsType == "9")
        //        {
        //            var lastNews = DateTime.Parse(tempDateTime);
        //        }
        //    }
        //}

        //private static HtmlNodeCollection FeedURl(string system, string newsType)
        //{
        //    var web = new HtmlWeb();
        //    var url = RetrieveUrl.GetNewsUrl();
        //    HtmlDocument feed = null;
        //    HtmlNodeCollection nodes;

        //    var feedString = "//*[@id='newsfeed_page']/ol/li[1]";

        //    var xbox = "53" + "&typeid=" + newsType;
        //    var psn = "73" + "&typeid=" + newsType;

        //    if (system == "xbox")
        //    {
        //        feed = web.Load(url + xbox);
        //    }
        //    else if (system == "psn")
        //    {
        //        feed = web.Load(url + psn);
        //    }
        //    nodes = feed.DocumentNode.SelectNodes(feedString);
        //    return nodes;
        //}

        //private static object GetIcon()
        //{
        //    var sFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        //    var iconFile = @"Images\trade.png";
        //    var iconLocation = Path.Combine(sFile, iconFile);
        //    return iconLocation;
        //}
    }
}
