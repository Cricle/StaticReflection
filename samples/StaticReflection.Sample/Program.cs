using StaticReflection.Annotions;
using System.Diagnostics;
using System.ComponentModel;
using System.Text.Json.Serialization;


[assembly:StaticReflection(Type= typeof(StaticReflection.Sample.A))]
namespace StaticReflection.Sample
{
    internal class Program
    {
        [StaticReflection(Type =typeof(B))]
        public A a { get; set; }

        static void Main(string[] args)
        {
            var a=new A();
            var evS = ((IEventTransfer)AReflection.Instance.Events[0]);
            evS.Start(a);
            evS.EventTransfed += Instance_EventTransfed;
            //ABxEReflection.Instance.Start(a);
            //ABxEReflection.Instance.EventTransfed += Instance_EventTransfed;
            a.Raise(new B {  S=22});
            foreach (var item in C.Default.Types[0].Constructors)
            {
                Console.WriteLine(item.Name);
            }
        }

        private static void Instance_EventTransfed(object? sender, EventTransferEventArgs e)
        {
            foreach (var item in e.Args)
            {
                Console.WriteLine(item);
            }
        }
    }

    [JsonSerializable(typeof(B))]
    public partial class Bc:JsonSerializerContext
    {

    }
    public record class B
    {
        public int S { get; set; }

        public double Hello1 { get; set; }

        public double Well1 { get; set; }
    }
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

        public event EventHandler<B> Bx;

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