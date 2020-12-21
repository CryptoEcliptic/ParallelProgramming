using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace _12.ContinuationsAndChildTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            //var task1 = Task.Factory.StartNew(() =>
            //{
            //    Console.WriteLine("Boiling water!");
            //    Thread.Sleep(2000);
            //});
            ////task2 is a continuation of task1. task2 will always be executed when task1 is executed
            //var task2 = task1.ContinueWith(t =>
            //{
            //    Console.WriteLine($"Completet tasl {t.Id}, then pour water into the cup.");
            //});

            //task2.Wait();


            var task1 = Task.Factory.StartNew(() => "Task1");
            var task2 = Task.Factory.StartNew(() => "Task2");

            //task3 will continue when task1 and task2 are executed. ContinueWhenAll() accepts array of Task and Action as a second param.
            var task3 = Task.Factory.ContinueWhenAll(new[] { task1, task2 }, t => 
            {
                Console.WriteLine("Tasks completed!");
                foreach (var task in t)
                {
                    Console.WriteLine($" - {task.Result}");
                }

                Console.WriteLine("All tasks are done!");
            });
            Task.WaitAll(task3);
        }
    }
}
