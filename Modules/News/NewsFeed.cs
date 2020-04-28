using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;
using Sky_Bot.Essentials;

namespace Sky_Bot.Modules.News
{
    public class NewsFeed
    {
        private static HtmlNodeCollection FeedURl(string system, string newsType)
        {
            var web = new HtmlWeb();
            var url = RetrieveUrl.GetNewsUrl();
            HtmlDocument feed = null;
            HtmlNodeCollection nodes;

            var feedString = "//*[@id='newsfeed_page']/ol/li[1]";

            var xbox = "53" + "&typeid=" + newsType;
            var psn = "73" + "&typeid=" + newsType;

            if (system == "xbox")
            {
                feed = web.Load(url + xbox);
            }
            else if (system == "psn")
            {
                feed = web.Load(url + psn);
            }
            nodes = feed.DocumentNode.SelectNodes(feedString);
            return nodes;
        }
    }
}
