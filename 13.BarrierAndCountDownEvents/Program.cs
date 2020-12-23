using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace _13.BarrierAndCountDownEvents
{
    class Program
    {
        //Allows multiple tasks to work on stages. The first argument is the number of the participants we should wait.
        static Barrier barrier = new Barrier(2, x => 
        {
            Console.WriteLine($"Phase {x.CurrentPhaseNumber} is finished!");
        });

        ////For the CountDown example
        //private static int taskCount = 5;

        ////Basically it is the same as barrier except that it is counting down and Signaling and Waiting are in separate methods
        //static CountdownEvent cte = new CountdownEvent(taskCount);
        //static Random random = new Random();

        static void Water()
        {
            Console.WriteLine("Putting Kettle on (waiting water to boil)");
            Thread.Sleep(2000);
            barrier.SignalAndWait(); //1 //First stage. We wait until all threads finish with the first stage
            Console.WriteLine("Pouring water into a cup"); //After all threads finish the first stage we execute that line
            barrier.SignalAndWait(); // Second stage. After line above is executed we enter and wait here until all tasks complete their second stage.
            Console.WriteLine("Putting the kettle away");

        }

        static void Cup()
        {
            Console.WriteLine("Finding a cup (fast operation)");
            barrier.SignalAndWait(); // 1 //First stage. We wait until all threads finish with the first stage
            Console.WriteLine("Adding tea");
            barrier.SignalAndWait();
            Console.WriteLine("Adding sugar");
        }

        static void Main(string[] args)
        {
            var water = Task.Factory.StartNew(Water);
            var cup = Task.Factory.StartNew(Cup);

            var tea = Task.Factory.ContinueWhenAll(new[] { water, cup }, x =>
            {
                Console.WriteLine("Enjoy your cup of tea!");
            });

            try
            {
                tea.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle(x => true);
            }
            ////------------------------------------------------
            ////Example for countDown
            //CountDown();
            ////------------------------------------------------
        }

        //Countdown example
        //static void CountDown()
        //{
        //    var tasks = new Task[taskCount];
        //    for (int i = 0; i < taskCount; i++)
        //    {
        //        tasks[i] = Task.Factory.StartNew(() =>
        //        {
        //            Console.WriteLine($"Entering task {Task.CurrentId}.");
        //            Thread.Sleep(random.Next(3000)); 
        //            cte.Signal(); // also takes a signalcount
        //                          //cte.CurrentCount/InitialCount
        //            Console.WriteLine($"Exiting task {Task.CurrentId}.");
        //        });
        //    }

        //    var finalTask = Task.Factory.StartNew(() =>
        //    {
        //        Console.WriteLine($"Waiting for other tasks in task {Task.CurrentId}");
        //        cte.Wait();
        //        Console.WriteLine("All tasks completed.");
        //    });

        //    finalTask.Wait();
        //}
    }
}
