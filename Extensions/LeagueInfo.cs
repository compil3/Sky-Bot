using System.Collections.Generic;
using HtmlAgilityPack;
using LGFA.Properties;

namespace LGFA.Extensions
{
    internal class LeagueInfo
    {
        public static List<LeagueProperties> GetSeason(string system)
        {
            var homepage = new HtmlWeb();
            HtmlDocument doc = null;
            HtmlDocument userDoc = null;
            if (system.Contains("psn"))
            {
                doc = homepage.Load("https://www.leaguegaming.com/forums/index.php?forums/lgfa-psn.604/");
                userDoc = homepage.Load(
                    "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=draftlist&leagueid=73&seasonid=18");
            }

            else if (system.Contains("xbox"))
            {
                doc = homepage.Load(
                    "https://www.leaguegaming.com/forums/index.php?forums/leaguegaming-fifa-association-lgfa.56/");
                userDoc = homepage.Load(
                    "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=draftlist&leagueid=53&seasonid=18");
            }

            var leagueInfo = new List<LeagueProperties>
            {
                new LeagueProperties
                {
                    System = system,
                    Season = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[2]/dd").InnerText,
                    SeasonType = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[3]/dd").InnerText,
                    Week = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[4]/dd").InnerText,
                    User = string.Empty,
                    Games = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[6]/dd").InnerText,
                    Latest = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[7]/dd").InnerText
                }
            };
            return leagueInfo;
        }
    }
}