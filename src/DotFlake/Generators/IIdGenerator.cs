namespace DotFlake.Generators
{
    public interface IIdGenerator : IIdGenerator<object>
    {

    }

    public interface IIdGenerator<out T>
    {
        T Next();
    }
}