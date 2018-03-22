namespace DotFlake.Sources.Timing
{
    using System;
    using System.Diagnostics;

    public class StopwatchTimeSource : ITimeSource
    {
        private readonly TimeSpan _offset;
        private readonly TimeSpan _tickDuration;
        private readonly Stopwatch _stopwatch;

        internal StopwatchTimeSource(ISystemClock systemClock, StopwatchTimeSourceOptions options)
        {
            _stopwatch = Stopwatch.StartNew();
            _offset = systemClock.UtcNow - options.Epoch;
            _tickDuration = options.TickDuration;
        }

        public long GetTicks()
        {
            return (_offset.Ticks + _stopwatch.Elapsed.Ticks) / _tickDuration.Ticks;
        }
    }
}