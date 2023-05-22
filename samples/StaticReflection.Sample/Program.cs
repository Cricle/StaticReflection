using StaticReflection.Annotions;

//[assembly: StaticReflection(Type = typeof(StaticReflection.Sample.A))]

namespace StaticReflection.Sample
{
    internal class Program
    {
        [StaticReflection]
        [StaticReflection(Type = typeof(B))]
        public A a { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(",", AReflection.Instance.Properties.Select(x => $"{x.PropertyType} {x.Name}")));
        }
    }
    public class B
    {
        public int S1 { get; set; }

        public double Hello1 { get; set; }

        public double Well1 { get; set; }
    }
    public class A
    {
        public int S { get; set; }

        public double Hello { get; set; }

        public double Well { get; set; }
    }
}