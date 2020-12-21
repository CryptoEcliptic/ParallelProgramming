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
            //TASK CONTINUATION EXAMPLE
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


            //var task1 = Task.Factory.StartNew(() => "Task1");
            //var task2 = Task.Factory.StartNew(() => "Task2");

            ////task3 will continue when task1 and task2 are executed. ContinueWhenAll() accepts array of Task and Action as a second param.
            //var task3 = Task.Factory.ContinueWhenAll(new[] { task1, task2 }, t => 
            //{
            //    Console.WriteLine("Tasks completed!");
            //    foreach (var task in t)
            //    {
            //        Console.WriteLine($" - {task.Result}");
            //    }

            //    Console.WriteLine("All tasks are done!");
            //});
            //Task.WaitAll(task3);


            //PARENT AND CHILD TASKS EXAMPLE
            var parent = new Task(() => 
            {
                var child = new Task(() => 
                {
                    Console.WriteLine("Child Task Starting");
                    Thread.Sleep(3000);
                    //throw new Exception();
                    Console.WriteLine("Child Task Finishing");
                }, TaskCreationOptions.AttachedToParent); //If not attached to its parrent, the parrent will not wait its child to complete.

                //Will be executed only if the child task is completed successfully
                var completionHandler = child.ContinueWith(x => 
                {
                    Console.WriteLine($"Task with ID: {x.Id} finished with status {x.Status}");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

                //If an exception occures the fail handler will be executed.
                var failHandler = child.ContinueWith(x =>
                {
                    Console.WriteLine($"Task with ID: {x.Id} finished with status {x.Status}");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);

                child.Start();
            });
            parent.Start();
            try
            {
                parent.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle(x => true);
            }
        }
    }
}
