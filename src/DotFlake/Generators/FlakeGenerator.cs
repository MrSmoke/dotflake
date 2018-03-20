namespace DotFlake.Generators
{
    using System;
    using Timing;

    public class FlakeGenerator : IIdGenerator<long>
    {
        private readonly ITimeSource _timeSource;

        private long _lastTimestamp;
        private long _sequence;

        private const int TimeBits = 41;
        private const int MachineBits = 10;
        private const int SequenceBits = 12;

        private readonly long MASK_TIME = GetMask(TimeBits);
        private readonly long MASK_MACHINE = GetMask(MachineBits);
        private readonly long MASK_SEQUENCE = GetMask(SequenceBits);

        private const int SHIFT_TIME = MachineBits + SequenceBits;
        private const int SHIFT_GENERATOR = SequenceBits;

        private readonly long _machineId = 0;

        private readonly object _lock = new object();

        public FlakeGenerator(ITimeSource timeSource)
        {
            _timeSource = timeSource;
        }

        public long Next()
        {
            lock (_lock)
            {
                var ticks = _timeSource.GetTicks();
                var timestamp = ticks & MASK_TIME;

                if(timestamp < _lastTimestamp || ticks < 0)
                    throw new Exception("You have travelled through time");

                if (timestamp == _lastTimestamp)
                {
                    if (_sequence < MASK_SEQUENCE)
                        ++_sequence;
                    else
                        throw new Exception("Too many ids generated. Try again soon");
                }
                else
                {
                    _sequence = 0;
                    _lastTimestamp = timestamp;
                }

                unchecked
                {
                    return (timestamp << SHIFT_TIME)
                           + (_machineId << SHIFT_GENERATOR)         // GeneratorId is already masked, we only need to shift
                           + _sequence;
                }
            }
        }

        private static long GetMask(byte bits)
        {
            return (1L << bits) - 1;
        }
    }
}
