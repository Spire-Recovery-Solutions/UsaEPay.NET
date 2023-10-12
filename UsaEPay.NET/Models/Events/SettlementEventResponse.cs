using Newtonsoft.Json;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Events
{
    public class SettlementEventResponse : BaseEventResponse
    {
        /// <summary>
        /// Gets or sets the body of the settlement event.
        /// </summary>
        [JsonProperty("event_body")]
        public SettlementEventBody EventBody { get; set; }
    }

    /// <summary>
    /// Represents the body of a settlement event.
    /// </summary>
    public partial class SettlementEventBody : BaseEventBody
    {
        /// <summary>
        /// Gets or sets the details of the settlement event.
        /// </summary>
        [JsonProperty("object")]
        public SettlementObject Object { get; set; }
    }

    /// <summary>
    /// Represents the details of a settlement event.
    /// </summary>
    public partial class SettlementObject
    {
        /// <summary>
        /// Object type. This will always be batch.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// This is the gateway generated unique identifier for the batch.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc.
        /// </summary>
        [JsonProperty("batchnum")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Batchnum { get; set; }

        /// <summary>
        /// This is the unique batch identifier. This was originally used in the SOAP API.
        /// </summary>
        [JsonProperty("batchrefnum")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Batchrefnum { get; set; }

        /// <summary>
        /// Response code that indicates if the batch was successfully closed.
        /// </summary>
        [JsonProperty("response")]
        public string Response { get; set; }

        /// <summary>
        /// Total sales amount of the settlement batch.
        /// </summary>
        [JsonProperty("totalsales")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Totalsales { get; set; }

        /// <summary>
        /// Gets or sets the Total sales count in the settlement batch.
        /// </summary>
        [JsonProperty("numsales")]
        public long Numsales { get; set; }

        /// <summary>
        /// Gets or sets the total credits amount of the settlement batch.
        /// </summary>
        [JsonProperty("totalcredits")]
        public string Totalcredits { get; set; }

        /// <summary>
        /// Gets or sets the URI associated with the settlement batch.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}