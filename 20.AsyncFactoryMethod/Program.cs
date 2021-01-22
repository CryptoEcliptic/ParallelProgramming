using System.Threading.Tasks;

namespace _20.AsyncFactoryMethod
{
    //Since constructors cannot be called asynchroneously i.e. we cannot create an object async, this is a workaround.
    public class Foo
    {
        //Private ctor not allowing the instance to be created outside
        private Foo()
        {

        }

        //Private async method returning the object
        private async Task<Foo> InitAsync()
        {
            await Task.Delay(1000);
            return this;
        }

        //Public method accessed from outside wich returns new instance of the class
        public static Task<Foo> CreateAsync()
        {
            var result = new Foo();
            return result.InitAsync();
        }
    }
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Foo fooAsync = await Foo.CreateAsync();
        }
    }
}
