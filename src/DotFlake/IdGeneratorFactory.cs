namespace DotFlake
{
    using System;
    using System.Collections.Generic;
    using Exceptions;

    public class IdGeneratorFactory : IIdGeneratorFactory
    {
        private readonly Dictionary<string, Func<object>> _generators = new Dictionary<string, Func<object>>();

        public object Next(string name)
        {
            if (_generators.TryGetValue(name, out var nextFunc))
                return nextFunc();

            throw new UnknownGeneratorException(name);
        }

        public void AddGenerator(string name, Func<object> nextFunc)
        {
            _generators.Add(name, nextFunc);
        }

        public void AddGenerator<T>(string name, IIdGenerator<T> generator)
        {
            _generators.Add(name, () => generator.Next());
        }
    }
}
