using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Properties
{
    public class PlayerProperties
    {
        internal class URL
        {
            public int Id { get; set; }
            public string PlayerName { get; set; }
            public string PlayerUrl { get; set; }
            public string System { get; set; }
        }

        internal class Career
        {
            public int Id { get; set; }
            public string PlayerName { get; set; }

            public string Record { get; set; }
            public double GamesPlayed { get; set; }
            public double AvgMatchRating { get; set; }

            public double Goals { get; set; }
            public string Assists { get; set; }

            public double ShotAttempts { get; set; }
            public string ShotsOnTarget { get; set; }
            public double ShotPercentage { get; set; }

            public double PassesCompleted { get; set; }
            public double PassesAttempted { get; set; }
            public double PassingPercentage { get; set; }
            public string KeyPasses { get; set; }

            public double Tackles { get; set; }
            public double TackleAttempts { get; set; }
            public double TacklePercentage { get; set; }

            public string Interceptions { get; set; }
            public string Blocks { get; set; }
            public string YellowCards { get; set; }
            public string RedCards { get; set; }
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
