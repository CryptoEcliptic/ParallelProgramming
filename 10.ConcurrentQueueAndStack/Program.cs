using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _10.ConcurrentQueueAndStack
{
    class Program
    {
        static void Main(string[] args)
        {
            //////Concurrent Queue Example
            Console.WriteLine("Concurrent Queue example:");
            var q = new ConcurrentQueue<int>();
            q.Enqueue(1);
            q.Enqueue(2);

            // Queue: 2 1 <- front
            int resultQueue;
            //int last = q.Dequeue();
            if (q.TryDequeue(out resultQueue))
            {
                Console.WriteLine($"Removed element {resultQueue}");
            }

            // Queue: 2
            //int peeked = q.Peek();
            if (q.TryPeek(out resultQueue))
            {
                Console.WriteLine($"Last element is {resultQueue}");
            }

            Console.WriteLine();

            //////Concurrent Stack Example
            Console.WriteLine("Concurrent stack example:");
            var stack = new ConcurrentStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);

            int resultStack;
            if (stack.TryPeek(out resultStack))
            {
                Console.WriteLine($"{resultStack} is on top");
            }

            if (stack.TryPop(out resultStack))
            {
                Console.WriteLine($"Popped {resultStack}");
            }

            var items = new int[5];
            if (stack.TryPopRange(items, 0, 5) > 0) // actually pops only 3 items
            {
                var text = string.Join(", ", items.Select(i => i.ToString()));
                Console.WriteLine($"Popped these items: {text}");
            }
            Console.WriteLine();
            //////////Concurrent Bag
            //Unlike Queue and Stack, the Concurrent Bag is unordered collection.
            Console.WriteLine("Concurrent bag example:");
            var concurrentBag = new ConcurrentBag<int>();

            var tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                var i1 = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    
                    concurrentBag.Add(i1);
                    Console.WriteLine($"{Task.CurrentId} just added {i1}.");

                    int result;
                    if (concurrentBag.TryPeek(out result))
                    {
                        Console.WriteLine($"{Task.CurrentId} has peeked {result}");
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            //We have no guarantee which element will be taken since the collection is unordered.
            int last;
            if (concurrentBag.TryTake(out last))
            {
                Console.WriteLine($"The taken value is: {last}");
            }

        }
    }
}
