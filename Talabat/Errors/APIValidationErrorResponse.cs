namespace Talabat.APIs.Errors
{
    public class APIValidationErrorResponse : APIResponse
    {
        public IEnumerable<string> Error { get; set; }
        public APIValidationErrorResponse() : base(400)
        {
            Error = new List<string>();
        }

    }
}
