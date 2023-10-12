using Newtonsoft.Json;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public partial class ACHStatusEvenResponse
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
        /// Describes the type of the event, e.g., "ach.voided."
        /// </summary>
        [JsonProperty("event_type")]
        public string EventType { get; set; }

        /// <summary>
        /// The body of the ACH event. Contains detailed information about the event,
        /// such as merchant details, transaction object, and changes in values.
        /// </summary>
        [JsonProperty("event_body")]
        public EventBody EventBody { get; set; }

        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        [JsonProperty("event_id")]
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
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; }

        /// <summary>
        /// Gets or sets the transaction object related to the event.
        /// </summary>
        [JsonProperty("object")]
        public Object Object { get; set; }

        /// <summary>
        /// Gets or sets the changes in values during the update.
        /// </summary>
        [JsonProperty("changes")]
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
        [JsonProperty("old")]
        public ChangeDetails Old { get; set; }

        /// <summary>
        /// Gets or sets the new values after the update.
        /// </summary>
        [JsonProperty("new")]
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
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date when the transaction was settled (if applicable).
        /// </summary>
        [JsonProperty("processed")]
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
        [JsonProperty("merch_key")]
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
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Unique gateway generated key.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Unique transaction reference number.
        /// </summary>
        [JsonProperty("refnum")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Refnum { get; set; }

        /// <summary>
        /// Merchant assigned order ID
        /// </summary>
        [JsonProperty("orderid")]
        public string Orderid { get; set; }

        /// <summary>
        /// Object which holds all check information
        /// </summary>
        [JsonProperty("check")]
        public Check Check { get; set; }

        /// <summary>
        /// transaction URI.
        /// </summary>
        [JsonProperty("uri")]
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
        [JsonProperty("trackingcode")]
        public string Trackingcode { get; set; }
    }
}
