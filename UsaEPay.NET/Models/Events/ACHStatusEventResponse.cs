using Newtonsoft.Json;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Events
{
    public partial class ACHStatusEvenResponse : BaseEventResponse
    {

        /// <summary>
        /// The body of the ACH event. Contains detailed information about the event,
        /// such as merchant details, transaction object, and changes in values.
        /// </summary>
        [JsonProperty("event_body")]
        public ACHEventBody EventBody { get; set; }
    }

    /// <summary>
    /// Represents the body of an ACH event.
    /// </summary>
    public partial class ACHEventBody : BaseEventBody
    {

        /// <summary>
        /// Gets or sets the transaction object related to the event.
        /// </summary>
        [JsonProperty("object")]
        public ACHObject Object { get; set; }

        /// <summary>
        /// Gets or sets the changes in values during the update.
        /// </summary>
        [JsonProperty("changes")]
        public ACHChanges Changes { get; set; }
    }

    /// <summary>
    /// Logs what fields changed during the update and displays the old and new values.
    /// </summary>
    public partial class ACHChanges
    {
        /// <summary>
        /// Gets or sets the old values before the update.
        /// </summary>
        [JsonProperty("old")]
        public ACHChangeDetails Old { get; set; }

        /// <summary>
        /// Gets or sets the new values after the update.
        /// </summary>
        [JsonProperty("new")]
        public ACHChangeDetails New { get; set; }
    }
    /// <summary>
    ///  Object containing old/new field values before ACH status was updated
    /// </summary>
    public partial class ACHChangeDetails
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
    /// Transaction object related to the event.
    /// </summary>
    public partial class ACHObject
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
        public ACHCheck Check { get; set; }

        /// <summary>
        /// transaction URI.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }

    /// <summary>
    /// Represents check information associated with the ACH event.
    /// </summary>
    public partial class ACHCheck
    {
        /// <summary>
        /// Gets or sets the tracking code for the check.
        /// </summary>
        [JsonProperty("trackingcode")]
        public string Trackingcode { get; set; }
    }
}
