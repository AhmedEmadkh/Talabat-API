
namespace Talabat.APIs.Errors
{
    public class APIResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public APIResponse(int statusCode,string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "Your Are Not Authorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
