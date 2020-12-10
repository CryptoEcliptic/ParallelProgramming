using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _08.ReaderWriterLocksExample
{
    class Program
    {
        //Using ReaderWriterLockSlim allows while locked read operations to be executed, but not write operations and the oposite.
        static ReaderWriterLockSlim padlock = new ReaderWriterLockSlim();
        static Random random = new Random();
        static void Main(string[] args)
        {
            var tasks = new List<Task>();

            int x = 0;
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {    //Entering Readlock allows read operations and forbid write operations to be executed. 
                    padlock.EnterReadLock();

                    //EnterUpgradeableReadLock is used when we want to write during reading block of code.
                    //padlock.EnterUpgradeableReadLock();

                    //if (i % 2 == 0)
                    //{   //When in Upgradable readlock we should ented writelock -> execute write operation -> and then exit writelock.
                    //    padlock.EnterWriteLock();
                    //    x = 116;
                    //    padlock.ExitWriteLock();
                    //}

                    Console.WriteLine($"Entered read lock, x = {x}, pausing for 5sec");
                    Thread.Sleep(5000);

                    //After read operation is finished we need to exit the Readlock.
                    padlock.ExitReadLock();

                    //Exiting the EnterUpgradeableReadLock
                   //padlock.ExitUpgradeableReadLock();
                    Console.WriteLine($"Exited read lock, x = {x}.");
                }));
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

           while(true)
            {
                Console.ReadKey();
                //Entering Writelock allows write operations to be executed.
                padlock.EnterWriteLock();
                Console.WriteLine("Write lock acquired");

                int newValue = random.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");

                //After read operation is finished we should exit from the Writelock
                padlock.ExitWriteLock();
                Console.WriteLine("Write lock released");
            }
        }
    }
}
