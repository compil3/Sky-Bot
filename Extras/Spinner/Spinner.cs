using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;

namespace Sky_Bot.Extras.Spinner
{
    public static class Spinner
    {
        private static BackgroundWorker spinner = intialiseBackgroundWorker();

        private static int spinnerPosition = 25;
        private static int spinWait = 25;
        private static bool isRunning;

        private static bool IsRunning
        {
            get { return isRunning; }
        }

        private static BackgroundWorker intialiseBackgroundWorker()
        {
            BackgroundWorker obj = new BackgroundWorker();
            obj.WorkerSupportsCancellation = true;


            var stopwatch = new Stopwatch();


            obj.DoWork += delegate
            {
                spinnerPosition = Console.CursorLeft;
                while (!obj.CancellationPending)
                {
                    char[] spinChars = new char[] {'|', '/', '-', '\\'};
                    foreach (char spinChar in spinChars)
                    {
                        Console.CursorLeft = spinnerPosition;
                        Console.Write(spinChar);
                        System.Threading.Thread.Sleep(spinWait);
                    }
                }
            };
            return obj;
        }

        private static void OnTimeEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"Elapsed Time: {e}");
        }

        public static void Start(int spinWait)
        {
            isRunning = true;
            Spinner.spinWait = spinWait;
            if (!spinner.IsBusy) spinner.RunWorkerAsync();
            else throw new InvalidOperationException("Cannot start spinner whilst another spinner is running.");
        }

        public static void Start() { Start(25);}

        public static void Stop()
        {
            spinner.CancelAsync();
            while(spinner.IsBusy) System.Threading.Thread.Sleep(25);
            Console.CursorLeft = spinnerPosition;
            isRunning = false;
        }
    }
}
