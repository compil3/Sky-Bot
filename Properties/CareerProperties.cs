
using System;
using System.Collections.Generic;
using System.Globalization;
using HtmlAgilityPack;

namespace LGFA.Properties
{
    public class CareerProperties
    {
        public string something { get; set; }
        public string System { get; set; }
        public string SystemIcon { get; set; }
        public string TeamIcon { get; set; }
        public string Position { get; set; }
        public string SeasonId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerUrl { get; set; }
        public string SeasonType { get; set; }

        #region Record
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
                this.gamesPlayed = gamesPlayed.ToString().Trim();
            }
        }

        public string Record { get; set; }

        private string _matchRating;
        public string AvgMatchRating
        {
            get => _matchRating;
            set
            {
                var rating = Convert.ToDouble(MatchRating) / Convert.ToDouble(GamesPlayed);
                _matchRating = Math.Round(rating,2).ToString(CultureInfo.InvariantCulture);
            }
        }
        //used for season career stats
        public string MatchRating { get; set; }
        #endregion
        

        #region Offensive
        public string Goals { get; set; }
        public string Assists { get; set; }

      private string _goalsPerGame;
        public string GoalsPerGame
        {
            get=> _goalsPerGame;
            set => _goalsPerGame = Math.Round((Convert.ToDouble(Goals)) / Convert.ToDouble(GamesPlayed),3).ToString("F");
        }

        private string _shotsPerGoal;

        public string ShotPerGoal
        {
            get => _shotsPerGoal;
            set
            {
                if (Math.Abs(Convert.ToDouble(Goals)) > 0)
                {
                    string[] shotsGoalPercent = new string[2];
                    shotsGoalPercent[0] = Math.Round((Convert.ToDouble(ShotAttempts)) / Convert.ToDouble(Goals), 3).ToString("F");
                    shotsGoalPercent[1] = Math.Round(Convert.ToDouble(Goals) / Convert.ToDouble(ShotAttempts), 2).ToString("P");
                    _shotsPerGoal = string.Join(" - ", shotsGoalPercent);
                }
                else _shotsPerGoal = "0.0";

            }
        }

        public string ShotAttempts { get; set; }
        public string ShotsOnTarget { get; set; }

        private string _shotPercent;

        public string ShotPercentage
        {
            get => _shotPercent;
            set => _shotPercent = Math.Round(Convert.ToDouble(Goals) / Convert.ToDouble(ShotAttempts), 2).ToString("P");
        }

        private string _shotPerGame;

        public string ShotPerGame
        {
            get => _shotPerGame;
            set => _shotPerGame = Math.Round((Convert.ToDouble(ShotAttempts)) / Convert.ToDouble(GamesPlayed), 3).ToString("F");
        }

        private string _shotSot;
        public string ShotSot
        {
            get => _shotSot;
            set
            {
                string[] tempShotSot = new string[2];
                tempShotSot[0] = ShotAttempts;
                tempShotSot[1] = ShotsOnTarget;
                _shotSot = string.Join(" - ", tempShotSot);
            }
        }

        #endregion

        #region Passing
        private string _passPercent;
        public string PassesCompleted { get; set; }
        public string PassesAttempted { get; set; }

        private string _passRecord;

        public string PassRecord
        {
            get => _passRecord;
            set
            {
                string[] tempPassRec = new string[2];
                tempPassRec[0] = PassesCompleted;
                tempPassRec[1] = PassesAttempted;
                _passRecord = string.Join(" - ", tempPassRec);
            }
        }

        public string PassingPercentage
        {
            get => _passPercent;
            set => _passPercent = Math.Round((Convert.ToDouble(PassesCompleted) / Convert.ToDouble(PassesAttempted)), 3).ToString("P");
        }

        private string _avgPassGame;

        public string PassPerGame
        {
            get => _avgPassGame;
            set
            {
                string[] passGamePercent = new string[2];
                passGamePercent[0] = Math.Round((Convert.ToDouble(PassesAttempted)) / Convert.ToDouble(GamesPlayed), 3)
                    .ToString("F");
                passGamePercent[1] = _passPercent = Math.Round((Convert.ToDouble(PassesCompleted) / Convert.ToDouble(PassesAttempted)), 3).ToString("P");
                _avgPassGame = string.Join(" - ", passGamePercent);
            }
        }

        private string _assistPerGame;

        public string AssistPerGame
        {
            get => _assistPerGame;
            set => _assistPerGame = Math.Round((Convert.ToDouble(Assists)) / Convert.ToDouble(GamesPlayed), 3).ToString("F");
        }

        private string _keyPassPerGame;
        public string KeyPasses { get; set; }

        public string KeyPassPerGame
        {
            get => _keyPassPerGame;
            set => _keyPassPerGame = Math.Round((Convert.ToDouble(KeyPasses)) / Convert.ToDouble(GamesPlayed), 3).ToString("F");
        }

        #endregion

        #region Defensive
        private string _tackling;
        public string Tackles { get; set; }
        public string TackleAttempts { get; set; }

        public string Tackling
        {
            get => _tackling;
            set
            {
                string[] defensive = new string[2];
                defensive[0] = Tackles;
                defensive[1] = TackleAttempts;
                _tackling = string.Join("-", defensive);
            }
        }

        private string _tacklePercent;
        public string TacklePercent
        {
            get => _tacklePercent;
            set => _tacklePercent = Math.Round((Convert.ToDouble(Tackles)) / Convert.ToDouble(TackleAttempts),2)
                    .ToString("P");
        }

        private string _tacklePerGame;

        public string TacklesPerGame
        {
            get => _tacklePerGame;
            set => _tacklePerGame = Math.Round((Convert.ToDouble(Tackles)) / Convert.ToDouble(GamesPlayed), 2)
                .ToString("F");
        }

        public string Interceptions { get; set; }
        private string _interPerGame;

        public string InterPerGame
        {
            get=>_interPerGame;
            set => _interPerGame = Math.Round((Convert.ToDouble(Interceptions)) / Convert.ToDouble(GamesPlayed), 2)
                .ToString("F");
        }

        private string _blocksPerGame;
        public string Blocks { get; set; }

        public string BlocksPerGame
        {
            get=>_blocksPerGame;
            set=>_blocksPerGame=Math.Round((Convert.ToDouble(Blocks)) / Convert.ToDouble(GamesPlayed),2).ToString("F");
        }
        private string _brick;
        public string Wall
        {
            get=>_brick;
            set
            {
                _brick = value;
                string[] steals = new string[2];
                steals[0] = Interceptions;
                steals[1] = Blocks;
                _brick = string.Join("-", steals);
            }
        }

        public string PossW { get; set; }
        public string PossL { get; set; }

        private string _poss;

        public string Poss
        {
            get => _poss;
            set
            {
                string[] possWPossL = new string[2];
                possWPossL[0] = PossW;
                possWPossL[1] = PossL;
                _poss = string.Join("-", possWPossL);
            }
        }
        #endregion

        #region Discipline
        public string YellowCards { get; set; }
        public string RedCards { get; set; }
        private string _discipline;

        public string Discipline
        {
            get => _discipline;
            set
            {
                _discipline = value;
                string[] cards = new string[2];
                cards[0] = YellowCards;
                cards[1] = RedCards;
                _discipline = string.Join("-", cards);
            }
        }
        #endregion
    }
}
