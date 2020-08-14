using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using FluentScheduler;
using LGFA.Engines;
using LGFA.Engines.Current;
using LGFA.Modules.Helpers;

namespace LGFA.Schedule
{
    public class WeeklyTable : Registry
    {
        public WeeklyTable(IMessageChannel chnl)
        {
            var leagueTable = new Action(async () =>
            {
                var system = new string[] {"psn", "xbox"};
                foreach (var table in system)
                {
                    EmbedBuilder embed = new EmbedBuilder();
                   // TeamInfo.ClubInfo("psn","Leverkusen", ref embed);

                    TeamStanding.GetStandings(table, ref embed, "WeeklyTable");
                    await chnl.SendMessageAsync("", embed: embed.Build());
                }
            });
            Schedule(leagueTable).ToRunNow().AndEvery(1).Weeks().On(DayOfWeek.Wednesday).At(12, 0);
        }
    }
}
