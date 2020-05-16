using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using HtmlAgilityPack;
using LGFA.Essentials;
using LGFA.Essentials.Writer.News;
using LGFA.Properties;
using LiteDB;
using static LGFA.Properties.NewsProperties;

namespace LGFA.Modules.News
{
    public class RunNews
    {
        public static async Task RunTask(HtmlNodeCollection nodes, IMessageChannel channel, string system, string newsType)
        {
            var tempDateTime = "";
            var teamTwoName = "";
            var systemIcon = "";

            var web = new HtmlWeb();
            var url = RetrieveUrl.GetNewsUrl();

            //var xbox = "53" + "&typeid=" + newsType;
            //var psn = "73" + "&typeid=" + newsType;

            if (system == "xbox" && newsType == "trade")
            {
                if (newsType == "trade")
                {
                    newsType = "7";
                    await RunReport(nodes, channel, system, newsType);
                }
                else
                {
                    newsType = "9";
                    await RunReport(nodes, channel, system, newsType);
                }
            } else if (system == "psn")
            {
                if (newsType == "trade")
                {
                    newsType = "7";
                    await RunReport(nodes, channel, system, newsType);
                }
                else
                {
                    newsType = "9";
                    await RunReport(nodes, channel, system, newsType);
                }
            }
            #region old
            //if (system == "psn") systemIcon = "https://media.playstation.com/is/image/SCEA/navigation_home_ps-logo-us?$Icon$";
            //else if (system == "xbox") systemIcon = "http://www.logospng.com/images/171/black-xbox-icon-171624.png";

            //foreach (var item in nodes)
            //{
            //    tempDateTime = item.SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/div/abbr").InnerText;
            //    var line = item.SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/div/h3").InnerText;

            //    var str = line;
            //    var removeThe = new string[] { "The ", "the " };

            //    foreach (var c in removeThe)
            //    {
            //        str = str.Replace(c, String.Empty);
            //    }

            //    if (newsType == "7")
            //    {
            //        var splitStr = str.Split(new string[] { "to " }, StringSplitOptions.None);
            //        splitStr[1] = splitStr[1].Replace("  ", " ");

            //        var iconTemp = WebUtility.HtmlDecode(item
            //            .SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/a[2]/img").Attributes["src"].Value);

            //        var lastNews = DateTime.Parse(tempDateTime);

            //        if (!SaveNews(lastNews, splitStr[0], splitStr[1], system))
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            try
            //            {
            //                var table = "";
            //                table = (system == "xbox") ? "XboxTrade" : "PsnTrade";

            //                using (var newsDb = new LiteDatabase(@"News.db"))
            //                {
            //                    var news = newsDb.GetCollection<NewsProperties.News>(table);
            //                    var result = news.Find(x => x.date.Equals(lastNews));
            //                    foreach (var headline in result)
            //                    {
            //                        var builder = new EmbedBuilder()
            //                            .WithColor(new Color(0xFF0019))
            //                            .WithTimestamp(lastNews)
            //                            .WithFooter(footer =>
            //                            {
            //                                footer
            //                                    .WithText("leaguegaming.com/fifa")
            //                                    .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
            //                            })
            //                            .WithThumbnailUrl(
            //                                "https://cdn0.iconfinder.com/data/icons/trading-outline/32/trading_outline_2._Location-512.png")
            //                            .WithAuthor(author =>
            //                            {
            //                                author
            //                                    .WithName("Sky Sports Trade Center")
            //                                    .WithIconUrl(systemIcon);
            //                            })
            //                            .AddField(splitStr[0], ":arrow_right:", false)
            //                            .AddField(splitStr[1], ":white_check_mark:", false);

            //                        var embed = builder.Build();
            //                        await channel.SendMessageAsync(
            //                                null,
            //                                embed: embed)
            //                            .ConfigureAwait(false);
            //                    }
            //                }
            //            }
            //            catch (Exception e)
            //            {
            //                Console.WriteLine(e);
            //                throw;
            //            }
            //        }
            //    }
            //    else if (newsType == "9")
            //    {
            //        var lastNews = DateTime.Parse(tempDateTime);
            //    }
            //}
            #endregion
        }

        private static async Task RunReport(HtmlNodeCollection nodes, IMessageChannel channel, string system, string newsType)
        {
            var systemIcon = "";
            var table = "";

            if (system == "psn") systemIcon = "https://media.playstation.com/is/image/SCEA/navigation_home_ps-logo-us?$Icon$";
            else if (system == "xbox") systemIcon = "http://www.logospng.com/images/171/black-xbox-icon-171624.png";

            foreach (var item in nodes)
            {
                var tempDateTime = item.SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/div/abbr").InnerText;
                var line = item.SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/div/h3").InnerText;


                //removes unwanted strings from the news feed
                var str = line;
                var remove = new string[] { "The ", "the " };

                foreach (var s in remove)
                {
                    str = str.Replace(s, string.Empty);
                }

                var lastNews = DateTime.Parse(tempDateTime);
                table = (system == "xbox") ? "XboxWaiver" : "PsnWaiver";

                if (newsType == "7")
                {
                    var iconTemp = item.SelectSingleNode(@"//*[@id='newsfeed_page']/ol/li[1]/a[2]/img").Attributes["src"].Value;

                    var splitStr = str.Split(new string[] { "to " }, StringSplitOptions.None);
                    splitStr[1] = splitStr[1].Replace(" ", " ");

                    if (!SaveNews.SaveTrade(lastNews, splitStr[0], splitStr[1], system, table)) break;
                    else
                    {
                        try
                        {
                            table = (system == "xbox") ? "XboxTrade" : "PsnTrade";

                            using (var newsDb = new LiteDatabase(@"/Database/News.db"))
                            {
                                var news = newsDb.GetCollection<NewsProperties.News>(table);
                                var result = news.Find(x => x.date.Equals(lastNews));
                                foreach (var headline in result)
                                {
                                    var builder = new EmbedBuilder()
                                        .WithColor(new Color(0xFF0019))
                                        .WithTimestamp(lastNews)
                                        .WithFooter(footer =>
                                        {
                                            footer
                                                .WithText("leaguegaming.com/fifa")
                                                .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
                                        })
                                        .WithThumbnailUrl(
                                            "https://cdn0.iconfinder.com/data/icons/trading-outline/32/trading_outline_2._Location-512.png")
                                        .WithAuthor(author =>
                                        {
                                            author
                                                .WithName("Sky Sports Trade Center")
                                                .WithIconUrl(systemIcon);
                                        })
                                        .AddField(splitStr[0], ":arrow_right:", false)
                                        .AddField(splitStr[1], ":white_check_mark:", false);

                                    var embed = builder.Build();
                                    await channel.SendMessageAsync(
                                            null,
                                            embed: embed)
                                        .ConfigureAwait(false);
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
                else if (newsType == "9")
                {
                    if (!SaveNews.SaveWaivers(lastNews, str, system, table)) break;
                    else
                    {
                        try
                        {
                            using (var waiverDb = new LiteDatabase(@"News.db"))
                            {
                                var waiver = waiverDb.GetCollection<NewsProperties.Waivers>(table);
                                var result = waiver.Find(x => x.dateTime.Equals(lastNews));

                                foreach (var headline in result)
                                {
                                    var builder = new EmbedBuilder()
                                        .WithColor(new Color(0xFF0019))
                                        .WithTimestamp(lastNews)
                                        .WithFooter(footer =>
                                        {
                                            footer
                                                .WithText("leaguegaming.com/fifa")
                                                .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
                                        })
                                        .WithAuthor(author =>
                                        {
                                            author
                                                .WithName("Sky Sports Waiver Wire")
                                                .WithIconUrl(systemIcon);
                                        })
                                        .WithDescription("**" +str + "**");
                                    var embed = builder.Build();
                                    await channel.SendMessageAsync(null, embed: embed).ConfigureAwait(false);
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
        }
    }
}
