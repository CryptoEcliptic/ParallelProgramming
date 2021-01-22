using System;
using System.Threading.Tasks;

namespace _21.AsyncInitializationPattern
{
    //For DI container Interface could be used for async initialization.
    public interface IAsyncInit
    {
        Task InitializeTask { get; }
    }

    public class MyClass : IAsyncInit
    {
        public MyClass()
        {
            InitializeTask = InitalizeAsync();
        }

        public Task InitializeTask { get; }

        private async Task InitalizeAsync()
        {
            await Task.Delay(1000);
        }
    
    }
    class Program
    {
        public static async Task Main(string[] args)
        {
            var myClass = new MyClass();

            if (myClass is IAsyncInit ia)
            {
                await ia.InitializeTask;
            }
        }
    }
}
