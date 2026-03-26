using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayPayRequestResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Object type. This will always be request.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Unique request identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// The expiration is the date/time the request will expire and no longer be valid.
        /// </summary>
        [JsonPropertyName("expiration")]
        public string Expiration { get; set; }

        /// <summary>
        /// The status of the request.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Once the transaction has been processed, a transaction object will be added to the result.
        /// </summary>
        [JsonPropertyName("transaction")]
        public UsaEPayResponse Transaction { get; set; }

        /// <summary>
        /// Confirms if transaction is complete. Value will be: true or false.
        /// </summary>
        [JsonPropertyName("complete")]
        public bool? Complete { get; set; }
    }
}
