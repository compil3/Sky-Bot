using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using LGFA.Properties;
using LiteDB;

namespace Sky_Bot.Modules
{
    public class Manager
    {
        [Group("Manager Commands")]
        [RequireContext(ContextType.DM)]
        public class Block : ModuleBase
        {
            [Command("cb")]
            [Alias("checkblock")]
            public async Task CheckTradeBlock() => await ReplyAsync("");

            [Command("ab")]
            [Alias("addblock")]
            public async Task AddBlock(string _name, [Optional]string _value)
            {
                if (Context.User is SocketGuildUser gUser)
                {
                    if (gUser.Roles.Any(r => r.Name == "Manager"))
                    {
                        AddToBlock(_name, _value);
                        await Context.User.SendMessageAsync("PAGINATED TRADEBLOCK EMBED");
                    }
                }
                await ReplyAsync("");
            }

           

            [Command("rb")]
            [Alias("removeblock")]
            public async Task RemoveBlock(string name)
            {
                if (Context.User is SocketGuildUser gUser)
                {
                    if (gUser.Roles.Any(r => r.Name == "Manager"))
                    {
                        await Context.User.SendMessageAsync("REMOVE USER FROM TB");
                    }
                }
                await ReplyAsync("");
            }
        }
        private void AddToBlock(string name, string value)
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(path, dbFolder);

            using var tradeDb = new LiteDatabase($"Filename={dbDir}LGFA.db;connection=shared");
            var block = tradeDb.GetCollection<TradeBlock.BlockProperties>("TradeBlock");

            block.EnsureIndex(x => x.Name);

            var result = block.Query()
                .Where(x => x.Name.Contains(name))
                .ToList();

            foreach (var t in result)
            {
                
            }
            throw new System.NotImplementedException();
        }

    

    }
}