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
        public static Task Manage()
        {
            JobManager.Initialize(new WeekUpdate());
            return Task.CompletedTask;
        }
    }
}
