using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;
using UsaEPay.NET.Models.Enumerations.Event;

namespace UsaEPay.NET.Models.Events
{
    public partial class BaseEventResponse
    {
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The timestamp indicating when the event was triggered.
        /// </summary>
        [JsonPropertyName("event_triggered")]
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? EventTriggered { get; set; }

        /// <summary>
        /// Describes the type of the event.
        /// </summary>
        [JsonPropertyName("event_type")]
        [JsonConverter(typeof(EventTypeConverter))]
        public EventType EventType { get; set; }

        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        [JsonPropertyName("event_id")]
        public string EventId { get; set; }
    }

    public partial class BaseEventBody
    {
        /// <summary>
        /// Merchant information which triggered the event.
        /// </summary>
        [JsonPropertyName("merchant")]
        public Merchant Merchant { get; set; }
    }

    /// <summary>
    /// Merchant information which triggered the event.
    /// </summary>
    public partial class Merchant
    {
        /// <summary>
        /// Unique identifier for the merchant which triggered the event.
        /// </summary>
        [JsonPropertyName("merch_key")]
        public string MerchKey { get; set; }
    }
}