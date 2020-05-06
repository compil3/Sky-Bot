using System;
using System.Threading;
using System.Threading.Tasks;
using Engine.Schedule;

namespace Engine
{
    class Program
    {
        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
            Thread.Sleep(Timeout.Infinite);
        }
        public static async Task MainAsync()
        {
            await Manager.Manage();
            await Task.Delay(Timeout.Infinite);
        }
    }
}
