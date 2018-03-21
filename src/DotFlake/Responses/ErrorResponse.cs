namespace DotFlake.Responses
{
    public class ErrorResponse
    {
        public string Error { get; set; }

        public ErrorResponse(string errorMessage)
        {
            Error = errorMessage;
        }
    }
}
