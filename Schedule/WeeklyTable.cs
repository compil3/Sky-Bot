using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using FluentScheduler;
using LGFA.Engines;
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
                EmbedBuilder embed = new EmbedBuilder();
                var systemIcon = "";
                var standingsUri = "";
                //await chnl.SendMessageAsync("``[League Tables provided by Leaguegaming.com``").ConfigureAwait(false);

                foreach (var table in system)
                {
                
                    var (teamStandings, teamPoints, currentSeason, leagueUrl, _system) = TeamStanding.GetStandings(table);
                    if (teamStandings == null) continue;
                    if (table.Contains("psn"))
                    {
                        systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                        standingsUri = standingsUri + "&leagueid=73&seasonid=" + currentSeason;
                    }
                    else if (table.Contains("xbox"))
                    {
                        systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                        standingsUri = standingsUri + "&leagueid=53&seasonid=" + currentSeason;
                    }
                    embed.Title = $"Current Standings - S{currentSeason}";
                    embed.Author = new EmbedAuthorBuilder()
                    {
                        Name = $"[Table provided by Leaguegaming.com\n League: {table.ToUpper()}",
                        IconUrl = systemIcon
                        
                    };
                    
                    TeamHelper.StandingsBuilder(teamStandings, teamPoints, table, ref embed);

                    await chnl.SendMessageAsync("", embed: embed.Build());
                }
            });
            Schedule(leagueTable).ToRunEvery(1).Weeks().On(DayOfWeek.Wednesday).At(12, 0);
        }
    }
}
