using StaticReflection.Annotions;
using StaticReflection;

namespace WW
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var b=new Student();
            var ss = typeof(Student).FindStaticAssembly();
            Console.WriteLine(ss?.AssemblyFullName);
            var @class = C.Default.FindType("Student")!;
            @class.SetProperty(b, "Id", 1);
            Console.WriteLine("Id: "+@class.GetProperty(b, "Id"));
            var @event = @class.FindEventTransfer("AlreadyGoSchool")!;
            using (var eventScope = @event.CreateScope(b))
            {
                eventScope.Start();
                eventScope.EventTransfed += Instance_EventTransfed;
                var method = @class.FindMethod("GoToSchool")!;
                Console.WriteLine("GoToSchool:" + method.InvokeUsualMethod(b));
            }
            var obj = @class.FindEmptyConstructor();
            var inst = obj.InvokeUsualMethod(null);
            Console.WriteLine(inst);
        }

        private static void Instance_EventTransfed(object? sender, EventTransferEventArgs e)
        {
            Console.WriteLine("EventRaise: " + e.Args[0]);
        }
    }
    [StaticReflection]
    public record class Student
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public event EventHandler<Student>? AlreadyGoSchool;

        public int GoToSchool()
        {
            AlreadyGoSchool?.Invoke(this, this);
            return Id;
        }
    }
    [StaticReflectionAssembly(AssemblyFullName = "Microsoft.VisualBasic.Core, Version=12.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
    public partial class C
    {
    }
}