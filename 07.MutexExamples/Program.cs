using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _07.MutexExamples
{
    internal class BankAccount
    {
        public int Balance { get; private set; }

        public void AddMoney(int amount)
        {
            Balance += amount;
        }

        public void Withdrawl(int amount)
        {
            Balance -= amount;
        }

        public void Transfer(BankAccount where, int amount)
        {
            Balance -= amount;
            where.Balance +=amount;

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();

            var bankAccount = new BankAccount();
            var bankAccount2 = new BankAccount();

            //Controls the access to different parts of the code.
            Mutex mutex = new Mutex();
            Mutex mutex2 = new Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {   //Return true/false depending we have the mutex available. Timeout value can be provided in the ctor of WaitOne() - we can wait the mutex for some time to become available.
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            bankAccount.AddMoney(1);
                        }
                        finally
                        {
                            if (haveLock)
                            {   //If we have the mutex available here we release it
                                mutex.ReleaseMutex();
                            }
                        }
                    }

                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {
                            bankAccount2.AddMoney(1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        //Since we are going to have operations in two different accounts, we need to be sure that their two mutexes used in the above opperations are released. 
                        bool haveLock = WaitHandle.WaitAll(new[] { mutex, mutex2 });

                        try
                        {
                            bankAccount.Transfer(bankAccount2, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {   //If we have been able to use the two mutexes we should release them.
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }


                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The Balance in BankAccount1 is: {bankAccount.Balance}");
            Console.WriteLine($"The Balance in BankAccount2 is: {bankAccount2.Balance}");
        }
    }
}
