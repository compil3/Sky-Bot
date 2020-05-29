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
                var options = new RequestOptions{Timeout = 2};
                await Context.Message.DeleteAsync(options);

                if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
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
                        .WithDescription(
                            "**Command not yet implemented.**\n You can use *.ps* or *.cs* to check stats instead.");

                    embed = builder.Build();
                    await Context.Channel.SendMessageAsync("``[Stats Provided by LGFA]``", embed: embed)
                        .ConfigureAwait(false);
                }
                else 
                    await ReplyAsync(
                        $"{Context.User.Mention} this command is yet to be implemented, and you are using it in the wrong channel, try again in " +
                        $"{MentionUtils.MentionChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))}");
            }
    }
}
