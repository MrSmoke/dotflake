namespace DotFlake.Generators.Flake
{
    using Sources.MachineId;
    using Sources.Timing;

    public class FlakeGeneratorOptions
    {
        public byte TimeBits { get; set; } = 41;
        public byte MachineBits { get; set; } = 10;
        public byte SequenceBits { get; set; } = 12;

        public ITimeSource TimeSource { get; set; }
        public IMachineIdSource MachineIdSource { get; set; }
    }
}