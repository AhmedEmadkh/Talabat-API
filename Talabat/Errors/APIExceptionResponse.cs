namespace Talabat.APIs.Errors
{
    public class APIExceptionResponse : APIResponse
    {
        public string? Details { get; set; }

        public APIExceptionResponse(int StatusCode,string? Message = null,string? detials = null):base(StatusCode,Message)
        {
            Details = detials;
        }
    }
}
