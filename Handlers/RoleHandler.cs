using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Serilog;

namespace LGFA.Handlers
{
    public class RoleHandler
    {
        public static async Task OnReaction(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel chnl,
            SocketReaction reaction)
        {
            Log.Logger.Information("Loaded RoleHandler");
            SocketGuildUser reactionUser = reaction.User.IsSpecified ? reaction.User.Value as SocketGuildUser : null;

            ulong ruleMessageId = 712883043823779871;

            var acceptedRules = new Emoji("✅");
            var declinedRules = new Emoji("❌");
            var messageValue = await message.GetOrDownloadAsync();

            if (reactionUser != null)
            {
                var newMember = reactionUser.Guild.Roles.FirstOrDefault(n => n.Name == "New Member");
                var acceptedMember = reactionUser.Guild.Roles.FirstOrDefault(a => a.Name == "Accepted Rules");
                //add timer to wipe reactions every day.
                if (reaction.MessageId == ruleMessageId && !reactionUser.IsBot)
                {
                    Log.Logger.Information($"Rules triggered by: {reactionUser.Nickname}");

                    if (reaction.Emote.Name == acceptedRules.Name)
                    {
                        if (reactionUser.Roles.Contains(newMember))
                        {
                            await reactionUser.AddRoleAsync(acceptedMember);
                            await reactionUser.RemoveRoleAsync(newMember);
                            await messageValue.RemoveReactionAsync(acceptedRules, reactionUser, RequestOptions.Default);
                        }
                    }
                    else if (reaction.Emote.Name == declinedRules.Name)
                    {
                        await reactionUser.SendMessageAsync(
                            "In order to proceed into Leaguegaming FIFA Discord, you must read and accept the rules.\n" +
                            "Please re-join, re-read the rules and accept if you wish to be part of our Discord community.");
                        await reactionUser.KickAsync(null);
                    }
                }
            }
            if (messageValue == null)
            {
                Log.Logger.Error("Could not get message for reaction roles.");
                await Task.CompletedTask;
            }
        }

    }
}
