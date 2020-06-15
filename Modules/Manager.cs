using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace Sky_Bot.Modules
{
    [RequireContext(ContextType.Guild)]
    public class Manager : ModuleBase
    {
        [Command("ab")]
        [Alias("addblock")]
        [Summary("Add a player to the trade block with optional value so other managers are aware of what you'd like in return.")]
        public async Task AddBlock(string playerName, [Remainder][Optional] string _value)
        {
            DateTime now = DateTime.Now;
            EmbedBuilder embed = new EmbedBuilder();
            if (Context.User is SocketGuildUser gUser)
            {
                if (gUser.Roles.Any(r => r.Name.Contains("Manager") || r.Name.Contains("Owner")))
                {
                    var team = gUser.Guild.Roles.FirstOrDefault(a =>
                        a.Name.StartsWith("Xbox ") || a.Name.StartsWith("PSN "));
                    if (team != null)
                    {
                        var tempTeam = team.Name;
                    }

                    embed.Author = new EmbedAuthorBuilder
                    {
                        Name = $"Trade Block"
                    };
                    embed.Footer = new EmbedFooterBuilder
                    {
                        Text = "leaguegaming.com",
                        IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                    };

                    if (AddToBlock(playerName, _value, team, now, ref embed))
                    {
                        await Context.User.SendMessageAsync("", embed: embed.Build());
                    }
                    else
                    {
                        await Context.User.SendMessageAsync("", embed: embed.Build());
                    }
                }
            }
            else
            {
                await ReplyAsync(
                    $"{Context.User.Mention} you are unable to use this command as you are not an owner or manager.");
            }
        }


        [Command("rb")]
        [Alias("removeblock")]
        [Summary("Removes a player from the trade block.  Only use this command if you are the team that currently owns the player.  `Abuse or tampering will be punished.`")]
        public async Task RemoveBlock(string playerName)
        {
            EmbedBuilder embed = new EmbedBuilder();
            if (Context.User is SocketGuildUser gUser)
            {
                if (gUser.Roles.Any(r => r.Name.Contains("Manager") || r.Name.Contains("Owner")))
                {
                    embed.Author = new EmbedAuthorBuilder
                    {
                        Name = $"Trade Block"
                    };
                    embed.Footer = new EmbedFooterBuilder
                    {
                        Text = "leaguegaming.com",
                        IconUrl = "https://www.leaguegaming.com/images/logo/logonew.png"
                    };

                    if (RemoveFromBlock(playerName, ref embed))
                    {
                        await Context.User.SendMessageAsync("", embed: embed.Build());
                    }
                    else
                    {
                        await Context.User.SendMessageAsync("", embed: embed.Build());
                    }
                }
            }
        }

        [Command("cb")]
        [Alias("checkblock")]
        public async Task CheckBlock(string league)
        {
            EmbedBuilder embed = new EmbedBuilder();
            if (Context.User is SocketGuildUser gUser)
            {
                if (gUser.Roles.Any(r => r.Name.Contains("Manager") || r.Name.Contains("Owner")))
                {
                    GetBlock(league);
                }
            }
        }

        private void GetBlock(string league)
        {
            EmbedBuilder embed = new EmbedBuilder();
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(path, dbFolder);

            using var database = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var col = database.GetCollection<TradeBlock.BlockProperties>("TradeBlock");

            var builders = new List<EmbedBuilder>();

           
            var paginatedBlock = new PaginatedMessage()
            {
            }; 
            var result = col.Query()
                .Where(x => x.Team.Contains(league))
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var tBlock in result)
            {
                var pages = new[]
                {
                    new PaginatedMessage()
                    {
                        
                        Author = new EmbedAuthorBuilder()
                        {
                            Name = "Trade Block"
                        }







                    }
                };
            }
        }

        private static bool RemoveFromBlock(string name, ref EmbedBuilder embed)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(path, dbFolder);

            using var database = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var col = database.GetCollection<TradeBlock.BlockProperties>("TradeBlock");

            try
            {
                col.Delete(name);
                embed.Description = $"Removed `{name}` from trade block.";
                return true;
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error removing {name} from Trade block: {e}");
                embed.Description = $"Failed to remove `{name}` from trade block. Contact Spillshot";
                return false;
            }
        }

        private static bool AddToBlock(string _name, string _value, SocketRole _role, DateTime date,
            ref EmbedBuilder embed)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(path, dbFolder);

            using var tradeDb = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var block = tradeDb.GetCollection<TradeBlock.BlockProperties>("TradeBlock");


            var _team = _role.ToString();
            var blockInfo = new TradeBlock.BlockProperties
            {
                Id = _name,
                Team = _team,
                Value = _value,

            };
            try
            {
                block.EnsureIndex(x => x.Name);
                var result = block.FindById(_name);
                if (result == null)
                {
                    block.Insert(blockInfo);
                    embed.Description = $"Added `{_name}` to trade block.";
                    embed.AddField("Value", _value, true);
                    return true;
                }
                else
                {
                    block.Update(blockInfo);
                    embed.Description = $"Updated `{_name}`";
                    embed.AddField("Changed To", _value, true);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Error adding player to trade block: {e}.");
                embed.Description = $"Error occured while adding `{_name}` to trade block.  Contact Spillshot";
                return false;
            }

        }
    }
}
