namespace DotFlake
{
    using System;

    public interface IIdGeneratorFactory
    {
        object Next(string name);
        void AddGenerator(string name, Func<object> nextFunc);
        void AddGenerator<T>(string name, IIdGenerator<T> generator);
    }
}