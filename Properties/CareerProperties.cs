
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

       
        public string ShotAttempts { get; set; }
        public string ShotsOnTarget { get; set; }

        private string _shotPercent;

        public string ShotPercentage
        {
            get => _shotPercent;
            set
            {
                this._shotPercent = value;
                string[] percent =new string[3];
                percent[0] = ShotAttempts;
                percent[1] = ShotsOnTarget;
                percent[2] = Math.Round((Goals / Convert.ToDouble(ShotAttempts)), 2).ToString("P", CultureInfo.InvariantCulture);
                this._shotPercent = string.Join("-", percent);
            }
        }

        public string PassesCompleted { get; set; }
        public string PassesAttempted { get; set; }
        public double PassingPercentage { get; set; }

        public string Tackles { get; set; }
        public string TackleAttempts { get; set; }
        public double TacklePercentage { get; set; }

        public string Interceptions { get; set; }
        public string Blocks { get; set; }
        public string YellowCards { get; set; }
        public string RedCards { get; set; }
    }
}
