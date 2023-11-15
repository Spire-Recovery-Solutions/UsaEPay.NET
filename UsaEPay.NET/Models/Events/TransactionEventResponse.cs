using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Models.Events
{
    public partial class TransactionEventResponse : BaseEventResponse
    {
        /// <summary>
        /// Gets or sets the body of the transaction event.
        /// </summary>
        [JsonPropertyName("event_body")]
        public TransactionEventBody EventBody { get; set; }
    }

    /// <summary>
    /// Represents the body of a transaction event, inheriting from the base event body.
    /// </summary>
    public partial class TransactionEventBody : BaseEventBody
    {
        /// <summary>
        /// Transaction object related to the event. Will be similar to the response 
        /// when the transaction is processed through the REST API.
        /// </summary>
        [JsonPropertyName("object")]
        public UsaEPayResponse Transaction { get; set; }
    }
}
