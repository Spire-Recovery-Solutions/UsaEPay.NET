using System.Net;

namespace UsaEPay.NET
{
    public class UsaEPayApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? ResponseContent { get; }

        public UsaEPayApiException(string message, HttpStatusCode statusCode, string? responseContent)
            : base(message)
        {
            StatusCode = statusCode;
            ResponseContent = responseContent;
        }
    }
}
