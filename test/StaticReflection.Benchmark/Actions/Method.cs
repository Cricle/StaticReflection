﻿using BenchmarkDotNet.Attributes;
using StaticReflection.Invoking;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace StaticReflection.Benchmark.Actions
{
    public class A
    {
        public static readonly A Instance = new A();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Call<T>(Student student, ref T arg)
        {
            student.Go(Unsafe.As<T, int>(ref arg));
        }
    }
    [MemoryDiagnoser]
    public class Method
    {
        private Student student;
        private object ostudent;

        private Action<object, object> expression;
        private MethodInfo methodInfo;

        [GlobalSetup]
        public void Setup()
        {
            ostudent = student = new Student();
            var par1 = Expression.Parameter(typeof(object));
            var par2 = Expression.Parameter(typeof(object));
            expression = Expression.Lambda<Action<object, object>>(
                Expression.Call(
                        Expression.Convert(par1, typeof(Student)), typeof(Student).GetMethod(nameof(Student.Go)), Expression.Convert(par2, typeof(object))), par1, par2).Compile(preferInterpretation: true);
            methodInfo = typeof(Student).GetMethod(nameof(Student.Go));
        }

        [Params(5012)]
        public int LoopCount { get; set; }

        [Benchmark(Baseline = true)]
        public void Raw()
        {
            for (int i = 0; i < LoopCount; i++)
                student.Go(null);
        }
        [Benchmark]
        public void ReflectionCall()
        {
            var args = new object[] { null };
            for (int i = 0; i < LoopCount; i++)
                methodInfo.Invoke(student, args);
        }
        [Benchmark]
        public void ExpressionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                expression(student, null);
        }

        [Benchmark]
        public void StaticReflection()
        {
            var m = (IUsualVoidArgs1AnonymousMethod)StudentReflection.Instance.Methods.First(x => x.Name == nameof(Student.Go));
            for (int i = 0; i < LoopCount; i++)
            {
                m.InvokeUsualAnonymous(student, null);
            }
        }
    }
}
