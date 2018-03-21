namespace DotFlake.Exceptions
{
    public class UnknownGeneratorException : DotFlakeException
    {
        public UnknownGeneratorException(string generatorName) : base($"Unknown generator {generatorName}")
        {
        }
    }
}
