using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using FluentScheduler;
using LGFA.Modules.News;
using Serilog;

namespace LGFA.Schedule
{
    public class Manager
    {
        public static Task Manage(IMessageChannel chnl, IMessageChannel news)
        {
            var dbPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var dbFolder = "Database/";
            var dbDir = Path.Combine(dbPath, dbFolder);

            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }
            JobManager.Initialize(new WeekUpdate(chnl));
            //JobManager.Initialize(new WeeklyTable(news));
            JobManager.Initialize(new Trades(news, chnl));
            JobManager.Initialize(new WaiverNews(news, chnl));
            Log.Logger.Information("Schedule Initialized.");
            return Task.CompletedTask;
        }
    }
}