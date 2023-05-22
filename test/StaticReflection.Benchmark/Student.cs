using StaticReflection.Annotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticReflection.Benchmark
{
    [StaticReflection]
    public class Student
    {
        [Person(123)]
        public int Id { get; set; }

        public string Name { get; set; }
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
