using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;

namespace Sky_Bot.Schedule
{
    public class Manager : ModuleBase
    {
        public static Task Manage(IMessageChannel chnl)
        {
            JobManager.Initialize(new Weekly(chnl));
            return Task.CompletedTask;
        }
    }
}
