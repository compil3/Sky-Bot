using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Serilog;

namespace LGFA.Handlers
{
    public class Joined
    {
        public static async Task UserJoined(SocketGuildUser user)
        {
            var newUserRole = user.Guild.Roles.FirstOrDefault(n => n.Name == "New Member");
            await user.AddRoleAsync(newUserRole);
            Log.Logger.Information($"{user.Username} assigned {newUserRole} for joining.");
        }
    }
}