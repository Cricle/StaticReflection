using StaticReflection.Annotions;
using System.ComponentModel;

[assembly: StaticReflection(Type = typeof(StaticReflection.Sample.A))]
namespace StaticReflection.Sample
{
    internal class Program
    {
        [StaticReflection(Type =typeof(B))]
        public A a { get; set; }

        static void Main(string[] args)
        {
            var attrs = AReflection.Instance.Attributes;
        }

        private static void Instance_EventTransfed(object? sender, EventTransferEventArgs e)
        {
            foreach (var item in e.Args)
            {
                Console.WriteLine(item);
            }
        }
    }

    public record class B
    {
        public int S { get; set; }

        public double Hello1 { get; set; }

        public double Well1 { get; set; }
    }
    [DataObject(false)]
    public class A
    {
        public A()
        {

        }
        [AmbientValue("aa")]
        public A(int s)
        {

        }

        [DefaultValue(12)]
        public int S { get; set; }

        public int Fi;

        [DefaultValue(12)]
        private int W { get; set; }

        public event EventHandler<B>? Bx;

        public void Raise(B b)
        {
            Bx?.Invoke(this, b);
        }
    }
    [StaticReflectionAssembly]
    public partial class C
    {
    }
}