using System.Net;

namespace UsaEPay.NET
{
    public class UsaEPayApiException : Exception
    {
        // Intentional public API surface for consumers to inspect error details
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public HttpStatusCode StatusCode { get; }
        public string? ResponseContent { get; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global

        public UsaEPayApiException(string message, HttpStatusCode statusCode, string? responseContent)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }
}
