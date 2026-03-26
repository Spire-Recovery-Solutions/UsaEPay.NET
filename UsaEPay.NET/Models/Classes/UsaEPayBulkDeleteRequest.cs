using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayBulkDeleteRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string? Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        [JsonPropertyName("keys")]
        public string[]? Keys { get; set; }
    }
}
