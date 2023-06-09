﻿using BenchmarkDotNet.Attributes;
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
                        Expression.Convert(par1, typeof(Student)), typeof(Student).GetMethod(nameof(Student.Go)), Expression.Convert(par2, typeof(int))), par1, par2).Compile(preferInterpretation: true);
            methodInfo = typeof(Student).GetMethod(nameof(Student.Go));
        }

        [Params(5012)]
        public int LoopCount { get; set; }

        [Benchmark(Baseline = true)]
        public void Raw()
        {
            for (int i = 0; i < LoopCount; i++)
                student.Go(1);
        }
        [Benchmark]
        public void ReflectionCall()
        {
            var args = new object[] { 1 };
            for (int i = 0; i < LoopCount; i++)
                methodInfo.Invoke(student, args);
        }
        [Benchmark]
        public void ExpressionCall()
        {
            for (int i = 0; i < LoopCount; i++)
                expression(student, 1);
        }

        [Benchmark]
        public void StaticReflection()
        {
            object obji = 1;
            for (int i = 0; i < LoopCount; i++)
            {
                //InvokeUsual(student, 1);
                StudentReflection.StudentGoT0P0MReflection.Instance.InvokeUsualAnonymous(student, obji);
            }
        }
        void InvokeUsual(Student instance, int arg0)
        {
            instance.Go((int)(arg0));
        }
    }
}
