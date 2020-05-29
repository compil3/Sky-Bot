using Discord;

namespace LGFA.Modules.Helpers
{
    public class Missing
    {
        public static Embed NotFound(string playerName, string playerSystem, string playerUrl)
        {
            var builder = new EmbedBuilder()
                .WithTitle(playerName)
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithAuthor(author =>
                {
                    author
                        .WithName("There doesn't seem to be any statistics for this season.")
                        .WithIconUrl(playerSystem);
                })
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                });
            return builder.Build();
        }

        public static Embed CareerNotFound(string playerName, string playerUrl)
        {
            var builder = new EmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                        .WithName($"No Official career statistics found for {playerName}");
                })
                .WithUrl(playerUrl)
                .WithColor(new Color(0x26A20B))
                .WithCurrentTimestamp()
                .WithFooter(footer =>
                {
                    footer
                        .WithText("leaguegaming.com")
                        .WithIconUrl("https://www.leaguegaming.com/images/logo/logonew.png");
                });
            return builder.Build();
        }
    }
}