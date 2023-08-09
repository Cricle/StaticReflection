using BenchmarkDotNet.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace StaticReflection.Benchmark.Actions
{
    [MemoryDiagnoser]
    public class PropertyGet
    {
        private Student student;

        private Func<object, object> expression;
        private PropertyInfo propertyInfo;

        [GlobalSetup]
        public void Setup()
        {
            student = new Student();
            var par1 = Expression.Parameter(typeof(object));
            expression = Expression.Lambda<Func<object, object>>(
                Expression.Convert(
                    Expression.Call(
                        Expression.Convert(par1, typeof(Student)), typeof(Student).GetProperty(nameof(Student.Name)).GetMethod), typeof(object)), par1).Compile();
            propertyInfo = typeof(Student).GetProperty(nameof(Student.Name));
        }

        [Params(5012)]
        public int LoopCount { get; set; }

        [Benchmark(Baseline = true)]
        public void Raw()
        {
            for (int i = 0; i < LoopCount; i++)
                _ = student.Name;
        }
        [Benchmark]
        public void ReflectionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                _ = propertyInfo.GetValue(student);
        }
        [Benchmark]
        public void ExpressionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                _ = expression(student);
        }

        [Benchmark]
        public void StaticReflection()
        {
            var @ref = (IMemberAnonymousInvokeDefine)StudentReflection.Instance.Properties.First(x => x.Name == nameof(Student.Name));
            for (int i = 0; i < LoopCount; i++)
            {
                _ = @ref.GetValueAnonymous(student);
            }
        }
    }
}
