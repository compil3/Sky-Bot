﻿namespace LGFA.Properties
{
    public class TeamProperties
    {
        public int SeasonId { get; set; }
        public int Id { get; set; }
        public string SeasonTypeId { get; set; }
        public string System { get; set; }


        public string Rank { get; set; }
      
        private string _teamName;
        public string TeamName
        {
            get => _teamName;
            set
            {
                _teamName = value;
                string[] splitTeam = _teamName.Split(' ');
                if (splitTeam.Length >= 3)
                {
                    var split = _teamName.Split(new[] {' '}, 2);
                    Rank = split[0].Replace(")","").Trim();
                    _teamName = split[1];
                }
                else
                {
                    Rank = splitTeam[0].Replace(")","").Trim();
                    _teamName = splitTeam[1];
                }
            }
        }


        public string GamesPlayed { get; set; }
        public string GamesWon { get; set; }
        public string GamesDrawn { get; set; }
        public string GamesLost { get; set; }
        public string Points { get; set; }
        public string Streak { get; set; }
        public string GoalsFor { get; set; }
        public string GoalsAgainst { get; set; }
        public string CleanSheets { get; set; }
        public string LastTenGames { get; set; }
        public string HomeRecord { get; set; }
        public string AwayRecord { get; set; }
        public string OneGoalGames { get; set; }
        public string TeamIconUrl { get; set; }
        public string TeamURL { get; set; }
    }

    public class TeamStandings
    {
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Points { get; set; }
    }
}
