using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayResponse : IUsaEPayResponse
    {

        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// Transaction Key. Unique gateway generated key.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        /// <summary>
        /// Object holding check information for check payments
        /// </summary>
        [JsonPropertyName("check")]
        public Check? Check { get; set; }
        /// <summary>
        /// Unique transaction reference number.
        /// </summary>
        [JsonPropertyName("refnum")]
        public string? ReferenceNumber { get; set; }
        /// <summary>
        /// Transaction type code.
        /// </summary>
        [JsonPropertyName("trantype_code")]
        public string? TrantypeCode { get; set; }
        /// <summary>
        /// Reference number returned from the check processor. This will not be returned by all ACH processors.
        /// </summary>
        [JsonPropertyName("proc_refnum")]
        public string? ProcessorReferenceNumber { get; set; }
        /// <summary>
        /// If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
        /// </summary>
        [JsonPropertyName("is_duplicate")]
        public string? IsDuplicate { get; set; }
        /// <summary>
        /// Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
        /// </summary>
        [JsonPropertyName("result_code")]
        public string? ResultCode { get; set; }
        /// <summary>
        /// Description of above result_code (Approved, etc)
        /// </summary>
        [JsonPropertyName("result")]
        public string? Result { get; set; }
        /// <summary>
        /// Authorization code
        /// </summary>
        [JsonPropertyName("authcode")]
        public string? AuthCode { get; set; }
        /// <summary>
        /// Status code
        /// </summary>
        [JsonPropertyName("status_code")]
        public string? StatusCode { get; set; }
        /// <summary>
        /// Description of the status
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        /// <summary>
        /// Object holding credit card information
        /// </summary>
        [JsonPropertyName("creditcard")]
        public CreditCard? CreditCard { get; set; }
        /// <summary>
        /// Object containing saved/tokenized credit card information
        /// </summary>
        [JsonPropertyName("savedcard")]
        public SavedCard? SavedCard { get; set; }
        /// <summary>
        /// Custom Invoice Number to easily retrieve sale details.
        /// </summary>
        [JsonPropertyName("invoice")]
        public string? Invoice { get; set; }
        /// <summary>
        /// The Address Verification System (AVS) result.
        /// </summary>
        [JsonPropertyName("avs")]
        public Avs? Avs { get; set; }
        /// <summary>
        /// The Card Security Code (3-4 digit code) verification result.
        /// </summary>
        [JsonPropertyName("cvc")]
        public Cvc? Cvc { get; set; }
        /// <summary>
        /// Batch information.
        /// </summary>
        [JsonPropertyName("batch")]
        public Batch? Batch { get; set; }
        /// <summary>
        /// Amount needed for transaction.
        /// </summary>
        [JsonPropertyName("amount")]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Amount { get; set; }
        /// <summary>
        /// Amount details needed for transaction.
        /// </summary>
        [JsonPropertyName("amount_detail")]
        public AmountDetail? AmountDetail { get; set; }
        /// <summary>
        /// Merchant assigned order identifier.
        /// </summary>
        [JsonPropertyName("orderid")]
        public string? OrderId { get; set; }
        /// <summary>
        /// Description for the transaction
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        /// <summary>
        /// Private comment details only visible to the merchant.
        /// </summary>
        [JsonPropertyName("comments")]
        public string? Comments { get; set; }
        /// <summary>
        /// Object which holds the customer's billing address information.
        /// </summary>
        [JsonPropertyName("billing_address")]
        public Address? BillingAddress { get; set; }
        /// <summary>
        /// Credit card bin detail. This will only be included if the return_bin flag is passed through.
        /// </summary>
        [JsonPropertyName("bin")]
        public Bin? Bin { get; set; }
        /// <summary>
        /// Credit card fraud detail. This will only be included if the return_fraud flag is passed through.
        /// </summary>
        [JsonPropertyName("fraud")]
        public Fraud? Fraud { get; set; }
        /// <summary>
        /// Amount authorized
        /// </summary>
        [JsonPropertyName("auth_amount")]
        public string? AmountAuthorized { get; set; }
        /// <summary>
        /// The transaction type.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/transactioncodes/#transaction-type-codes"/>
        [JsonPropertyName("trantype")]
        public string? TransactionType { get; set; }
        /// <summary>
        /// Receipt information.
        /// </summary>
        [JsonPropertyName("receipts")]
        public Receipts? Receipts { get; set; }
        /// <summary>
        /// ICC information fields. This will only be included for EMV transactions.
        /// </summary>
        [JsonPropertyName("iccdata")]
        public string? IccData { get; set; }
        /// <summary>
        /// Error summary
        /// </summary>
        [JsonPropertyName("error")]
        public string? Error { get; set; }
        /// <summary>
        /// Numeric error code returned by the gateway on error/decline responses.
        /// Decline codes are in the 10xxx range (e.g., 10205 = "Do not Honor", 10251 = "Insufficient funds").
        /// Gateway error codes are in the 20xxx range (e.g., 20019 = "Unknown command").
        /// Note: API returns this as a string on declines, integer on gateway errors.
        /// </summary>
        [JsonPropertyName("error_code")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long ErrorCode { get; set; }
        /// <summary>
        /// Alternate error code field used by non-transaction endpoints (no underscore).
        /// The API inconsistently uses "error_code" (transactions) vs "errorcode" (receipts, bulk, etc.).
        /// </summary>
        [JsonPropertyName("errorcode")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long ErrorCodeAlt { get; set; }

        /// <summary>
        /// Created Date Time
        /// </summary>
        [JsonPropertyName("created")]
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? CreatedTimestamp { get; set; }

        /// <summary>
        /// Customer's purchase order number.
        /// </summary>
        [JsonPropertyName("ponum")]
        public string? Ponum { get; set; }
        /// <summary>
        /// Terminal identifier.
        /// </summary>
        [JsonPropertyName("terminal")]
        public string? Terminal { get; set; }
        /// <summary>
        /// Clerk/Cashier/Server name.
        /// </summary>
        [JsonPropertyName("clerk")]
        public string? Clerk { get; set; }
        /// <summary>
        /// Gateway customer key.
        /// </summary>
        [JsonPropertyName("custkey")]
        public string? CustomerKey { get; set; }
        /// <summary>
        /// Gateway customer identifier (SOAP API style).
        /// </summary>
        [JsonPropertyName("custid")]
        public string? CustId { get; set; }
        /// <summary>
        /// Merchant-assigned customer ID.
        /// </summary>
        [JsonPropertyName("customerid")]
        public string? CustomerId { get; set; }
        /// <summary>
        /// Customer email address.
        /// </summary>
        [JsonPropertyName("customer_email")]
        public string? CustomerEmail { get; set; }
        /// <summary>
        /// Merchant email address.
        /// </summary>
        [JsonPropertyName("merchant_email")]
        public string? MerchantEmail { get; set; }
        /// <summary>
        /// IP address of client.
        /// </summary>
        [JsonPropertyName("clientip")]
        public string? ClientIp { get; set; }
        /// <summary>
        /// API key name / source.
        /// </summary>
        [JsonPropertyName("source_name")]
        public string? SourceName { get; set; }
        /// <summary>
        /// List of valid actions on this transaction.
        /// </summary>
        [JsonPropertyName("available_actions")]
        public string[]? AvailableActions { get; set; }
        /// <summary>
        /// Processor platform information.
        /// </summary>
        [JsonPropertyName("platform")]
        public Platform? Platform { get; set; }
        /// <summary>
        /// Object which holds the customer's shipping address information.
        /// </summary>
        [JsonPropertyName("shipping_address")]
        public Address? ShippingAddress { get; set; }
        /// <summary>
        /// Array of line items attached to the transaction.
        /// </summary>
        [JsonPropertyName("lineitems")]
        public List<LineItem>? LineItems { get; set; }
        /// <summary>
        /// Custom fields attached to the transaction.
        /// </summary>
        [JsonPropertyName("custom_fields")]
        public Dictionary<string, string>? CustomFields { get; set; }
    }

    public partial class CreditCard
    {
        /// <summary>
        /// Category level code returned from card brand.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/cardlevelcodes/"/>
        [JsonPropertyName("category_code")]
        public string? CategoryCode { get; set; }
        /// <summary>
        /// Application identifier. This will only be included for EMV transactions.
        /// </summary>
        [JsonPropertyName("aid")]
        public string? ApplicationId { get; set; }
        /// <summary>
        /// How payment was presented (i.e. swipe, contactless, manually key, etc.)
        /// </summary>
        [JsonPropertyName("entry_mode")]
        public string? EntryMode { get; set; }

        /// <summary>
        /// The type of credit card being used in the transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string? CardType { get; set; }

    }

    public class SavedCard
    {
        /// <summary>
        /// The brand of the card (visa, mastercard, amex)
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// Unique key for the card. This is the token.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        /// <summary>
        /// Masked credit card data.
        /// </summary>
        /// <summary>
        /// Card expiration date (MMYY format).
        /// </summary>
        [JsonPropertyName("expiration")]
        public string? Expiration { get; set; }
        /// <summary>
        /// Masked credit card data.
        /// </summary>
        [JsonPropertyName("cardnumber")]
        public string? CardNumber { get; set; }
    }

    /// <summary>
    /// Response-only fields for Check (ACH) transactions returned in GET detail.
    /// </summary>
    public partial class Check
    {
        /// <summary>
        /// Check number (response field).
        /// </summary>
        [JsonPropertyName("checknum")]
        public string? CheckNum { get; set; }
        /// <summary>
        /// ACH tracking number.
        /// </summary>
        [JsonPropertyName("trackingnum")]
        public string? TrackingNum { get; set; }
        /// <summary>
        /// Effective date.
        /// </summary>
        [JsonPropertyName("effective")]
        public string? Effective { get; set; }
        /// <summary>
        /// Processed date.
        /// </summary>
        [JsonPropertyName("processed")]
        public string? Processed { get; set; }
        /// <summary>
        /// Settled date.
        /// </summary>
        [JsonPropertyName("settled")]
        public string? Settled { get; set; }
        /// <summary>
        /// Returned date.
        /// </summary>
        [JsonPropertyName("returned")]
        public string? Returned { get; set; }
        /// <summary>
        /// Bank note.
        /// </summary>
        [JsonPropertyName("banknote")]
        public string? BankNote { get; set; }
        /// <summary>
        /// Bank account number (masked).
        /// </summary>
        [JsonPropertyName("account_number")]
        public string? AccountNumber { get; set; }
        /// <summary>
        /// Bank routing number (masked).
        /// </summary>
        [JsonPropertyName("routing_number")]
        public string? RoutingNumber { get; set; }
    }

    public class Avs
    {
        /// <summary>
        /// Address verification result code.
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/developer/docs/reference/avs"/>
        [JsonPropertyName("result_code")]
        public string? ResultCode { get; set; }
        /// <summary>
        /// Description of result code.
        /// </summary>
        [JsonPropertyName("result")]
        public string? Result { get; set; }
    }

    public class Cvc
    {
        /// <summary>
        /// Cvc result code
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/developer/docs/reference/cvv2"/>
        [JsonPropertyName("result_code")]
        public string? ResultCode { get; set; }
        /// <summary>
        /// Description of result code.
        /// </summary>
        [JsonPropertyName("result")]
        public string? Result { get; set; }
    }
    public class Batch : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Denotes this object is a batch.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// This is the gateway generated unique identifier for the batch.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        /// <summary>
        /// This is the unique batch identifier. This was originally used in the SOAP API.
        /// </summary>
        [JsonPropertyName("batchrefnum")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long BatchReferenceNumber { get; set; }
        /// <summary>
        /// Date and time the batch was opened. Format will be, YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonPropertyName("opened")]
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Opened { get; set; }
        /// <summary>
        /// Date and time the batch was closed. Format will be, YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonPropertyName("closed")]
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Closed { get; set; }
        /// <summary>
        /// Batch status. Options are: open, closed, and closing when the batch is in the process of closing.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        /// <summary>
        /// Date and time the batch is scheduled to be closed. Format will be, YYYY-MM-DD HH:MM:SS.
        /// Only shows for open batches.
        /// </summary>
        [JsonPropertyName("scheduled")]
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Scheduled { get; set; }
        /// <summary>
        /// The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc.
        /// </summary>
        [JsonPropertyName("sequence")]
        public string? Sequence { get; set; }
        /// <summary>
        /// Total amount in dollars in the specific batch. This includes sales, voids, and refunds.
        /// </summary>
        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// Total transaction count. This includes sales, voids, and refunds.
        /// Only shows for open batches.
        /// </summary>
        [JsonPropertyName("total_count")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long TotalCount { get; set; }
        /// <summary>
        /// Total amount for sales in dollars in the specific batch.
        /// Only shows when batch contains this transaction type.
        /// </summary>
        [JsonPropertyName("sales_amount")]
        public decimal SalesAmount { get; set; }
        /// <summary>
        /// Total transaction count. This includes sales only.
        /// Only shows when batch contains this transaction type.
        /// </summary>
        [JsonPropertyName("sales_count")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long SalesCount { get; set; }
        /// <summary>
        /// Total amount for voids in dollars in the specific batch.
        /// Only shows when batch contains this transaction type.
        /// </summary>
        [JsonPropertyName("voids_amount")]
        public decimal VoidsAmount { get; set; }
        /// <summary>
        /// Total transaction count. This includes voids only.
        /// Only shows when batch contains this transaction type.
        /// </summary>
        [JsonPropertyName("voids_count")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long VoidsCount { get; set; }
        /// <summary>
        /// Total amount for refunds in dollars in the specific batch.
        /// Only shows when batch contains this transaction type.
        /// </summary>
        [JsonPropertyName("refunds_amount")]
        public decimal RefundsAmount { get; set; }
        /// <summary>
        /// Total transaction count. This includes refunds only.
        /// Only shows when batch contains this transaction type.
        /// </summary>
        [JsonPropertyName("refunds_count")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long RefundsCount { get; set; }
        /// <summary>
        /// Parent batch sequence number.
        /// </summary>
        [JsonPropertyName("parent_sequence")]
        public string? ParentSequence { get; set; }
        /// <summary>
        /// Whether the batch has been resubmitted.
        /// </summary>
        [JsonPropertyName("resubmitted")]
        public string? Resubmitted { get; set; }
        /// <summary>
        /// Whether the batch is locked.
        /// </summary>
        [JsonPropertyName("locked")]
        public bool? Locked { get; set; }
        /// <summary>
        /// Date and time the batch was locked.
        /// </summary>
        [JsonPropertyName("lockdate")]
        public string? LockDate { get; set; }
        /// <summary>
        /// Transaction cutoff time for the batch.
        /// </summary>
        [JsonPropertyName("trancutoff")]
        public string? TranCutoff { get; set; }
        /// <summary>
        /// Total sales dollar amount in the batch.
        /// </summary>
        [JsonPropertyName("sales")]
        public string? Sales { get; set; }
        /// <summary>
        /// Total credits dollar amount in the batch.
        /// </summary>
        [JsonPropertyName("credits")]
        public string? Credits { get; set; }
        /// <summary>
        /// Total number of credit transactions in the batch.
        /// </summary>
        [JsonPropertyName("credits_count")]
        public string? CreditsCount { get; set; }
    }

    public class Bin
    {
        /// <summary>
        /// Six digit credit card bin.
        /// </summary>
        [JsonPropertyName("bin")]
        public int CardBin { get; set; }
        /// <summary>
        /// Masked credit card number.
        /// </summary>
        [JsonPropertyName("number")]
        public string? Number { get; set; }
        /// <summary>
        /// Card brand type (Visa, MasterCard, Discover, etc.)
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// Card Issuer
        /// </summary>
        [JsonPropertyName("issuer")]
        public string? Issuer { get; set; }
        /// <summary>
        /// Card issuing bank.
        /// </summary>
        [JsonPropertyName("bank")]
        public string? Bank { get; set; }
        /// <summary>
        /// Country abbreviation of issuing bank.
        /// </summary>
        [JsonPropertyName("country")]
        public string? Country { get; set; }
        /// <summary>
        /// Country name of issuing bank.
        /// </summary>
        [JsonPropertyName("country_name")]
        public string? CountryName { get; set; }
        /// <summary>
        /// Country code of issuing bank.
        /// </summary>
        [JsonPropertyName("country_iso")]
        public string? CountryIso { get; set; }
        /// <summary>
        /// Bank location information
        /// </summary>
        [JsonPropertyName("location")]
        public string? Location { get; set; }
        /// <summary>
        /// Bank contact information
        /// </summary>
        [JsonPropertyName("phone")]
        public string? Phone { get; set; }
        /// <summary>
        /// Category level code returned from card brand.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/cardlevelcodes/"/>
        [JsonPropertyName("category")]
        public string? Category { get; set; }
        /// <summary>
        /// Bin info source.
        /// </summary>
        [JsonPropertyName("data_source")]
        public string? DataSource { get; set; }
    }

    public class Fraud
    {
        /// <summary>
        /// Denotes if a card is blocked and how it has been blocked. Possible options are:
        /// <para>none: Card has not been blocked.</para>
        /// <para>merchant: Card has been blocked on merchant level.</para>
        /// <para>global: Card has been added to global block list.</para>
        /// </summary>
        [JsonPropertyName("card_blocked")]
        public string? CardBlocked { get; set; }
    }

    public class Platform
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class Receipts
    {
        /// <summary>
        /// Will read "Mail Sent Successfully" if receipt was sent to customer.
        /// </summary>
        [JsonPropertyName("customer")]
        public string? Customer { get; set; }
        /// <summary>
        /// Will read "Mail Sent Successfully" if receipt was sent to merchant.
        /// </summary>
        [JsonPropertyName("merchant")]
        public string? Merchant { get; set; }
    }
    public class UsaEPayBatchListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// The maximum amount of batches that will be included.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }
        /// <summary>
        /// The number of batches skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }
        /// <summary>
        /// An array of batches matching the search.
        /// </summary>
        [JsonPropertyName("data")]
        public Batch[]? Data { get; set; }
        /// <summary>
        /// The total amount of batches, including filtering results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
    public class UsaEPayBatchTransactionResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// The maximum amount of batches that will be included.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }
        /// <summary>
        /// The number of batches skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }
        /// <summary>
        /// An array of batches matching the search.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayResponse[]? Data { get; set; }
        /// <summary>
        /// The total amount of batches, including filtering results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }

    }
    public class UsaEPayListTransactionResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        /// <summary>
        /// The maximum amount of batches that will be included.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }
        /// <summary>
        /// The number of batches skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }
        /// <summary>
        /// An array of batches matching the search.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayResponse[]? Data { get; set; }
        /// <summary>
        /// The total amount of batches, including filtering results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }

    }
}
