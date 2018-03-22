namespace DotFlake.Sources.Timing
{
    using System;

    public class StopwatchTimeSourceOptions
    {
        public DateTimeOffset Epoch { get; set; } = new DateTime(2018, 1, 1, 0, 0, 0);
        public TimeSpan TickDuration { get; set; } = TimeSpan.FromMilliseconds(1);
    }
}