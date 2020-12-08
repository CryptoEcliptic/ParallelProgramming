using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _06.SpinLocking
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
    }
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();

            var bankAccount = new BankAccount();

            //SpinLocking can prevent Dead locks which could appear using lock statement. The spinnlock will spin the thread until we executhe the operation and no other thrads could execute the same operation. So it will prevent race condition.
            SpinLock spLocking = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        //Creating a bool set to false by default.
                        bool lockTaken = false;
                        try
                        {   //If the return result is true that means that the lock has been taken successfully.
                            spLocking.Enter(ref lockTaken);

                            //If we manage to get the lock we execute the operation
                            bankAccount.AddMoney(100);
                        }
                        finally
                        {
                            //If successfully taken the lock should be released.
                            if (lockTaken)
                            {
                                spLocking.Exit();
                            }
                        }

                    }

                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            spLocking.Enter(ref lockTaken);
                            bankAccount.Withdrawl(100);
                        }
                        finally
                        {
                            if (lockTaken)
                            {
                                spLocking.Exit();
                            }
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The Balance in BankAccount is: {bankAccount.Balance}");
        }
    }
}
