using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _09.ConcurrentDictionary
{
    class Program
    {
        private static ConcurrentDictionary<string, string> capitals = new ConcurrentDictionary<string, string>();

        public static void AddCapital()
        {
            //TryAdd() tries to add key and value. The result is bool - if adding is sucessfull returns true : false
            bool isAdded = capitals.TryAdd("France", "Paris");
            string who = Task.CurrentId.HasValue ? ($"Task Id: {Task.CurrentId}") : "Main thread!";

            Console.WriteLine($"{who} {(isAdded ? "added" : "did not add")} the element.");
        }

        static void Main(string[] args)
        {
            AddCapital();

            Task.Factory.StartNew(AddCapital).Wait();

            capitals["Russia"] = "Leningrad";
            //AddOrUpdate() - If the key exests, the value is updated. The function accepts key and returns old value ("Leningrad"). Returns the new value for the key.
            var s = capitals.AddOrUpdate("Russia", "Moscow", (k, old) =>  old + " --> Moscow");
            Console.WriteLine("The capital of Russia is " + capitals["Russia"]);


            capitals["Sweden"] = "Uppsala";
            //GetOrAdd - If the key exists returns its value. If no such key we add key and value("Sweden", "Stockholm")
            var capOfNorway = capitals.GetOrAdd("Sweden", "Stockholm");
            Console.WriteLine($"The capital of Sweden is {capOfNorway}.");

            // removal
            const string toRemove = "Russia"; // make a mistake here

            string removed;
            //TryRemove() - trying to remove the value. Returns true if success. Can the the removed value assigning it to an out variable
            var didRemove = capitals.TryRemove(toRemove, out removed);
            if (didRemove)
            {
                Console.WriteLine($"We just removed {removed}");
            }
            else
            {
                Console.WriteLine($"Failed to remove capital of {toRemove}");
            }

            // some operations are slow, e.g.,
            Console.WriteLine($"The ");

            foreach (var kv in capitals)
            {
                Console.WriteLine($" - {kv.Value} is the capital of {kv.Key}");
            }

            Console.ReadKey();
        }
    }
}
