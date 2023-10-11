using Newtonsoft.Json;
using RestSharp;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class ACHStatusEventResponse : BaseEventResponse
    {

        /// <summary>
        /// Merchant information which triggered the event.
        /// </summary>
        public class Merchant
        {
            /// <summary>
            /// Unique identifier for the merchant which triggered the event.
            /// </summary>
            [JsonProperty("merch_key", NullValueHandling = NullValueHandling.Ignore)]
            public string MerchKey { get; set; }
        }

        /// <summary>
        /// Represents check information associated with the ACH event.
        /// </summary>
        public class Check
        {
            /// <summary>
            /// Gets or sets the tracking code for the check.
            /// </summary>
            [JsonProperty("trackingcode", NullValueHandling = NullValueHandling.Ignore)]
            public string TrackingCode { get; set; }
        }

        /// <summary>
        /// Transaction object related to the event.
        /// </summary>
        public class TransactionObject
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
            public string RefNum { get; set; }

            /// <summary>
            /// Merchant assigned order ID
            /// </summary>
            [JsonProperty("orderid", NullValueHandling = NullValueHandling.Ignore)]
            public string OrderId { get; set; }

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
        ///  Object containing old field values before ACH status was updated
        /// </summary>
        public class OldValues
        {
            /// <summary>
            /// Gets or sets the status before the update.
            /// </summary>
            [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
            public string Status { get; set; }

            /// <summary>
            /// Gets or sets the date when the transaction was settled (if applicable).
            /// </summary>
            [JsonProperty("settled", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(ParseStringToDateTimeConvertor))]
            public DateTime? Settled { get; set; }
        }

        /// <summary>
        ///  Object containing new field values after ACH status updated
        /// </summary>
        public class NewValues
        {
            /// <summary>
            /// Gets or sets the updated status.
            /// </summary>
            [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
            public string Status { get; set; }

            /// <summary>
            /// Gets or sets the date when the transaction was settled (if applicable).
            /// </summary>
            [JsonProperty("settled", NullValueHandling = NullValueHandling.Ignore)]
            [JsonConverter(typeof(ParseStringToDateTimeConvertor))]
            public DateTime? Settled { get; set; }
        }

        /// <summary>
        /// Logs what fields changed during the update and displays the old and new values.
        /// </summary>
        public class Changes
        {
            /// <summary>
            /// Gets or sets the old values before the update.
            /// </summary>
            [JsonProperty("old", NullValueHandling = NullValueHandling.Ignore)]
            public OldValues Old { get; set; }

            /// <summary>
            /// Gets or sets the new values after the update.
            /// </summary>
            [JsonProperty("new", NullValueHandling = NullValueHandling.Ignore)]
            public NewValues New { get; set; }
        }
    }

     /// <summary>
    /// Represents the body of an ACH event.
    /// </summary>
    public class EventBody
    {
        /// <summary>
        /// Gets or sets the merchant information triggering the event.
        /// </summary>
        [JsonProperty("merchant", NullValueHandling = NullValueHandling.Ignore)]
        public ACHStatusEventResponse.Merchant Merchant { get; set; }

        /// <summary>
        /// Gets or sets the transaction object related to the event.
        /// </summary>
        [JsonProperty("object", NullValueHandling = NullValueHandling.Ignore)]
        public ACHStatusEventResponse.TransactionObject Object { get; set; }

        /// <summary>
        /// Gets or sets the changes in values during the update.
        /// </summary>
        [JsonProperty("changes", NullValueHandling = NullValueHandling.Ignore)]
        public ACHStatusEventResponse.Changes Changes { get; set; }
    }
}
