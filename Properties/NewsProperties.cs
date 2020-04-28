using System;
using System.Collections.Generic;
using System.Text;

namespace Sky_Bot.Properties
{
    public class NewsProperties
    {
        public class News
        {
            public int Id { get; set; }
            public DateTime date { get; set; }

            public string teamOneName { get; set; }
            public string teamOneIcon { get; set; }

            public string teamTwoName { get; set; }
            public string teamTwoIcon { get; set; }

            public string newsLineOne { get; set; }
            public string newsLineTwo { get; set; }
        }

        public class Waivers
        {
            public int Id { get; set; }
            public DateTime dateTime { get; set; }
            public string line { get; set; }
        }
    }
}
