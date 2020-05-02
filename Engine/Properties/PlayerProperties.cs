using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Properties
{
    internal class PlayerProperties
    {
        internal class URL
        {
            public int Id { get; set; }
            public string PlayerName { get; set; }
            public string PlayerUrl { get; set; }
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
    }
}
