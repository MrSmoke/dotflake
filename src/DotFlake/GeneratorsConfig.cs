namespace DotFlake
{
    using System;
    using Generators.Flake;
    using Microsoft.Extensions.DependencyInjection;
    using Sources.MachineId;
    using Sources.Timing;

    public static class GeneratorsConfig
    {
        public static void RegisterAll()
        {
            IdGeneratorFactory.RegisterGenerator<FlakeGenerator, FlakeGeneratorConfig>("flake",
                (provider, config) =>
                {
                    var options = new FlakeGeneratorOptions();

                    if(config == null)
                        config = new FlakeGeneratorConfig();

                    if (config.SequenceBits.HasValue)
                        options.SequenceBits = config.SequenceBits.Value;

                    if (config.MachineBits.HasValue)
                        options.MachineBits = config.MachineBits.Value;

                    if (config.TimeBits.HasValue)
                        options.TimeBits = config.TimeBits.Value;

                    var timesourceOptions = new StopwatchTimeSourceOptions();
                    var timeSource = new StopwatchTimeSource(provider.GetRequiredService<ISystemClock>(), timesourceOptions);

                    options.TimeSource = timeSource;
                    options.MachineIdSource = provider.GetRequiredService<IMachineIdSource>();

                    return new FlakeGenerator(options);
                });
        }
    }

    public class FlakeGeneratorConfig
    {
        public byte? TimeBits { get; set; }
        public byte? MachineBits { get; set; }
        public byte? SequenceBits { get; set; }

        public DateTimeOffset? Epoch { get; set; }
    }
}
