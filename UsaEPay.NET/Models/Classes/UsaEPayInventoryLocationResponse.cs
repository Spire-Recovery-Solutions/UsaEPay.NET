using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayInventoryLocationResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Object type. Successful calls will always return "inventorylocation".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gateway generated inventory location (warehouse) identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        /// <summary>
        /// The inventory location name specified by the merchant.
        /// </summary>
        [JsonPropertyName("merch_locationid")]
        public string? MerchLocationId { get; set; }

        /// <summary>
        /// The inventory location name specified by the merchant.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The inventory location description.
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Status of the delete operation. Returned as "success" when a location is deleted.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class UsaEPayInventoryLocationListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The type of object returned. Returns a list.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// The maximum amount of inventory locations that will be included in response.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }

        /// <summary>
        /// The number of inventory locations skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }

        /// <summary>
        /// An array of inventory locations matching the request.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayInventoryLocationResponse[]? Data { get; set; }

        /// <summary>
        /// The total amount of inventory locations, including filtered results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
}
