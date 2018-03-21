namespace DotFlake.Generators
{
    using System;
    using Timing;

    public class FlakeGeneratorOptions
    {
        public byte TimeBits { get; set; } = 41;
        public byte MachineBits { get; set; } = 10;
        public byte SequenceBits { get; set; } = 12;

        public long MachineId { get; set; }
    }

    public class FlakeGenerator : IIdGenerator<long>
    {
        private readonly ITimeSource _timeSource;

        private long _lastTimestamp;
        private long _sequence;

        private readonly long _maskTime;
        private readonly long _maskSequence;

        private readonly int _shiftTime;
        private readonly int _shiftGenerator;

        private readonly long _machineId;

        private readonly object _lock = new object();

        public FlakeGenerator(ITimeSource timeSource, FlakeGeneratorOptions options)
        {
            _timeSource = timeSource;

            _maskTime = GetMask(options.TimeBits);
            _maskSequence = GetMask(options.SequenceBits);

            _shiftTime = options.MachineBits + options.SequenceBits;
            _shiftGenerator = options.SequenceBits;

            _machineId = options.MachineId;
        }

        public long Next()
        {
            lock (_lock)
            {
                var ticks = _timeSource.GetTicks();
                var timestamp = ticks & _maskTime;

                if(timestamp < _lastTimestamp || ticks < 0)
                    throw new Exception("You have travelled through time");

                if (timestamp == _lastTimestamp)
                {
                    if (_sequence < _maskSequence)
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
                    return (timestamp << _shiftTime)
                           + (_machineId << _shiftGenerator)         // GeneratorId is already masked, we only need to shift
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
