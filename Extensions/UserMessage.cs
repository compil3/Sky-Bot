using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;

namespace LGFA.Extensions
{
    public static class UserMessage
    {
        public static void AutoRemove(this IUserMessage message, int delay = 30)
        {
            if (delay <= 0 || message == null) return;
            DelayAction(TimeSpan.FromSeconds(delay), async _ =>
            {
                await message.DeleteAsync().ConfigureAwait(false);
            });
        }

        public static async Task<IUserMessage> AutoRemove(this Task<RestUserMessage> message, int delay = 30)
        {
            var cts = new CancellationTokenSource((delay +1) * 1000);
            return await message.ContinueWith(a =>
            {
                a.Result.AutoRemove(delay);
                return a.Result;
            }, cts.Token);
        }


        public static async Task<IUserMessage> AutoRemove(this Task<IUserMessage> message, int delay = 30)
        {
            var cts = new CancellationTokenSource((delay+1) * 1000);
            return await message.ContinueWith(a =>
            {
                a.Result.AutoRemove(delay);
                return a.Result;
            }, cts.Token);
        }
        private static void DelayAction(TimeSpan delay, Func<Task, Task> action)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            Task.Delay(delay, cancellationToken).ContinueWith(action, cancellationToken);
        }
    }
}
