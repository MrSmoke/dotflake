using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotFlake
{
    internal interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }

    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
