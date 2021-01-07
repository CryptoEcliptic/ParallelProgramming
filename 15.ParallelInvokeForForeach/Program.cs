using System;
using System.Threading.Tasks;

namespace _15.ParallelInvokeForForeach
{
    class Program
    {
        static void Main(string[] args)
        {
            //Parallel class accepts parrallel options where we could specify max thread number, cancellation token etc.
            var options = new ParallelOptions();
            options.MaxDegreeOfParallelism = 15;

            //Parallel Invoke
            var a = new Action(() => Console.WriteLine($"First: {Task.CurrentId}"));
            var b = new Action(() => Console.WriteLine($"Second: {Task.CurrentId}"));
            var c = new Action(() => Console.WriteLine($"Third: {Task.CurrentId}"));

            //All actions are invoked on a separate thread. All the threads are waited to complete.
            Parallel.Invoke(a, b, c);


            //Parallel For
            //This is a parallel for loop. Accepts start index, end index and an action.
            //All actions in the loop will be executed on a separate thread. And we are waiting all the threads to complete.
            //Parallel accepts parrallel options
            //Should be kept in mind that the execution order is not specific.
            //Should be kept in mind that the step is 1. 
            Parallel.For(1, 11, options, x => 
            {
                Console.WriteLine($"{x*x}\t");
            });


            //Parallel Foreach
            string[] words = new string[] { "oh", "what", "a", "night" };
            Parallel.ForEach(words, word =>
            {
                Console.WriteLine($"{word} has length of {word.Length} (task: {Task.CurrentId})");
            });
        }
    }
}
