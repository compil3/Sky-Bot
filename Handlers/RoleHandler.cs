using System;
using System.Linq;
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
            var reactionUser = reaction.User.IsSpecified ? reaction.User.Value as SocketGuildUser : null;

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
                        await reactionUser.KickAsync();
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
            var reactionUser = reaction.User.IsSpecified ? reaction.User.Value as SocketGuildUser : null;

            ulong roleMessage = 715267433170206855;
            var xboxRole = new Emoji("🇽");
            var psnRole = new Emoji("🇵");

            try
            {
                if (reaction.MessageId == roleMessage && !reactionUser.IsBot)
                {
                    var messageValue = await message.GetOrDownloadAsync();
                    var xbox = reactionUser.Guild.Roles.FirstOrDefault(x => x.Name == "Xbox");
                    var psn = reactionUser.Guild.Roles.FirstOrDefault(p => p.Name == "PSN");
                    var verified = reactionUser.Guild.Roles.FirstOrDefault(v => v.Name == "Accepted Rules");
                    if (reaction.Emote.Name == xboxRole.Name)
                    {
                        if (reactionUser.Roles.Contains(verified))
                        {
                            await reactionUser.AddRoleAsync(xbox);
                            await reactionUser.RemoveRoleAsync(verified);
                            await messageValue.RemoveReactionAsync(xboxRole, reactionUser, RequestOptions.Default);
                        }
                    }
                    else if (reaction.Emote.Name == psnRole.Name)
                    {
                        if (reactionUser.Roles.Contains(verified))
                        {
                            await reactionUser.AddRoleAsync(psn);
                            await reactionUser.RemoveRoleAsync(verified);
                            await messageValue.RemoveReactionAsync(psnRole, reactionUser, RequestOptions.Default);
                        }
                    }
                    else
                    {
                        await reactionUser.SendMessageAsync("Error assigning requested role, please contact a BOG.");
                    }
                }
            }
            catch (Exception e)
            {
                Log.Logger.Error($"Error assigning PSN/Xbox role. {e}");
                throw;
            }
        }
    }
}