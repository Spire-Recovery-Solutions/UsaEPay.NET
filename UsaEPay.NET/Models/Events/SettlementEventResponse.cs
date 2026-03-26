using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Events
{
    public class SettlementEventResponse : BaseEventResponse
    {
        /// <summary>
        /// Gets or sets the body of the settlement event.
        /// </summary>
        [JsonPropertyName("event_body")]
        public SettlementEventBody? EventBody { get; set; }
    }

    /// <summary>
    /// Represents the body of a settlement event.
    /// </summary>
    public class SettlementEventBody : BaseEventBody
    {
        /// <summary>
        /// Gets or sets the details of the settlement event.
        /// </summary>
        [JsonPropertyName("object")]
        public SettlementObject? Object { get; set; }
    }

    /// <summary>
    /// Represents the details of a settlement event.
    /// </summary>
    public class SettlementObject
    {
        /// <summary>
        /// Object type. This will always be batch.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// This is the gateway generated unique identifier for the batch.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        /// <summary>
        /// The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc.
        /// </summary>
        [JsonPropertyName("batchnum")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long BatchNum { get; set; }

        /// <summary>
        /// This is the unique batch identifier. This was originally used in the SOAP API.
        /// </summary>
        [JsonPropertyName("batchrefnum")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long BatchRefNum { get; set; }

        /// <summary>
        /// Response code that indicates if the batch was successfully closed.
        /// </summary>
        [JsonPropertyName("response")]
        public string? Response { get; set; }

        /// <summary>
        /// Total sales amount of the settlement batch.
        /// </summary>
        [JsonPropertyName("totalsales")]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal TotalSales { get; set; }

        /// <summary>
        /// Gets or sets the Total sales count in the settlement batch.
        /// </summary>
        [JsonPropertyName("numsales")]
        public long NumSales { get; set; }

        /// <summary>
        /// Gets or sets the total credits amount of the settlement batch.
        /// </summary>
        [JsonPropertyName("totalcredits")]
        public string? TotalCredits { get; set; }

        /// <summary>
        /// Gets or sets the URI associated with the settlement batch.
        /// </summary>
        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        /// <summary>
        /// Gets or sets the reason for batch failure.
        /// </summary>
        [JsonPropertyName("reason")]
        public string? Reason { get; set; }
    }
}
