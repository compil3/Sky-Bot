using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace LGFA.Modules
{
    [RequireOwner]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class Admin
    {
        //[Command("restart")]
        //[Summary(".restart to restart the bot.")]
        //public async Task Restart()
        //{
        //    var user = Context.User as SocketGuildUser;
        //    if (user.Id != 111252573054312448)
        //    {
        //        await ReplyAsync($"Sorry {user.Mention}, but you don't have permission to run that command.");
        //    }
        //    else
        //    {
        //        var programName = Assembly.GetExecutingAssembly().Location;
        //        await ReplyAsync(programName);
        //        Process.Start(programName);
        //        Environment.Exit(0);
        //    }
        //}
    }
}