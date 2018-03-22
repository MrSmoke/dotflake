namespace DotFlake
{
    using System;
    using System.Collections.Generic;
    using Generators;
    using Microsoft.Extensions.Configuration;

    public interface IIdGeneratorFactory
    {
        object Next(string name);
        void AddGenerator(string name, Func<object> nextFunc);
        void AddGenerator(string name, IIdGenerator generator);
        void CreateInstances(IEnumerable<IConfigurationSection> configs);
    }
}