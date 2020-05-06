using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LiteDB;
using Serilog;
using Sky_Bot.Engines;
using Sky_Bot.Properties;

namespace Sky_Bot.Modules
{
    public class PlayerStats : ModuleBase
    {
        [Command("ps")]
        [Summary(
            ".ps Web-Site-UserName SeasonNumber (optional)\n Eg: .ps SpillShot 7\n If the website name has spaces try wrapping the name (.ps \"Name tag\" ")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(ChannelPermission.SendMessages)]
        public async Task GetPlayerStatsLG(string playerLookup, string seasonID = null, string seasonType = null)
        {
            var tableName = "";
            var dbName = "";
            var outPutSeason = "";
            var guildID = Context.Guild.Id;

            if (guildID == 174342051318464512)
            {

               
                Log.Logger.Warning($"{Context.Guild.Name} (LG command triggered)");
                await Context.Channel.SendMessageAsync(null, embed: Player.GetPlayer(playerLookup, seasonType, seasonType)).ConfigureAwait(false);
                GC.Collect();
            }
            else if (Context.Guild.Id == 689119429375819951) await Context.Channel.SendMessageAsync($"{Context.Guild.Name} (LG command triggered)");
            GC.Collect();
        }

    }

    internal class DisplayBuilder : ModuleBase
    {
        internal static Embed EmbedMessage(string playerLookup, string dbName, string seasonID, ulong guild)
        {
            #region variables
            var success = false;
            var tableName = "";
            var outPutSeason = "";
            var systemIcon = "";
            var shotRecord = "";
            var goalRecord = "";
            var cleanMan = "";
            var gaa = "";
            var division = "";
            EmbedBuilder builder;
            Embed message = null;
            #endregion

            try
            {
                using var playerDatabase = new LiteDatabase(dbName);
                if (seasonID == null)
                {
                    tableName = "CurrentSeason";
                    outPutSeason = "Regular Season";
                }

                var player = playerDatabase.GetCollection<PlayerProperties.StatProperties>(tableName);
                var result = player.Find(x =>
                    x.PlayerName.StartsWith(playerLookup) || x.PlayerName.ToLower().StartsWith(playerLookup));

                foreach (var found in result)
                {
                    if (guild == 174342051318464512)
                    {
                        division = string.Empty;
                        if (found.Position == "G")
                        {
                            string[] shotsFaced = new string[3];
                            shotsFaced[0] = found.ShotsAgainst;
                            shotsFaced[1] = found.Saves;
                            shotsFaced[2] = found.SavePercentage + "%";
                            shotRecord = string.Join("-", shotsFaced);

                            string[] goalsAllowed = new string[2];
                            goalsAllowed[0] = found.GoalsAgainst;
                            goalsAllowed[1] = found.GoalsAgainstAvg;
                            goalRecord = string.Join(" - ", goalsAllowed);

                            string[] goalieCm = new string[2];
                            goalieCm[0] = found.CleanSheets;
                            goalieCm[1] = found.ManOfTheMatch;
                            cleanMan = string.Join("-", goalieCm);

                            if (found.UserSystem == "psn")
                            {
                                systemIcon =
                                    "https://media.playstation.com/is/image/SCEA/navigation_home_ps-logo-us?$Icon$";
                            }
                            else if (found.UserSystem == "xbox")
                            {
                                systemIcon =
                                    "http://www.logospng.com/images/171/black-xbox-icon-171624.png";
                            }

                            message = Helpers.BuilderLG(found.UserSystem, shotRecord, goalRecord, cleanMan, systemIcon,
                                found.SeasonId, found.PlayerName, found.Position, found.PlayerUrl, found.TeamIcon,
                                found.Record, found.AvgMatchRating, guild, division);
                        }
                        else if (found.Position != "G")
                        {

                        }
                    }
                    else if (guild == 689119429375819951)
                    {
                        if (found.Position == "G")
                        {
                            string[] shotsFaced = new string[3];
                            shotsFaced[0] = found.GoalsAgainst;
                            shotsFaced[1] = found.Saves;
                            shotsFaced[2] = found.SavePercentage;
                            shotRecord = string.Join("-", shotsFaced);

                            string[] goalsAgainstAvg = new string[2];
                            goalsAgainstAvg[0] = found.GoalsAgainst;
                            goalsAgainstAvg[1] = found.GoalsAgainstAvg;
                            gaa = string.Join("-", goalsAgainstAvg);

                            string[] goalieCleanMan = new string[2];
                            goalieCleanMan[0] = found.CleanSheets;
                            goalieCleanMan[1] = found.ManOfTheMatch;
                            cleanMan = string.Join("-", goalieCleanMan);
                            message = Helpers.BuilderPCN(found.UserSystem, shotRecord, goalRecord, cleanMan, systemIcon,
                                found.SeasonId, found.PlayerName, found.Position, found.PlayerUrl, found.TeamIcon,
                                found.Record, found.AvgMatchRating, guild, division);

                        } else if (found.Position != "G")
                        {

                        }
                       
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return message;
        }
    }
}
