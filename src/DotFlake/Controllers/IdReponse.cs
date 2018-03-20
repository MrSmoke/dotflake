namespace DotFlake.Controllers
{
    public class IdReponse<T>
    {
        public T Id { get; set; }

        public IdReponse(T id)
        {
            Id = id;
        }
    }
}