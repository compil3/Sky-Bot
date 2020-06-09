using System.Threading.Tasks;
using Discord.Commands;

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
            public async Task AddBlock(string _name, [Optional]string _value) => await ReplyAsync("");

            [Command("rb")]
            [Alias("removeblock")]
            public async Task RemoveBlock(string name) => await ReplyAsync("");
        }

    

    }
}