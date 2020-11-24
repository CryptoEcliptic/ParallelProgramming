using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace _03.WaitingForTimeToPass
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            //var t = new Task(() =>
            //{
            //    Console.WriteLine("You have 5 seconds to disarm this bomb by pressing a key");
            //    bool canceled = token.WaitHandle.WaitOne(5000);
            //    Console.WriteLine(canceled ? "Bomb disarmed." : "BOOM!!!!");
            //}, token);
            //t.Start();

            //// unlike sleep and waitone when using SpinWait
            //// thread does not give up its turn
            //// avoiding a context switch
            ////Thread.SpinWait(10000);
            ////SpinWait.SpinUntil(() => false);
            ////Console.WriteLine("Are you still here?");

            //Console.ReadKey();
            //cts.Cancel();


            var cts1 = new CancellationTokenSource();
            var token1 = cts.Token;

            Task t1 = new Task(() =>
            {
                Console.WriteLine("It will take 5 seconds.");

                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }

                Console.WriteLine("Task t1 is finished");

            }, token1);
            t1.Start();
            //Waiting the particular Task to complete. The cancellattion token will tell us if the task is cancelled.
            //We cen use Wait method also without providing the cancellation token
            //t1.Wait(token1);

            Task t2 = new Task(() => { Thread.Sleep(3000); });
            t2.Start();

            //Task.WaitAll(t1, t2); //Waiting t1 and t2 Tasks
            Task.WaitAny(t1, t2); //Waiting any of the Tasks to complete. If one of them finishes, the secont won't be waited. In that case t2 is going to finish first.

            Console.WriteLine($"Task t1 is with status: {t1.Status}");
            Console.WriteLine($"Task t2 is with status: {t2.Status}");

            Console.WriteLine("Main program done, press any key.");
            Console.ReadKey();
        }
    }
}
