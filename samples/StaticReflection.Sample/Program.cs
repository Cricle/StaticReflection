using StaticReflection.Annotions;

namespace StaticReflection.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(a.S);
        }
    }
    [StaticReflection]
    public class A
    {
        public int S { get; set; }

    }
}