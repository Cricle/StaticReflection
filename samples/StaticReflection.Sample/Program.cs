using StaticReflection.Annotions;

namespace StaticReflection.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var b=new Student();
            var @class=C.Default.Types.First(x => x.Name == "Student");
            @class.SetProperty(b, "Id", 1);
            Console.WriteLine("Id: "+@class.GetProperty(b, "Id"));
            var @event = (IEventTransfer)@class.Events.First(x => x.Name == "AlreadyGoSchool");
            using (var eventScope = @event.CreateScope(b))
            {
                eventScope.Start();
                eventScope.EventTransfed += Instance_EventTransfed;
                var method = @class.Methods.First(x => x.Name == "GoToSchool");
                Console.WriteLine("GoToSchool:" + method.InvokeUsualMethod(b));
            }
            var obj = @class.Constructors.First(x => x.ArgumentTypes.Count == 0);
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
    [StaticReflectionAssembly]
    public partial class C
    {
    }
}