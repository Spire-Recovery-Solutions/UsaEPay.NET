﻿using Newtonsoft.Json;

namespace UsaEPay.NET.Models.Events
{
    public partial class BaseEventResponse
    {
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// The timestamp indicating when the event was triggered.
        /// </summary>
        [JsonProperty("event_triggered")]
        public DateTimeOffset EventTriggered { get; set; }

        /// <summary>
        /// Describes the type of the event.
        /// </summary>
        [JsonProperty("event_type")]
        public string EventType { get; set; }

        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        [JsonProperty("event_id")]
        public string EventId { get; set; }
    }

    public partial class BaseEventBody
    {
        /// <summary>
        /// Merchant information which triggered the event.
        /// </summary>
        [JsonProperty("merchant")]
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
        [JsonProperty("merch_key")]
        public string MerchKey { get; set; }
    }
}
