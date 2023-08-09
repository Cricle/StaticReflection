using StaticReflection;
using StaticReflection.Annotions;

namespace WW
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var b = new Student();
            var ss = typeof(Student).FindStaticAssembly();
            Console.WriteLine(ss?.AssemblyFullName);
            var @class = C.Default.FindType("Student")!;
            @class.SetProperty(b, "Id", 1);
            Console.WriteLine("Id: " + @class.GetProperty(b, "Id"));
            var @event = @class.FindEventTransfer("AlreadyGoSchool")!;
            using (var eventScope = @event.CreateScope(b))
            {
                eventScope.Start();
                eventScope.EventTransfed += Instance_EventTransfed;
                var method = @class.FindMethod("GoToSchool")!;
                Console.WriteLine("GoToSchool:" + method.InvokeUsualMethod(b));
            }
            var obj = @class.FindEmptyConstructor();
            var inst = obj!.InvokeUsualMethod(null);
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
    [StaticReflection(Type =typeof(System.Uri))]
    [StaticReflectionAssembly(AssemblyName = "System")]
    public partial class C
    {
    }
    [StaticReflectionAssembly]
    public partial class C1
    {
    }
}