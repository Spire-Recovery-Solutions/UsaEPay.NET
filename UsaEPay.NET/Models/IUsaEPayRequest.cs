using RestSharp;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Models
{
    public interface IUsaEPayRequest
    {
        [JsonIgnore]
        public string? Endpoint { get; set; }
        //public Type ResponseType { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }
    }
}
