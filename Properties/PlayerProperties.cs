namespace Sky_Bot.Properties
{
    public class PlayerProperties
    {
        public class StatProperties
        {
            public int SeasonId { get; set; }
            public int Id { get; set; }
            public string SeasonTypeId { get; set; }

            public string LeagueType { get; set; }
            public string LeagueDivision { get; set; }
            public string PlayerName { get; set; }
            public string UserSystem { get; set; }
            public string Position { get; set; }
            public string GamesPlayed { get; set; }

            public string Goals { get; set; }
            public string Assists { get; set; }
            public string ShotsOnGoal { get; set; }
            public string ShotsOnTarget { get; set; }
            public string ShotPercentage { get; set; }
            public string Tackles { get; set; }
            public string TackleAttempts { get; set; }
            public string TacklePercentage { get; set; }
            public string PassingPercentage { get; set; }
            public string KeyPasses { get; set; }
            public string Interceptions { get; set; }
            public string Blocks { get; set; }
            public string YellowCards { get; set; }
            public string RedCards { get; set; }

            public string LgRank { get; set; }
            public string GoalsAgainst { get; set; }
            public string ShotsAgainst { get; set; }
            public string Saves { get; set; }
            public string SavePercentage { get; set; }
            public string GoalsAgainstAvg { get; set; }
            
            public string Record { get; set; }
            public string AvgMatchRating { get; set; }
            public string ManOfTheMatch { get; set; }
            public string CleanSheets { get; set; }

            public string PlayerUrl { get; set; }
            public string TeamIcon { get; set; }
        }

        public class GoalieProperties
        {
            public int SeasonId { get; set; }
            public int Id { get; set; }
            public string SeasonTypeId { get; set; }

            public string userSystem { get; set; }
            public string playerName { get; set; }
            public string lgRank { get; set; }
            public string gamesPlayed { get; set; }
            public string record { get; set; }
            public string goalsAgainst { get; set; }
            public string shotsAgainst { get; set; }
            public string saves { get; set; }
            public string savePercentage { get; set; }
            public string goalsAgainstAvg { get; set; }
            public string cleanSheets { get; set; }
            public string manOfTheMatch { get; set; }
            public string avgMatchRating { get; set; }
            public string playerURL { get; set; }
            public string teamIcon { get; set; }
        }

        public class PlayerInfo
        {
            public int Id { get; set; }
            public string playerName { get; set; }
            public string playerUrl { get; set; }
            public string System { get; set; }
        }

        public class Career
        {
            public int Id { get; set; }
            public string PlayerName { get; set; }

            public string Record { get; set; }
            public int GamesPlayed { get; set; }
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

        internal class URL
        {
            public int Id { get; set; }
            public string PlayerName { get; set; }
            public string PlayerUrl { get; set; }
            public string System { get; set; }
        }

    }
}

