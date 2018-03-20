namespace DotFlake
{
    public interface IIdGenerator<out T>
    {
        T Next();
    }
}