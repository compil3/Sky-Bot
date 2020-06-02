using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using FluentScheduler;
using LGFA.Modules.Helpers;

namespace LGFA.Schedule
{
    public class WeeklyTable : Registry
    {
        public WeeklyTable(IMessageChannel chnl)
        {
            var leagueTable = new Action(async () =>
            {
                var system = new string[] {"xbox", "psn"};
                await chnl.SendMessageAsync("``[Tables provided by Leaguegaming.com``").ConfigureAwait(false);

                foreach (var table in system)
                {
                    await chnl.SendMessageAsync(embed: TeamHelper.StandingEmbed(table)).ConfigureAwait(false);
                }
            });
            Schedule(leagueTable).ToRunNow().AndEvery(1).Weeks().On(DayOfWeek.Wednesday).At(12, 0);
        }
    }
}
