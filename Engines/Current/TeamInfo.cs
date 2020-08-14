using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Discord;
using HtmlAgilityPack;
using LGFA.Extensions;
using LGFA.Properties;

namespace LGFA.Engines.Current
{
    internal abstract class TeamInfo
    {
        public class TeamBasics
        {
            private static string _teamUrl;

            public string TeamUrl
            {
                get => _teamUrl;
                set
                {
                    _teamUrl = value;
                    var temp = Regex.Match(_teamUrl, "<a href=[\"'](.+?)[\"'].*?>").Groups[1].Value;
                    _teamUrl = "https://www.leaguegaming.com/forums/" + temp;
                }
            }
            private static string _teamIconUrl;

            public string TeamIconUrl
            {
                get => _teamIconUrl;
                set
                {
                    _teamIconUrl = value;
                    var tempUrl = Regex.Match(_teamIconUrl, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                    tempUrl = tempUrl.Replace("p38", "p100");
                    _teamIconUrl = "http://www.leaguegaming.com" + tempUrl;
                }
            }
        }
        public static void ClubInfo(string system, string club, ref EmbedBuilder embed,
            [CallerMemberName] string methodName = "")
        {
            var web = new HtmlWeb();
            var standingsUrl = "https://www.leaguegaming.com/forums/index.php?leaguegaming/league&action=league&page=standing&leagueid=";
            var leagueInfo = LeagueInfo.GetSeason(system);
            var currentSeason = "";
            //var leagueId = "";
            var tempUrl = new string[3];
            var systemIcon = "";

            HtmlDocument standingsDoc;

            if (system.Contains("xbox"))
            {
                systemIcon =
                    "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";

                foreach (var info in leagueInfo)
                {
                    tempUrl[0] = standingsUrl;
                    tempUrl[1] = "53&seasonid=";
                    tempUrl[2] = info.Season;
                    currentSeason = info.Season;
                    standingsUrl = string.Join("", tempUrl);
                }

                standingsDoc = web.Load(standingsUrl);
                if (club.Contains("koln"))
                {
                    club = "Köln";
                }
                try
                {
                    var teamStats = standingsDoc.DocumentNode
                        .SelectSingleNode("//*[@id='content']/div/div/div[3]/div/div/div/div/table")
                        .Descendants("tr")
                        .Skip(2)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                        .Where(tr => tr[0].Contains(club))
                        .Select(tr => new TeamProperties()
                        {
                            TeamName = tr[0],
                            Rank = "",
                            GamesPlayed = tr[1],
                            GamesWon = tr[2],
                            GamesDrawn = tr[3],
                            GamesLost = tr[4],
                            Points = tr[5],
                            Streak = tr[6],
                            GoalsFor = tr[7],
                            GoalsAgainst = tr[8],
                            CleanSheets = tr[9],
                            LastTenGames = tr[10],
                            HomeRecord = tr[11],
                            AwayRecord = tr[12],
                            OneGoalGames = tr[13],
                        }).ToList();

                    var teamUrl = standingsDoc.DocumentNode
                              .SelectSingleNode("//*[@id='content']/div/div/div[3]/div/div/div/div/table")
                              .Descendants("tr")
                              .Skip(2)
                              .Select(tr => tr.Elements("td").Select(td => td.InnerHtml.Trim()).ToList())
                              .Where(tr => tr[0].Contains(club))
                              .Select(tr => new TeamBasics()
                              {
                                  TeamIconUrl = tr[0],
                                  TeamUrl = tr[0]
                              });
                    var teamIcon = "";
                    var teamUri = "";
                    foreach (var teamBasicse in teamUrl)
                    {
                        teamIcon = teamBasicse.TeamIconUrl;
                        teamUri = teamBasicse.TeamUrl;
                    }
                    foreach (var stats in teamStats)
                    {
                        if (stats.GamesPlayed == null) throw new NullReferenceException();
                        else
                        {
                            embed.Title = $"{stats.TeamName} - S{currentSeason} Stats";
                            embed.WithUrl(teamUri);
                            embed.Author = new EmbedAuthorBuilder()
                            {
                                Name = $"[Leaguegaming.com Statistics]",
                                IconUrl = systemIcon
                            };
                            embed.Footer = new EmbedFooterBuilder()
                            {
                                Text = "leaguegaming.com",
                                IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                            };
                            embed.WithCurrentTimestamp();
                            embed.WithThumbnailUrl(teamIcon);
                            embed.AddField("Ranking", stats.Rank, true);
                            embed.AddField("W-D-L", stats.Record, true);
                            embed.AddField("Pts", stats.Points, true);
                            embed.AddField("Streak", stats.Streak, true);
                            embed.AddField("Goals For", stats.GoalsFor, true);
                            embed.AddField("Goals Against", stats.GoalsAgainst, true);
                            embed.AddField("Clean Sheets", stats.CleanSheets, true);
                            embed.AddField("Last 10", stats.LastTenGames, true);
                            embed.AddField("Home Rec", stats.HomeRecord, true);
                            embed.AddField("Away Rec", stats.AwayRecord, true);
                            embed.AddField("One Goal Games", stats.OneGoalGames, true);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e}");
                }
            }
            else if (system.Contains("psn"))
            {
                if (club.Equals("koln", StringComparison.OrdinalIgnoreCase))
                {
                    club = "Köln";
                }
                systemIcon = "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";

                foreach (var info in leagueInfo)
                {
                    tempUrl[0] = standingsUrl;
                    tempUrl[1] = "73&seasonid=";
                    tempUrl[2] = info.Season;
                    currentSeason = info.Season;
                    standingsUrl = string.Join("", tempUrl);
                }

                standingsDoc = web.Load(standingsUrl);

                try
                {
                    var teamStats = standingsDoc.DocumentNode
                        .SelectSingleNode("//*[@id='content']/div/div/div[3]/div/div/div/div/table")
                        .Descendants("tr")
                        .Skip(2)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                        .Where(tr => tr[0].Contains(club))
                        .Select(tr => new TeamProperties()
                        {
                            Rank = "",
                            TeamName = tr[0],
                            GamesPlayed = tr[1],
                            GamesWon = tr[2],
                            GamesDrawn = tr[3],
                            GamesLost = tr[4],
                            Record = "",
                            Points = tr[5],
                            Streak = tr[6],
                            GoalsFor = tr[7],
                            GoalsAgainst = tr[8],
                            CleanSheets = tr[9],
                            LastTenGames = tr[10],
                            HomeRecord = tr[11],
                            AwayRecord = tr[12],
                            OneGoalGames = tr[13],
                        }).ToList();

                    var teamUrl = standingsDoc.DocumentNode
                        .SelectSingleNode("//*[@id='content']/div/div/div[3]/div/div/div/div/table")
                        .Descendants("tr")
                        .Skip(2)
                        .Select(tr => tr.Elements("td").Select(td => td.InnerHtml.Trim()).ToList())
                        .Where(tr => tr[0].Contains(club))
                        .Select(tr => new TeamBasics()
                        {
                            TeamIconUrl = tr[0],
                            TeamUrl = tr[0]
                        });

                    //need to grab team icon some how???
                    //var teamIcon = standingsDoc
                    var teamIcon = "";
                    var teamUri = "";
                    foreach (var teamBasicse in teamUrl)
                    {
                        teamIcon = teamBasicse.TeamIconUrl;
                        teamUri = teamBasicse.TeamUrl;
                    }
                    foreach (var stats in teamStats)
                    {
                        if (stats.GamesPlayed == null) throw new NullReferenceException();
                        else
                        {
                            embed.Title = $"{stats.TeamName} - S{currentSeason} Stats";
                            embed.WithUrl(teamUri);
                            embed.Author = new EmbedAuthorBuilder()
                            {
                                Name = $"[Leaguegaming.com Statistics]",
                                IconUrl = systemIcon
                            };
                            embed.Footer = new EmbedFooterBuilder()
                            {
                                Text = "leaguegaming.com",
                                IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                            };
                            embed.WithCurrentTimestamp();
                            embed.WithThumbnailUrl(teamIcon);
                            embed.AddField("Ranking", stats.Rank, true);
                            embed.AddField("W-D-L", stats.Record, true);
                            embed.AddField("Pts", stats.Points, true);
                            embed.AddField("Streak", stats.Streak, true);
                            embed.AddField("Goals For", stats.GoalsFor, true);
                            embed.AddField("Goals Against", stats.GoalsAgainst, true);
                            embed.AddField("Clean Sheets", stats.CleanSheets, true);
                            embed.AddField("Last 10", stats.LastTenGames, true);
                            embed.AddField("Home Rec", stats.HomeRecord, true);
                            embed.AddField("Away Rec", stats.AwayRecord, true);
                            embed.AddField("One Goal Games", stats.OneGoalGames, true);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e}");
                }
            }
        }
    }
}
