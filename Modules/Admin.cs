using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LGFA.Modules
{
    public class Admin : ModuleBase
    {
        private static DiscordSocketClient _client;
        
        public Admin(IServiceProvider services)
        {
            _client = services.GetRequiredService<DiscordSocketClient>();

        }


        [Command("show",RunMode = RunMode.Async)]
        [Summary("testing new bot re-write")]
        public async Task ListGuilds()
        {
            StringBuilder sb = new StringBuilder();
            var guilds = _client.Guilds.ToList();
            foreach (var guild in guilds)
            {
                sb.AppendLine($"Name: {guild.Name} ID: {guild.Id} Owner: {guild.Owner}");
            }
            await ReplyAsync(sb.ToString());
        }

        [RequireOwner]
        [Command("restart")]
        [Summary(".restart")]
        [Remarks("Restarts the bot")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Restart()
        {
            var user = Context.User as SocketGuildUser;
            if (user.Id != 111252573054312448)
            {
                await ReplyAsync($"Sorry {user.Mention}, but you don't have permission to run that command.");
            }
            else
            {
                
                var programName = Assembly.GetExecutingAssembly().Location;
                await ReplyAsync(programName);
                System.Diagnostics.Process.Start(programName);
                Environment.Exit(0);
            }
        }
    }

}

