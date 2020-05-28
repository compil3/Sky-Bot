using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using LGFA.Modules.News;
using Serilog;

namespace LGFA.Schedule
{
    public class Manager : ModuleBase
    {
        public static Task Manage(IMessageChannel chnl, IMessageChannel news)
        {
            JobManager.Initialize(new Trades(news));
            JobManager.Initialize(new WaiverNews(news));
            JobManager.Initialize(new WeekUpdate(chnl));
            Log.Logger.Information("Schedule Initialized.");
            return Task.CompletedTask;

        }
    }
}
