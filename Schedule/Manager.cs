using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using FluentScheduler;
using Serilog;

namespace Sky_Bot.Schedule
{
    public class Manager : ModuleBase
    {
        public static Task Manage(IMessageChannel chnl)
        {
            Log.Logger.Information("Schedule Initializing.");
             JobManager.Initialize(new WeekUpdate(chnl));
             Log.Logger.Information("Schedule Initialized.");
            return Task.CompletedTask;
        }
    }
}
