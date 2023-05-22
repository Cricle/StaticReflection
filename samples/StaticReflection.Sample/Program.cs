using StaticReflection.Annotions;

[assembly: StaticReflection(Type = typeof(StaticReflection.Sample.A))]

namespace StaticReflection.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(string.Join(",", AReflection.Instance.Properties.Select(x => $"{x.PropertyType} {x.Name}")));
        }
    }
    public class A
    {
        public int S { get; set; }

        public double Hello { get; set; }

        public double Well { get; set; }
    }
}