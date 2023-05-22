using StaticReflection.Annotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StaticReflection.Benchmark
{
    [StaticReflection]
    public class Student
    {
        public static double B { get; }

        public event EventHandler<int> Complated;

        [Person(123)]
        public int Id { get; set; }

        public string Name { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Go(int a)
        {
            var q = a++;
            a = q;
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PersonAttribute:Attribute
    {
        public PersonAttribute(int a)
        {
            A = a;
        }

        public int A { get; set; }
    }
}
