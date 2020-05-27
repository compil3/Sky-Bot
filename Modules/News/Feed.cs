using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using HtmlAgilityPack;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace LGFA.Modules.News
{
    public class Feed
    {
        public static async Task RunTrades(HtmlDocument feed, string leagueId, IMessageChannel channel)
        {
            var feedString = "//*[@id='newsfeed_page']/ol/li[1]";
            var tempDateTime = "";
            var systemIcon = "";
            Embed embed = null;

            if (leagueId == "73")
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
            else if (leagueId == "53")
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";

            var nodes = feed.DocumentNode.SelectNodes(feedString);

            foreach (var item in nodes)
            {
                tempDateTime = item.SelectSingleNode("//*[@id='newsfeed_page']/ol/li[1]/div/abbr").InnerText;
                var line = item.SelectSingleNode("//*[@id='newsfeed_page']/ol/li[1]/div/h3").InnerText;
                var removeThe = new string[] { "The ", "the " };
                var newLine = "";
                if (line.Contains("The ")) newLine = line.Replace("The ", string.Empty);
                if (line.Contains("the ")) newLine = newLine.Replace("the ", string.Empty);

                var startTeam = newLine.Split(new string[] {"have", "traded", "to"}, StringSplitOptions.None);
                var endTeam = newLine.Split(new string[] {"to", "for"}, StringSplitOptions.None);


                var splitStr = newLine.Split(new string[] { "to " }, StringSplitOptions.None);
                splitStr[1] = splitStr[1].Replace("  ", " ");
                var tradeIcon = item.SelectSingleNode("//*[@id='newsfeed_page']/ol/li[1]/a[2]/img")
                    .Attributes["src"].Value;
                var lastNews = DateTime.Parse(tempDateTime);

                if (!NewsWriter.SaveTrade(lastNews, splitStr[0], splitStr[1], leagueId))
                {
                    break;
                }
                else
                {
                    try
                    {
                        
                        using var newsDb = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared");
                        var news = newsDb.GetCollection<LeagueNews.News>("Trades");
                        var result = news.Find(x => x.Date.Equals(lastNews));
                        foreach (var headline in result)
                        {
                            var builder = new EmbedBuilder()
                                .WithColor(new Color(0xFF0019))
                                .WithTimestamp(lastNews)
                                .WithFooter(footer =>
                                {
                                    footer
                                        .WithText("leaguegaming.com")
                                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                                })
                                .WithThumbnailUrl("https://www.leaguegaming.com/images/feed/trade.png")
                                .WithAuthor(author =>
                                {
                                    author
                                        .WithName("LGFA Breaking News: Trade")
                                        .WithIconUrl(systemIcon);
                                })
                                .AddField($"**{startTeam[0].Trim()}**",$"{endTeam[2].Trim()}", true)
                                .AddField($"**{endTeam[1].Trim()}**", $"{startTeam[2].Trim()}", true);

                            embed = builder.Build();
                        }
                        await channel.SendMessageAsync(null, embed: embed).ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error($"{e}");
                        throw;
                    }
                }
            }

        }

        public static async Task RunWaivers(HtmlDocument feed, string leagueId, IMessageChannel channel)
        {
            var waiverWeb = new HtmlWeb();
            var systemIcon = "";
            var tempDateTime = "";
            HtmlNodeCollection nodes = null;
            Embed embed = null;

            if (leagueId == "73")
            {
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                nodes = feed.DocumentNode.SelectNodes("//*[@id='newsfeed_page']/ol/li[1]");
            }
            else if (leagueId == "53")
            {
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                nodes = feed.DocumentNode.SelectNodes("//*[@id='newsfeed_page']/ol/li[1]/div/h3");
            }

            foreach (var items in nodes)
            {
                var newLine = "";
                tempDateTime = items.SelectSingleNode("//*[@id='newsfeed_page']/ol/li[1]/div/abbr").InnerText;
                var line = items.SelectSingleNode("//*[@id='newsfeed_page']/ol/li[1]/div/h3").InnerText;

                if (line.Contains("The ")) newLine = line.Replace("The ", string.Empty);
                if (line.Contains("the ")) newLine = newLine.Replace("the ", string.Empty);

                var lastNews = DateTime.Parse(tempDateTime);
                if (!NewsWriter.SaveWaiver(lastNews, newLine, leagueId)) return;
                else
                {
                    try
                    {
                        using var newsDb = new LiteDatabase(@"Filename=Database/LGFA.db;connection=shared");
                        var waivers = newsDb.GetCollection<LeagueNews.News>("Waivers");
                        var result = waivers.Find(w => w.Date.Equals(lastNews));

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
                                        .WithName("LGFA Waiver Wire")
                                        .WithIconUrl(systemIcon);
                                })
                                .WithDescription("**" +newLine + "**");
                            embed = builder.Build();
                        }
                        await channel.SendMessageAsync(null, embed: embed).ConfigureAwait(false);

                    }
                    catch (Exception e)
                    {
                        Log.Logger.Error($"{e}");
                        throw;
                    }
                }

            }
        }
    }
}
