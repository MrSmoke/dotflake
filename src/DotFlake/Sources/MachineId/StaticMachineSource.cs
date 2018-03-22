namespace DotFlake.Sources.MachineId
{
    public class StaticMachineSource : IMachineIdSource
    {
        private readonly long _machineId;

        public StaticMachineSource(long machineId)
        {
            _machineId = machineId;
        }

        public long GetMachineId()
        {
            return _machineId;
        }
    }
}