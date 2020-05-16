using System;
using System.Collections.Generic;
using System.Text;

namespace LGFA.Properties
{
    class General
    {
        public class Season
        {
            public string CurrentSeason { get; set; }
            public string PreviousSeason { get; set; }
        }

        public class UrlSettings
        {
            public string XboxSeasonId { get; set; }
            public string XboxStandingsUrl { get; set; }
            public string XboxPlayerStatsUrl { get; set; }
            public string XboxDraftListUrl { get; set; }
            public string XboxPrevious { get; set; }
            public string PsnSeasonId { get; set; }
            public string PsnStandingsUrl { get; set; }
            public string PsnPlayerStatsUrl { get; set; }
            public string PsnDraftListUrl { get; set; }
            public string PsnPrevious { get; set; }
            public string CurrentSeason { get; set; }
            public string PreviousSeason { get; set; }
            public string NewsUrl { get; set; }
        }
    }
}
