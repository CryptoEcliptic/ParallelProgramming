using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace _14.ManualResetEventSlimAndSemaphoreSlim
{
    class Program
    {

        static void Main(string[] args)
        {
            //Unlike barrier and countdown events where we have counter, here we have only one time event. 
            //We set the signal on one lonaction and wait it on another location.
            var evt = new ManualResetEventSlim();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Boiling water...");
                //Sets the signal
                evt.Set();
            });

            var makeTea = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Waiting for water...");
                //Wait the signal
                evt.Wait();
                Console.WriteLine("Here is your tea!");
            });

            makeTea.Wait();


            //Semaphore Example
            //We have different threads that want something from the semaphore. The Semaphore is a counter which can increase and decrease
            //The first argument is the number of the requests that could be counted concurrently
            //The second argument is max number of requests that could be counted concurrently
            var semaphore = new SemaphoreSlim(2, 10);

            for (int i = 0; i < 20; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering task {Task.CurrentId}");
                    semaphore.Wait(); //count-- Here we stop and wait counts to be released. 
                    Console.WriteLine($"Processing task {Task.CurrentId}");
                });
            }

            while (semaphore.CurrentCount <= 2)
            {
                Console.WriteLine($"Semaphore count: {semaphore.CurrentCount}");
                Console.ReadKey(); //Pressing Key will release 2 counts in the semaphore and will make possible to execute two more tasks.
                semaphore.Release(2); //count +=2 (2 counts released and available to execute tasks)
            }
        }
    }
}
