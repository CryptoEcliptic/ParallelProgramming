using System;
using System.Threading.Tasks;

namespace _02.CreatingAndStartingTasks
{
    class Program
    {
        public static void Write(char ch)
        {
            int i = 1000;

            while (i -- > 0)
            {
                Console.Write(ch);
            }
        }

        public static void Write(object o)
        {
            int i = 1000;

            while (i-- > 0)
            {
                Console.Write(o);
            }
        }

        public static int TextLength(object o)
        {
            Console.WriteLine($"\nTask with {Task.CurrentId} processing object {o}...");
            return o.ToString().Length;
        }

        static void Main(string[] args)
        {
            ////Invoking the first overload
            //Task.Factory.StartNew(() => Write('.'));

            //var t = new Task(() => Write('?'));
            //t.Start();

            //Write('-');

            ////Invoking the second overload
            //Task.Factory.StartNew(Write, "Hello");
            //var t1 = new Task(Write, 666);
            //t1.Start();

            string text1 = "text1", text2 = "text2+++";

            var t1 = new Task<int>(TextLength, text1);
            t1.Start();

            Task<int> t2 = Task.Factory.StartNew(TextLength, text2);

            Console.WriteLine($"The length of {text1} is {t1.Result}");
            Console.WriteLine($"The length of {text2} is {t2.Result}");

            Console.WriteLine("Main program done, press any key.");
            Console.ReadKey();
        }
    }
}
