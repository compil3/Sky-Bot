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
        public static async Task OnRulesReaction(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel chnl,
            SocketReaction reaction)
        {
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
                            "In order to proceed into Leagu715267433170206855egaming FIFA Discord, you must read and accept the rules.\n" +
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
        public static async Task OnSystemReaction(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, 
        SocketReaction reaction) 
        {
            SocketGuildUser reactionUser = reaction.User.IsSpecified ? reaction.User.Value as SocketGuildUser : null;

            ulong roleMessage = 715267433170206855;
            var xboxRole = new Emoji(":regional_indicator_x:");
            var psnRole = new Emoji(":regional_indicator_p:");
            var messageValue = await message.GetOrDownloadAsync();

            if (!reactionUser.IsBot) 
            {
               
            }


        }


    }
}
