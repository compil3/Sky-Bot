using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Serilog;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    internal class TeamStats : ModuleBase
    {
        [Command("ts")]
        [Summary(".ts TeamName xbox/psn [eg: .ts Liverpool Xbox]")]
        private async Task GetTeams(string teamName, string league)
        {
            var options = new RequestOptions { Timeout = 2 };
            await Context.Message.DeleteAsync(options);

            if (Context.Channel.Id == Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))
            {
                Embed embed;
                var builder = new EmbedBuilder()
                    .WithTitle("LGFA Team Stats")
                    .WithColor(new Color(0xFF0019))
                    .WithCurrentTimestamp()
                    .WithFooter(footer =>
                    {
                        footer
                            .WithText("leaguegaming.com/fifa")
                            .WithIconUrl("https://www.leaguegaming.com/images/league/icon/l53.png");
                    })
                    .WithDescription(
                        "**Command not yet implemented.**\n You can check the standings using the *.table* command.");
                embed = builder.Build();
                await Context.Channel.SendMessageAsync($"{Context.User.Mention}\n``[Stats Provided by Leaguegaming.com]``", embed: embed)
                    .ConfigureAwait(false);
            }
            else
                await ReplyAsync(
                    $"{Context.User.Mention} you are using the command in the wrong channel, try again in " +
                    $"{MentionUtils.MentionChannel(Convert.ToUInt64(Environment.GetEnvironmentVariable("stats_channel")))}");
        }
    }
}