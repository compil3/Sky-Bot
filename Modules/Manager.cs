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
using Discord.Addons.Interactive;
using Discord.Commands;
using Discord.WebSocket;
using HtmlAgilityPack;
using LGFA.Extensions;
using LGFA.Properties;
using LiteDB;
using Serilog;

namespace Sky_Bot.Modules
{
    [RequireContext(ContextType.Guild)]
    public class Manager : InteractiveBase
    {
        [Command("ab")]
        [Alias("addblock")]
        [Summary("Add a player to the trade block with optional value so other managers are aware of what you'd like in return.")]
        public async Task AddBlock(string playerName, [Remainder][Optional] string _value)
        {
            DateTime now = DateTime.Now;
            EmbedBuilder embed = new EmbedBuilder();
            var team = "";
            if (Context.User is SocketGuildUser gUser)
            {
                if (gUser.Roles.Any(r => r.Name.Contains("Manager") || r.Name.Contains("Owner")))
                {
                    var roleList = gUser.Roles.ToList();
                    foreach (var uRole in roleList)
                    {
                        var currentSeason = "";
                        if (uRole.Name.Contains("PSN ") && !uRole.Name.Contains("LGFA PSN"))
                        {
                            team = uRole.Name;
                        }
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
                    //GetBlock(league);
                    //await PagedReplyAsync(GetBlock(league));
                    var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var dbFolder = "Database/";
                    var dbDir = Path.Combine(path, dbFolder);

                    using var database = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
                    var col = database.GetCollection<TradeBlock.BlockProperties>("TradeBlock");

                    var listing = "";
                    var author = new EmbedAuthorBuilder()
                        .WithName("Trade Block");
            
                    var result = col.Query()
                        .Where(x => x.Team.Contains(league))
                        .OrderBy(x => x.Name)
                        .ToList();
                    
                    var list = new List<string>();
                    foreach (var tBloc in result)
                    {
                        listing += $"**{tBloc.Team}**\nPlayer:`{tBloc.Id}`\nRequested Value:`{tBloc.Value}`";
                        list.Add(listing);
                        listing = null;
                    }

                    var paginatedMessage = new PaginatedMessage
                    {
                        Title = "Trade Block",
                        Pages = list,
                        //Author = author,
                        Options = _Options
                    };

                    await PagedReplyAsync(paginatedMessage);
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

            var listing = "";
            var author = new EmbedAuthorBuilder()
                .WithName("Trade Block");
            
            var result = col.Query()
                .Where(x => x.Team.Contains(league))
                .OrderBy(x => x.Name)
                .ToList();
            var paginatedMessage = new PaginatedMessage
            {
                Author = author,
                Options = _Options
            };
            var list = new List<string>();
            foreach (var tBloc in result)
            {
                listing += $"**{tBloc.Team}**\n`{tBloc.Id}`\n`{tBloc.Value}`";
                list.Add(listing);
                listing = null;
            }

            paginatedMessage.Pages = list;

        }

        private PaginatedAppearanceOptions _Options => new PaginatedAppearanceOptions()
        {
            JumpDisplayOptions = JumpDisplayOptions.Never,
            DisplayInformationIcon = false,
            FooterFormat = $"Leaguegaming.com"
        };
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

        private static bool AddToBlock(string _name, string _value, string _team, DateTime date,
            ref EmbedBuilder embed)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(path, dbFolder);

            using var tradeDb = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var block = tradeDb.GetCollection<TradeBlock.BlockProperties>("TradeBlock");


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
