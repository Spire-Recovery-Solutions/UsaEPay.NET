using Newtonsoft.Json;

namespace UsaEPay.NET.Models.Events
{
    public partial class CardUpdateEventResponse : BaseEventResponse
    {
        /// <summary>
        /// The body of the Card Update event. Contains detailed information about the event,
        /// such as merchant details, transaction object, and changes in values.
        /// </summary>
        [JsonProperty("event_body")]
        public CardEventBody EventBody { get; set; }
    }

    public partial class CardEventBody : BaseEventBody
    {

        /// <summary>
        /// Card Update object related to the event.
        /// </summary>
        [JsonProperty("object")]
        public CardObject Object { get; set; }

        /// <summary>
        /// Logs what fields changed during the update and displays the old and new values.
        /// </summary>
        [JsonProperty("changes")]
        public CardChanges Changes { get; set; }
    }

    public partial class CardChanges
    {
        /// <summary>
        /// Object containing old field values before Card Update status was updated. Will show the status field.
        /// </summary>
        [JsonProperty("old")]
        public CardChangeDetails Old { get; set; }

        /// <summary>
        /// Object containing the same fields as the old object above with the updated field values.
        /// </summary>
        [JsonProperty("new")]
        public CardChangeDetails New { get; set; }
    }

    public partial class CardChangeDetails
    {
        /// <summary>
        /// Logs what fields changed during the update and displays the old and new values.
        /// </summary>
        [JsonProperty("cardaccount_closed")]
        public object CardaccountClosed { get; set; }
    }

    public partial class CardObject
    {
        /// <summary>
        /// Object type. This will always be cardupdate.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gateway generated cardupdate identifier.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Masked original card data including: number, expiration, type.
        /// </summary>
        [JsonProperty("original_card")]
        public CardOriginal OriginalCard { get; set; }

        /// <summary>
        /// The date and time the card update was created. Formatting is "YYYY-MM-DD HH:MM:SS."
        /// </summary>
        [JsonProperty("added")]
        public DateTimeOffset Added { get; set; }

        /// <summary>
        /// Card updater status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Object containing information about where the card update request was initiated.
        /// </summary>
        [JsonProperty("source")]
        public CardSource Source { get; set; }
    }

    public partial class CardOriginal
    {
        /// <summary>
        /// Card number before the update.
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }

        /// <summary>
        /// Expiration date before the update.
        /// </summary>
        [JsonProperty("expiration")]
        public string Expiration { get; set; }

        /// <summary>
        /// Type of the card before the update.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
    }


    public partial class CardSource
    {
        /// <summary>
        /// Object type of the source.
        /// </summary>
        [JsonProperty("object")]
        public string Object { get; set; }

        /// <summary>
        /// Type of the source.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Unique key of the source.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
    }
}
