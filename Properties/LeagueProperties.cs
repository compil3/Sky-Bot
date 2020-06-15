using System;
using System.Linq;
using HtmlAgilityPack;

namespace LGFA.Properties
{
    internal class LeagueProperties
    {
        public string System { get; set; }

        public string Season { get; set; }
        public string SeasonType { get; set; }
        public string Week { get; set; }

        private string _users;
        public string User { 
            get => _users;
            set
            {
                var draftPage = new HtmlWeb();
                HtmlDocument doc = null;

                if (System.Contains("psn"))
                {
                    doc =
                        draftPage.Load(
                            $"https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=draftlist&leagueid=73&seasonid={Season.Trim()}");
                    var entries = doc.DocumentNode.SelectNodes("//*[@id='lgtable']/tbody/tr");
                    this._users = entries.Count.ToString();
                } else if (System.Contains("xbox"))
                {
                    doc = draftPage.Load(
                        $"https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=draftlist&leagueid=53&seasonid={Season.Trim()}");
                    var entries = doc.DocumentNode.SelectNodes("//*[@id='lgtable']/tbody/tr");
                    this._users = entries.Count.ToString();
                }
            }

        }
        public string Games { get; set; }
        public string Latest { get; set; }
    }
}