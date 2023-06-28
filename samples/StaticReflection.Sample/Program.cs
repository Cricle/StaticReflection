using StaticReflection.Annotions;
using System.Diagnostics;
using System.ComponentModel;

namespace StaticReflection.Sample
{
    internal class Program
    {
        [StaticReflection]
        [StaticReflection(Type =typeof(B))]
        public A a { get; set; }

        static void Main(string[] args)
        {
            var a=new A();
            //ABxEReflection.Instance.Start(a);
            //ABxEReflection.Instance.EventTransfed += Instance_EventTransfed;
            a.Raise(new B {  S=22});
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
    public class A
    {
        [DefaultValue(12)]
        public int S { get; set; }

        public int Fi;

        public event EventHandler<B> Bx;

        public void Raise(B b)
        {
            Bx?.Invoke(this, b);
        }
    }
}