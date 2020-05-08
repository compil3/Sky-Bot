
using System;
using System.Globalization;

namespace Sky_Bot.Properties
{
    public class CareerProperties
    {
        private string gamesPlayed;
        public string GamesPlayed
        {
            get => gamesPlayed;
            set
            {
                this.gamesPlayed = value;
                var splitRecord = this.gamesPlayed.Split('-');
                int wins = int.Parse(splitRecord[0]);
                int draws = int.Parse(splitRecord[1]);
                int loses = int.Parse(splitRecord[2]);
                int gamesPlayed = wins + draws + loses;
                this.gamesPlayed = gamesPlayed.ToString();
            }
        }

        public int Record { get; set; }

        private double _matchRating;
        public double AvgMatchRating { 
            get => _matchRating;
            set
            {
                _matchRating = value;
                var tempAmr = Convert.ToDouble(_matchRating);
                var rating = tempAmr / Convert.ToDouble(GamesPlayed);
                _matchRating = Math.Round(rating,2);
            }
        }

        public double Goals { get; set; }
        public string Assists { get; set; }
        public string KeyPasses { get; set; }

       
        public double ShotAttempts { get; set; }
        public string ShotsOnTarget { get; set; }
        public double ShotPercentage { get; set; }

        public double PassesCompleted { get; set; }
        public double PassesAttempted { get; set; }
        public double PassingPercentage { get; set; }

        public double Tackles { get; set; }
        public double TackleAttempts { get; set; }
        public double TacklePercentage { get; set; }

        public string Interceptions { get; set; }
        public string Blocks { get; set; }
        public string YellowCards { get; set; }
        public string RedCards { get; set; }
    }
}
