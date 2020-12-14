using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _11.BlockingCollection
{
    class Program
    {
        //BlockingCollection is a wrapper whish implements IProducerConsumerCollection and inicializes collection which implements the same interface. BlockingCollection has bound catapity which sets limit of the number of elements we could add. This limit prevents overflowing. In that case collection will block if we try adding 11th element until the ProduceAndConsume method take one element and remove it from the collection. 
        static BlockingCollection<int> messages = new BlockingCollection<int>(new ConcurrentBag<int>(), 10);

        //For cancelling the tasks when needed.
        static CancellationTokenSource cts = new CancellationTokenSource();

        static Random random = new Random();
        static void Main(string[] args)
        {
            Task.Factory.StartNew(ProduceAndConsume, cts.Token);

            Console.ReadKey();
            cts.Cancel();
        }
        public static void ProduceAndConsume()
        {
            var producer = Task.Factory.StartNew(RunProducer);
            var consumer = Task.Factory.StartNew(RunConsumer);

            try
            {
                Task.WaitAll(new[] { producer, consumer }, cts.Token);
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => true);
            }
        }

        private static void RunConsumer()
        {
            //GetConsumingEnumerable provides enumeration. If there is an element, it will be taken. 
            foreach (var item in messages.GetConsumingEnumerable())
            {
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"-{item}");
                Thread.Sleep(random.Next(1000));
            }
        }

        private static void RunProducer()
        {
            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();
                int i = random.Next(100);

                messages.Add(i);
                Console.WriteLine($"+{i}\t");
                Thread.Sleep(random.Next(50));
            }
        }
    }
}
