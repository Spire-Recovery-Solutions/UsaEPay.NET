using RestSharp;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayGetRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string? Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; } = Method.Get;
    }
}
