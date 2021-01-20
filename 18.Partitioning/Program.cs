using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace _18.Partitioning
{
    public class Program
    {
        [Benchmark] //[4.019 ms ; 4.170 ms) - Benchmark result
        public void SquareEachValue()
        {
            const int count = 100000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];

            //Ineffective example since delegates are creathed thousands of times. See benchmark below
            Parallel.ForEach(values, x => { results[x] = (int)Math.Pow(x, 2); });

        }

        [Benchmark] //[2.516 ms ; 2.598 ms) - Benchmark result
        public void PartitionSquare()
        {
            const int count = 100000;
            var values = Enumerable.Range(0, count);
            var results = new int[count];


            //Using partitioner optimizes the process since the tasks are separated and no thousands of delegates will be created 
            var parts = Partitioner.Create(0, count, 10000);
            Parallel.ForEach(parts, range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    results[i] = (int)Math.Pow(i, 2);
                }
            });
        }

        static void Main(string[] args)
        {
            //Benchmark configuration needed to work in debug
            var config = new ManualConfig()
                        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                        .AddLogger(ConsoleLogger.Default);

            var summary = BenchmarkRunner.Run<Program>(config);
            Console.WriteLine(summary);
        }
    }
}
