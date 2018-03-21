using System;

namespace DotFlake.Tests.Performance
{
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;

    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<IdGeneratorFactoryBenchmarks>();
        }
    }

    public class DummyLongGenerator : IIdGenerator<long>
    {
        public long Next()
        {
            return 1234567L;
        }
    }

    public class DummyStringGenerator : IIdGenerator<string>
    {
        public string Next()
        {
            return "Hello world";
        }
    }

    public class IdGeneratorFactoryBenchmarks
    {
        private IIdGeneratorFactory _generatorFactory;

        [GlobalSetup]
        public void Setup()
        {
            _generatorFactory = new IdGeneratorFactory();

            _generatorFactory.AddGenerator("test", new DummyLongGenerator());
            _generatorFactory.AddGenerator("testString", new DummyStringGenerator());
        }

        [Benchmark]
        public object Next()
        {
            return _generatorFactory.Next("test");
        }
    }
}
