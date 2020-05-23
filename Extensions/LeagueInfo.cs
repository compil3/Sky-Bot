using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using LGFA.Properties;

namespace LGFA.Extensions
{
    class LeagueInfo
    {
        public static List<LeagueProperties> GetSeason()
        {
            var homepage = new HtmlWeb();

            var doc = homepage.Load(
                "https://www.leaguegaming.com/forums/index.php?forums/leaguegaming-fifa-association-lgfa.56/");


            var leagueInfo = new List<LeagueProperties>
            {
                new LeagueProperties
                {
                    Season = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[2]/dd").InnerText,
                    SeasonType = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[3]/dd").InnerText,
                    Week = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[4]/dd").InnerText,
                    User = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[5]/dd").InnerText,
                    Games = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[6]/dd").InnerText,
                    Latest = doc.DocumentNode.SelectSingleNode("//*[@id='boardStats']/div/dl[7]/dd").InnerText,
                }
            };
            return leagueInfo;
        }
    }
}
