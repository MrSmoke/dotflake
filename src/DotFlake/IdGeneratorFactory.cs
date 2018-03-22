namespace DotFlake
{
    using System;
    using System.Collections.Generic;
    using Exceptions;
    using Generators;
    using Microsoft.Extensions.Configuration;

    public class IdGeneratorFactory : IIdGeneratorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly Dictionary<string, Func<object>> _generators = new Dictionary<string, Func<object>>();
        private static readonly Dictionary<string, GeneratorRegistration> GeneratorRegistrations = new Dictionary<string, GeneratorRegistration>();

        public IdGeneratorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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

        public void AddGenerator(string name, IIdGenerator generator)
        {
            _generators.Add(name, generator.Next);
        }

        public static void RegisterGenerator<TGenerator, TConfig>(string generatorType, Func<IServiceProvider, TConfig, TGenerator> instanceCreator) where TGenerator : IIdGenerator
        {
            GeneratorRegistrations.Add(generatorType, new GeneratorRegistration
            {
                ConfigType = typeof(TConfig),
                InstanceCreator = (provider, config) => instanceCreator(provider, (TConfig) config)
            });
        }

        public void CreateInstances(IEnumerable<IConfigurationSection> configs)
        {
            foreach (var config in configs)
            {
                var instanceName = config.Key;
                var configValue = config.Get<GeneratorConfig>();

                if(!GeneratorRegistrations.TryGetValue(configValue.Generator, out var registration))
                    throw new Exception("Unkonwn generator");

                var options = config.GetSection("options").Get(registration.ConfigType);

                var generatorInstance = registration.InstanceCreator(_serviceProvider, options);
                AddGenerator(instanceName, generatorInstance);
            }
        }

        public class GeneratorConfig
        {
            public string Generator { get; set; }
        }

        private class GeneratorRegistration
        {
            public Type ConfigType { get; set; }
            public Func<IServiceProvider, object, IIdGenerator> InstanceCreator { get; set; }
        }
    }
}
