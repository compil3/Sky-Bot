using System;

namespace LGFA.Properties
{
    class LeagueNews
    {
        public class News
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }

            public string TeamOneName { get; set; }
            public string TeamOneIcon { get; set; }

            public string TeamTwoName { get; set; }
            public string TeamTwoIcon { get; set; }

            public string NewsLineOne { get; set; }
            public string NewsLineTwo { get; set; }

            public string System { get; set; }
        }

        public class Waivers
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public string Line { get; set; }
            public string System { get; set; }
        }
    }
}
