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
    public class Manager : ModuleBase
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
            JobManager.Initialize(new Trades(news));
            JobManager.Initialize(new WaiverNews(news));
            Log.Logger.Information("Schedule Initialized.");
            return Task.CompletedTask;
        }
    }
}