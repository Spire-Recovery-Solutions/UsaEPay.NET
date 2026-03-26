using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayInventoryLocationRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// The inventory location name specified by the merchant.
        /// </summary>
        [JsonPropertyName("merch_locationid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string MerchLocationId { get; set; }

        /// <summary>
        /// The inventory location name specified by the merchant.
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        /// <summary>
        /// The inventory location description.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Description { get; set; }
    }
}
