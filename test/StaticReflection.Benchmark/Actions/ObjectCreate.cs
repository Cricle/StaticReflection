using BenchmarkDotNet.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace StaticReflection.Benchmark.Actions
{
    [MemoryDiagnoser]
    public class ObjectCreate
    {

        private Func<object> expression;
        private ConstructorInfo propertyInfo;
        private StudentReflection.Student0CReflection c;

        [GlobalSetup]
        public void Setup()
        {
            expression = Expression.Lambda<Func<object>>(
                Expression.New(typeof(Student))).Compile();
            propertyInfo = typeof(Student).GetConstructor(Type.EmptyTypes);
            c = StudentReflection.Student0CReflection.Instance;
        }

        [Params(5012)]
        public int LoopCount { get; set; }

        [Benchmark(Baseline = true)]
        public void Raw()
        {
            for (int i = 0; i < LoopCount; i++)
                _ = new Student();
        }
        [Benchmark]
        public void ReflectionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                propertyInfo.Invoke(Array.Empty<object>());
        }
        [Benchmark]
        public void ExpressionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                expression();
        }

        [Benchmark]
        public void StaticReflection()
        {
            for (int i = 0; i < LoopCount; i++)
            {
                c.Invoke(null);
            }
        }
    }
}
