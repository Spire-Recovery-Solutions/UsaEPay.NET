using Newtonsoft.Json;
using System;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public partial class ACHStatusEvenResponse
    {
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// The timestamp indicating when the event was triggered.
        /// </summary>
        [JsonProperty("event_triggered", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset EventTriggered { get; set; }

        /// <summary>
        /// Describes the type of the event, e.g., "ach.voided."
        /// </summary>
        [JsonProperty("event_type", NullValueHandling = NullValueHandling.Ignore)]
        public string EventType { get; set; }

        /// <summary>
        /// The body of the ACH event. Contains detailed information about the event,
        /// such as merchant details, transaction object, and changes in values.
        /// </summary>
        [JsonProperty("event_body", NullValueHandling = NullValueHandling.Ignore)]
        public EventBody EventBody { get; set; }

        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        [JsonProperty("event_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EventId { get; set; }
    }

    /// <summary>
    /// Represents the body of an ACH event.
    /// </summary>
    public partial class EventBody
    {
        /// <summary>
        /// Gets or sets the merchant information triggering the event.
        /// </summary>
        [JsonProperty("merchant", NullValueHandling = NullValueHandling.Ignore)]
        public Merchant Merchant { get; set; }

        /// <summary>
        /// Gets or sets the transaction object related to the event.
        /// </summary>
        [JsonProperty("object", NullValueHandling = NullValueHandling.Ignore)]
        public Object Object { get; set; }

        /// <summary>
        /// Gets or sets the changes in values during the update.
        /// </summary>
        [JsonProperty("changes", NullValueHandling = NullValueHandling.Ignore)]
        public Changes Changes { get; set; }
    }

    /// <summary>
    /// Logs what fields changed during the update and displays the old and new values.
    /// </summary>
    public partial class Changes
    {
        /// <summary>
        /// Gets or sets the old values before the update.
        /// </summary>
        [JsonProperty("old", NullValueHandling = NullValueHandling.Ignore)]
        public ChangeDetails Old { get; set; }

        /// <summary>
        /// Gets or sets the new values after the update.
        /// </summary>
        [JsonProperty("new", NullValueHandling = NullValueHandling.Ignore)]
        public ChangeDetails New { get; set; }
    }
    /// <summary>
    ///  Object containing old/new field values before ACH status was updated
    /// </summary>
    public partial class ChangeDetails
    {
        /// <summary>
        /// Gets or sets the status before/after the update.
        /// </summary>
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date when the transaction was settled (if applicable).
        /// </summary>
        [JsonProperty("processed", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? Processed { get; set; }
    }

    /// <summary>
    /// Merchant information which triggered the event.
    /// </summary>
    public partial class Merchant
    {
        /// <summary>
        /// Unique identifier for the merchant which triggered the event.
        /// </summary>
        [JsonProperty("merch_key", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchKey { get; set; }
    }

    /// <summary>
    /// Transaction object related to the event.
    /// </summary>
    public partial class Object
    {
        /// <summary>
        /// This object type will always be transaction.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// Unique gateway generated key.
        /// </summary>
        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        /// <summary>
        /// Unique transaction reference number.
        /// </summary>
        [JsonProperty("refnum", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Refnum { get; set; }

        /// <summary>
        /// Merchant assigned order ID
        /// </summary>
        [JsonProperty("orderid", NullValueHandling = NullValueHandling.Ignore)]
        public string Orderid { get; set; }

        /// <summary>
        /// Object which holds all check information
        /// </summary>
        [JsonProperty("check", NullValueHandling = NullValueHandling.Ignore)]
        public Check Check { get; set; }

        /// <summary>
        /// transaction URI.
        /// </summary>
        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        public string Uri { get; set; }
    }

    /// <summary>
    /// Represents check information associated with the ACH event.
    /// </summary>
    public partial class Check
    {
        /// <summary>
        /// Gets or sets the tracking code for the check.
        /// </summary>
        [JsonProperty("trackingcode", NullValueHandling = NullValueHandling.Ignore)]
        public string Trackingcode { get; set; }
    }
}
