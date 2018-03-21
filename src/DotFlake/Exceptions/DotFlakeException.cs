namespace DotFlake.Exceptions
{
    using System;

    public class DotFlakeException : Exception
    {
        public DotFlakeException(string message) : base(message)
        {
        }
    }
}