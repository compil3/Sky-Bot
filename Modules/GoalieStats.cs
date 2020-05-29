using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class GoalieStats : ModuleBase
    {
           [Command("gs")]
            [Summary("Get a goalies current statistics. [Command not yet implemented]")]
            public async Task GetGoalieStats(string playerLookup)
            {
                Embed embed;
                var builder = new EmbedBuilder()
                    .WithTitle("LGFA Goalie Stats")
                    .WithColor(new Color(0xFF0019))
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("leaguegaming.com/fifa")
                            .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
                    })
                    .WithDescription("**Command not yet implemented.**\n You can use *.ps* or *.cs* to check stats instead.");
                    
                embed = builder.Build();
                await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: embed)
                    .ConfigureAwait(false);


            }
    }
}
