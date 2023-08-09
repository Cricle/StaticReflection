using StaticReflection.Annotions;
using System.Runtime.CompilerServices;

namespace StaticReflection.Benchmark
{
    [StaticReflection]
    public class Student
    {
        public static double B { get; }


        [Person(123)]
        public int Id { get; set; }

        public string Name { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Go(object a)
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PersonAttribute : Attribute
    {
        public PersonAttribute(int a)
        {
            A = a;
        }

        public int A { get; set; }
    }
}
