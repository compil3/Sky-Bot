using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using LGFA.Extensions;
using LGFA.Properties;

namespace LGFA.Modules
{
    [RequireContext(ContextType.Guild)]
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class Info : ModuleBase
    {
        [Command("info")]
        [Summary("Gets the league general information.")]
        public async Task GetInfo(string system)
        {
            var options = new RequestOptions{Timeout = 2};
            await Context.Message.DeleteAsync(options);

            Embed embed = null;
            var systemIcon = "";
            List<LeagueProperties> leagueInfo = null;

            switch (system)
            {
                case "psn":
                    systemIcon =
                        "https://cdn.discordapp.com/attachments/689119430021873737/711030693743820800/220px-PlayStation_logo.svg.jpg";
                    break;
                case "xbox":
                    systemIcon =
                        "https://cdn.discordapp.com/attachments/689119430021873737/711030386775293962/120px-Xbox_one_logo.svg.jpg";
                    break;
            }

            leagueInfo = LeagueInfo.GetSeason(system);
            foreach (var info in leagueInfo)
            {
                var builder = new EmbedBuilder()
                    .WithAuthor(author =>
                    {
                        author
                            .WithName($"LGFA {system.ToUpper()} Info")
                            .WithIconUrl(systemIcon);
                    })
                    .WithTitle("[Information provided by Leaguegaming.com]")
                    .AddField("Season", info.Season, true)
                    .AddField("Season Type", info.SeasonType, true)
                    .AddField("Week", info.Week, true)
                    .AddField("User", info.User, true)
                    .AddField("Completed/Games", info.Games, true);
                embed = builder.Build();
            }

            await Context.Channel.SendMessageAsync("", embed: embed)
                .ConfigureAwait(false);
        }
    }
}