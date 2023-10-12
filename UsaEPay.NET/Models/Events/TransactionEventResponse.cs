using Newtonsoft.Json;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Events
{
    public partial class TransactionEventResponse : BaseEventResponse
    {
        /// <summary>
        /// Gets or sets the body of the transaction event.
        /// </summary>
        [JsonProperty("event_body")]
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
        [JsonProperty("object")]
        public TransactionObject Object { get; set; }
    }

    /// <summary>
    /// Represents a transaction object containing details of a transaction event.
    /// </summary>
    public partial class TransactionObject
    {
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Transaction Key. Unique gateway generated key.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }

        /// <summary>
        /// Unique transaction reference number.
        /// </summary>
        [JsonProperty("refnum")]
        public long Refnum { get; set; }

        /// <summary>
        /// Amount authorized
        /// </summary>
        [JsonProperty("auth_amount")]
        public string AuthAmount { get; set; }

        /// <summary>
        /// Total amount charged.
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Object containing a more detailed breakdown of the amount.
        /// </summary>
        [JsonProperty("amount_detail")]
        public TransactionAmountDetail AmountDetail { get; set; }

        /// <summary>
        /// Authorization code
        /// </summary>
        [JsonProperty("authcode")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Authcode { get; set; }

        /// <summary>
        /// Result code. Possible options are:
        /// A = Approved, P = Partial Approval, D = Declined, E = Error, 
        /// or V = Verification Required
        /// </summary>
        [JsonProperty("result_code")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Custom Invoice Number to easily retrieve sale details.
        ///  (25 chars max)
        /// </summary>
        [JsonProperty("invoice")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Invoice { get; set; }

        /// <summary>
        /// The Address Verification System (AVS) result.
        /// </summary>
        [JsonProperty("avs")]
        public TransactionVerification Avs { get; set; }

        /// <summary>
        /// The Card Security Code (3-4 digit code) verification result.
        /// </summary>
        [JsonProperty("cvc")]
        public TransactionVerification Cvc { get; set; }

        /// <summary>
        /// The transaction type.(e.g., Credit Card Sale).
        /// </summary>
        [JsonProperty("trantype")]
        public string Trantype { get; set; }

        /// <summary>
        /// Object holding credit card information
        /// </summary>
        [JsonProperty("creditcard")]
        public TransactionCreditcard Creditcard { get; set; }

        /// <summary>
        /// Object which holds all Batch information.
        /// </summary>
        [JsonProperty("batch")]
        public TransactionBatch Batch { get; set; }

        /// <summary>
        ///  Object which holds all Receipt information.
        /// </summary>
        [JsonProperty("receipts")]
        public TransactionReceipts Receipts { get; set; }

        /// <summary>
        /// Description of result code.
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }

        /// <summary>
        /// The date the original transaction was run. 
        /// Applies to void, unvoid, adjust, and capture transactions.
        /// </summary>
        [JsonProperty("original_date")]
        public DateTimeOffset OriginalDate { get; set; }
    }

    /// <summary>
    /// Represents the amount details of a transaction.
    /// </summary>
    public partial class TransactionAmountDetail
    {
        /// <summary>
        /// Gets or sets the tax amount.
        /// </summary>
        [JsonProperty("tax")]
        public string Tax { get; set; }

        /// <summary>
        /// Gets or sets the discount amount.
        /// </summary>
        [JsonProperty("discount")]
        public string Discount { get; set; }

        /// <summary>
        /// Gets or sets the subtotal amount.
        /// </summary>
        [JsonProperty("subtotal")]
        public string Subtotal { get; set; }

        /// <summary>
        /// Gets or sets the duty amount.
        /// </summary>
        [JsonProperty("duty")]
        public string Duty { get; set; }
    }

    /// <summary>
    /// Represents the address verification service (AVS) or (CVS) details of a transaction.
    /// </summary>
    public partial class TransactionVerification
    {
        /// <summary>
        /// Gets or sets the AVS result code.
        /// </summary>
        [JsonProperty("result_code")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Gets or sets the AVS result description.
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
    }

    /// <summary>
    /// Represents the batch details associated with a transaction.
    /// </summary>
    public partial class TransactionBatch
    {
        /// <summary>
        /// Gets or sets the batch reference number.
        /// </summary>
        [JsonProperty("batchrefnum")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Batchrefnum { get; set; }

        /// <summary>
        /// Gets or sets the sequence number within the batch.
        /// </summary>
        [JsonProperty("sequence")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Sequence { get; set; }

        /// <summary>
        /// Gets or sets the type of the batch.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the key of the batch.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
    }

    /// <summary>
    /// Represents the credit card details associated with a transaction.
    /// </summary>
    public partial class TransactionCreditcard
    {
        /// <summary>
        /// Gets or sets the cardholder's name.
        /// </summary>
        [JsonProperty("cardholder")]
        public string Cardholder { get; set; }

        /// <summary>
        /// Gets or sets the entry mode for the credit card.
        /// </summary>
        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; }

        /// <summary>
        /// Gets or sets the type of the credit card.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the masked credit card number.
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }
    }

    /// <summary>
    /// Represents the receipts information associated with a transaction.
    /// </summary>
    public partial class TransactionReceipts
    {
        /// <summary>
        /// Gets or sets the customer's receipt status.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the merchant's receipt status.
        /// </summary>
        [JsonProperty("merchant")]
        public string Merchant { get; set; }
    }
}
