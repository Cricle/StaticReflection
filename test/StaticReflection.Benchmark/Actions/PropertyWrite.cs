using BenchmarkDotNet.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace StaticReflection.Benchmark.Actions
{

    [MemoryDiagnoser]
    public class PropertyWrite
    {
        private Student student;

        private Action<object, object> expression;
        private PropertyInfo propertyInfo;

        [GlobalSetup]
        public void Setup()
        {
            student = new Student();
            var par1 = Expression.Parameter(typeof(object));
            var par2 = Expression.Parameter(typeof(object));
            expression = Expression.Lambda<Action<object, object>>(
                Expression.Call(
                        Expression.Convert(par1, typeof(Student)), typeof(Student).GetProperty(nameof(Student.Name)).SetMethod,
                        Expression.Convert(par2, typeof(string))), par1, par2).Compile();
            propertyInfo = typeof(Student).GetProperty(nameof(Student.Name));
        }

        [Params(5012)]
        public int LoopCount { get; set; }

        [Benchmark(Baseline = true)]
        public void Raw()
        {
            for (int i = 0; i < LoopCount; i++)
                student.Name = i.ToString();
        }
        [Benchmark]
        public void ReflectionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                propertyInfo.SetValue(student, i.ToString());
        }
        [Benchmark]
        public void ExpressionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                expression(student, i.ToString());
        }

        [Benchmark]
        public void StaticReflection()
        {
            var @ref = (IMemberAnonymousInvokeDefine)StudentReflection.Instance.Properties.First(x => x.Name == nameof(Student.Name));
            for (int i = 0; i < LoopCount; i++)
            {
                @ref.SetValueAnonymous(student, i.ToString());
            }
        }
    }
}
