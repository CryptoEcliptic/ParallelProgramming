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
        }
    }
}
