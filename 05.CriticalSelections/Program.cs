using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _05.CriticalSelections
{
    //Thread safe work using lock statement
    internal class BankAccount
    {
        object locker = new object();

        public int Balance { get; private set; }

        public void AddMoney(int amount)
        {
            //Inside the scope of the lock statement only one thread can run the operation. So no two or more threads could work on the same resource. "locker" is basically an expression of refference type
            lock (locker)
            {
                Balance += amount;
            }
        }

        public void Withdrawl(int amount)
        {
            lock(locker)
            {
                Balance -= amount;
            } 
        }
    }
    //Thread safe work using Interlocked class
    internal class InterlockBankAccount
    {
        private int _balance;

        public int Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        public void AddMoney(int amount)
        {
            //The Interlock prevents two or more threads to work on the same resource. It is the same as the lock statement. But gives additional methods. Add method needs ref. Thats why we need the field _balance.
            Interlocked.Add(ref _balance, amount);
        }

        public void Withdrawl(int amount)
        {
            Interlocked.Add(ref _balance, -amount);

            //Offtopic: The using of MemoryBarrier() guarantees that Process 1 wont be executed afrer Process 2. Everithing before the barrier will be executed first.
            //Process 1
            Thread.MemoryBarrier();
            //Process 2
        }  
    }

    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();

            var bankAccount = new BankAccount();
            var interlockedBankAccount = new InterlockBankAccount();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => {

                    for (int i = 0; i < 100; i++)
                    {
                        bankAccount.AddMoney(100);
                        interlockedBankAccount.AddMoney(100);
                    }
                
                }));

                tasks.Add(Task.Factory.StartNew(() => {

                    for (int i = 0; i < 100; i++)
                    {
                        bankAccount.Withdrawl(100);
                        interlockedBankAccount.Withdrawl(100);
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The Balance in BankAccount is: {bankAccount.Balance}");
            Console.WriteLine($"The Balance in InterlockBankAccount is: {interlockedBankAccount.Balance}");
        }
    }
}
