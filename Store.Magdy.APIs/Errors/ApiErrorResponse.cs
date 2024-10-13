namespace Store.Magdy.APIs.Errors
{
    public class ApiErrorResponse
    {
        public ApiErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            var message = statusCode switch
            {
                400 => "A bad request you have made",
                401 => "Unauthorized",
                404 => "Resource was Not Found",
                500 => "Server Error",
                _ => null
            };

            return message;
        }

    }
}
