using BenchmarkDotNet.Running;

namespace StaticReflection.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run();
        }
    }
}