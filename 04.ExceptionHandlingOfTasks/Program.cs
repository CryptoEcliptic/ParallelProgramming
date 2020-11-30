using System;
using System.Threading.Tasks;

namespace _04.ExceptionHandlingOfTasks
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ExceptionRaiser();
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine($"Handled elswhere {e.GetType()}");
                } 
            }
            
            Console.WriteLine("Main program done!");
        }

        private static void ExceptionRaiser()
        {
            var t = Task.Factory.StartNew(() =>
            {
                throw new InvalidOperationException("Cannot do this!") { Source = "t" };
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                throw new AccessViolationException("Cannot do this!") { Source = "t2" };
            });

            //The try catch block assures that an exception will be handled and will not be suppressed since exceptions in the tasks are not propagated if no blocking operations are called. 
            try
            {   //Calling WaitAll() means that exception will be thrown outside the tasks. WaitAll() and Wait() observe for exceptions and should be in try catch block.
                Task.WaitAll(t, t2);
            }
            catch (AggregateException ae) //Aggregate exception gives access to the inner exceptions
            {

                //foreach (var ex in ae.InnerExceptions)
                //{
                //Showing the exceprions thrown in the tasks with proper description
                //    Console.WriteLine($"Exception {ex.GetType()} from {ex.Source}\nMessage: {ex.Message}");
                //    Console.WriteLine();
                //}

                //Below code handles only InvalidOperationException. The other exceprion is propagated and handled in another try catch block. So it's a way to selectively handle particular exceptions
                ae.Handle(x => 
                {
                    if (x is InvalidOperationException)
                    {
                        Console.WriteLine("InvalidOperationException");
                        //Returning true means that the exception is handled
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
        }
    }
}
