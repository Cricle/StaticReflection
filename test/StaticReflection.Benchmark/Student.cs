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
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
