using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace LGFA.Handlers
{
    public class Joined
    {
        public static async Task UserJoined(SocketGuildUser user)
        {
            var newUserRole = user.Guild.Roles.FirstOrDefault(n => n.Name == "New Member");
            await user.AddRoleAsync(newUserRole);
        }
    }
}
