using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ShellProgressBar;

namespace Sky_Bot.Extras.Tick
{
    public class TickComplete
    {
        public static void TickToCompletion(IProgressBar pbar, int totalTicks, int sleep, Action<int> childAction = null)
        {
            var initialMessage = pbar.Message;
            for (int i = 0; i < totalTicks; i++)
            {
                pbar.Message =
                    $"{initialMessage}: {i + 1} of {totalTicks}";// {Console.CursorTop}/{Console.WindowHeight}";
                childAction?.Invoke(i);
                Thread.Sleep(sleep);
                pbar.Tick($"End {i + 1} of {totalTicks}");// {initialMessage} {Console.CursorTop}/{Console.WindowHeight}");
            }
        }

        public static void TickToCompletion(IProgressBar pbar, int totalTicks, int sleep, string playerName, int playerCount,
            Action<int> childAction = null)
        {
            var initialMessage = pbar.Message;
            for (int i = 0; i < totalTicks; i++)
            {
                //pbar.Message = $"{initialMessage}: {i + 1} of {playerCount}";
                childAction?.Invoke(i);
                Thread.Sleep(sleep);
                pbar.Tick($"Updated {playerName}: {i+1} of {playerCount}");
            }
        }
    }
}
