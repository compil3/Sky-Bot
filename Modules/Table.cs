using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Engines;
using LGFA.Extensions;
using LGFA.Modules.Helpers;
using Serilog;

namespace LGFA.Modules
{
    public class Table : ModuleBase
    {
        [Command("table")]
        [Alias("standings")]
        [Summary("Retrieve the current season standings of Xbox or PSN.")]
        [RequireUserPermission(GuildPermission.SendMessages)]
        public async Task Standings(string league)
        {
            EmbedBuilder embed = new EmbedBuilder();
            var systemIcon = "";
            var standingsUri =
                "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=standing&";
            var options = new RequestOptions {Timeout = 2};
            await Context.Message.DeleteAsync(options);

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
               
                var (teamStandings, teamPoints, currentSeason, leagueUrl, system) = TeamStanding.GetStandings(league);
                if (teamStandings == null)
                {
                    TeamHelper.NoTable(league);
                    return;
                }
                if (league.Contains("psn"))
                {
                    systemIcon ="https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                    standingsUri = standingsUri + "&leagueid=73&seasonid=" + currentSeason;
                }
                else if (league.Contains("xbox"))
                {
                    systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                    standingsUri = standingsUri + "&leagueid=53&seasonid=" + currentSeason;

                }
                StandingBuilder(teamStandings, teamPoints, ref embed);

                embed.Title = $"Current Standings - S{currentSeason}";
                embed.WithUrl(standingsUri);
                embed.Author = new EmbedAuthorBuilder()
                {
                    Name = $"[Table provided by Leaguegaming.com]\n League: {league.ToUpper()}",
                    IconUrl = systemIcon
                };
                embed.Footer = new EmbedFooterBuilder()
                {
                    Text = "leaguegaming.com",
                    IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                };
                await ReplyAsync("", embed: embed.Build());
            }
            else
            {
                await ReplyAsync(
                    $"{Context.User.Mention} you are using the command in the wrong channel, try again in " +
                    $"{MentionUtils.MentionChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))}");
            }
        }

        public void StandingBuilder(List<string> ranking, List<string> points, ref EmbedBuilder embed)
        {
            var _ranking = string.Join(Environment.NewLine, ranking);
            var _points = string.Join(Environment.NewLine, points);

            embed.AddField("Ranking", _ranking, true);
            embed.AddField("Pts", _points, true);
        }
    }
}