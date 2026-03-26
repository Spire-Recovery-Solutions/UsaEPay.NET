using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayInventoryRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Gateway generated product identifier.
        /// </summary>
        [JsonPropertyName("product_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ProductKey { get; set; }

        /// <summary>
        /// Unique identifier for warehouse location.
        /// </summary>
        [JsonPropertyName("location_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string LocationKey { get; set; }

        /// <summary>
        /// Quantity on hand for this product/location combination.
        /// </summary>
        [JsonPropertyName("qtyonhand")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string QtyOnHand { get; set; }

        /// <summary>
        /// Quantity on order for this product/location combination.
        /// </summary>
        [JsonPropertyName("qtyonorder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string QtyOnOrder { get; set; }

        /// <summary>
        /// Date product will become available.
        /// </summary>
        [JsonPropertyName("date_available")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DateAvailable { get; set; }
    }
}
