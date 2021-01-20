using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _19.AsParallelParallelQuery
{
    public class Program
    {
        static void Main(string[] args)
        {

            const int count = 50;

            var items = Enumerable.Range(1, count);
            var result = new int[count];

            //The result will be executed in parallel but will not be ordered in a particular way.
            items.AsParallel()
                .ForAll(x =>
            {
                int newValue = x * x;

                Console.WriteLine($"newValue = {newValue} TaskId: {Task.CurrentId}\t");
                result[x - 1] = newValue;
            });
            Console.WriteLine();

            foreach (var res in result)
            {
                Console.WriteLine(res);
            }

            ////////////// ParallelQuery ////////////
            //The items will be ordered. However the actual evaluation of the result will happen after the ParallelQuery is materialized.
            //Before materialization "orderedInParallel" is just a plan of execution
            ParallelQuery<int> orderedInParallel = items.AsParallel().AsOrdered().Select(x => x * x);

            //Materialization of "orderedInParallel"
            int[] materializedCollection = orderedInParallel.ToArray();

            foreach (var num in orderedInParallel)
            {
                Console.WriteLine(num);
            }


            ///////Cancelling And Exception Handling//////
            var cts = new CancellationTokenSource();
            var items1 = Enumerable.Range(1, 20);

            var results = items1.AsParallel()
              .WithCancellation(cts.Token) //Using cancellation token to cancel the task on particular condition.
              .Select(i =>
              {
                  double result = Math.Log10(i);

                  //if (result > 1) throw new InvalidOperationException();

                  Console.WriteLine($"i = {i}, tid = {Task.CurrentId}");
                  return result;
              });

            try
            {
                foreach (var c in results)
                {
                    if (c > 1)
                    {
                        cts.Cancel();
                    }
                    Console.WriteLine($"result = {c}");
                }
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                    return true;
                });
            }
            catch (OperationCanceledException) //Needed when using cancellation token. Otherwise the exception will be unhandled.
            {
                Console.WriteLine($"Canceled");
            }
        }
    }
}

