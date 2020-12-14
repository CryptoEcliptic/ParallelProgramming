using System;
using System.Collections.Concurrent;
using System.Linq;

namespace _10.ConcurrentQueueAndStack
{
    class Program
    {
        static void Main(string[] args)
        {
            //////Concurrent Queue Example
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
        }
    }
}
