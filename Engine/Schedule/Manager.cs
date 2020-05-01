using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using FluentScheduler;


namespace Engine.Schedule
{
    class Manager
    {
        public static Task Manage(IMessageChannel chnl)
        {
            JobManager.Initialize(new WeekUpdate(chnl));
            return Task.CompletedTask;
        }
    }
}
