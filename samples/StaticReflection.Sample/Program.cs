using StaticReflection.Annotions;
using System.Diagnostics;

namespace StaticReflection.Sample
{
    internal class Program
    {
        [StaticReflection]
        [StaticReflection(Type =typeof(B))]
        public A a { get; set; }

        static void Main(string[] args)
        {
            AReflection.Instance.InvokeMethod("Ax", null);
            Console.WriteLine(string.Join(",", AReflection.Instance.Properties[0].Attributes.Select(x=>x)));
        }
    }

    public class B
    {
        public int S { get; set; }

        public double Hello1 { get; set; }

        public double Well1 { get; set; }
    }
    public class A
    {
        public int S { get; set; }

        public double Hello { get; set; }

        public double Well { get; }

        public static int TT { get; set; }

        public static void Ax()
        {
            Console.WriteLine("Ax");
        }
    }
}