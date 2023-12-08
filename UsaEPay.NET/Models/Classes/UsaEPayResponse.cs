using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayResponse : IUsaEPayResponse
    {
        
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
        /// <summary>
        /// Transaction Key. Unique gateway generated key.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
        /// <summary>
        /// Object holding check information for check payments
        /// </summary>
        [JsonPropertyName("check")]
        public Check Check { get; set; }
        ///// <summary>
        ///// Unique transaction reference number.
        ///// </summary>
        //[JsonPropertyName("refnum")]
        //public string ReferenceNumber { get; set; }
        /// <summary>
        /// Unique transaction reference number.
        /// </summary>
        [JsonPropertyName("trantype_code")]
        public string TrantypeCode { get; set; }
        /// <summary>
        /// Reference number returned from the check processor. This will not be returned by all ACH processors.
        /// </summary>
        [JsonPropertyName("proc_refnum")]
        public string ProcessorReferenceNumber { get; set; }
        /// <summary>
        /// If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
        /// </summary>
        [JsonPropertyName("is_duplicate")]
        public string IsDuplicate { get; set; }
        /// <summary>
        /// Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
        /// </summary>
        [JsonPropertyName("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// Description of above result_code (Approved, etc)
        /// </summary>
        [JsonPropertyName("result")]
        public string Result { get; set; }
        /// <summary>
        /// Authorization code
        /// </summary>
        [JsonPropertyName("authcode")]
        public string AuthCode { get; set; }
        /// <summary>
        /// Status code
        /// </summary>
        [JsonPropertyName("status_code")]
        public string StatusCode { get; set; }
        /// <summary>
        /// Description of the status 
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
        /// <summary>
        /// Object holding credit card information
        /// </summary>
        [JsonPropertyName("creditcard")]
        public CreditCard CreditCard { get; set; }
        /// <summary>
        /// Object containing saved/tokenized credit card information
        /// </summary>
        [JsonPropertyName("savedcard")]
        public SavedCard SavedCard { get; set; }
        /// <summary>
        /// Custom Invoice Number to easily retrieve sale details.
        /// </summary>
        [JsonPropertyName("invoice")]
        public string Invoice { get; set; }
        /// <summary>
        /// The Address Verification System (AVS) result.
        /// </summary>
        [JsonPropertyName("avs")]
        public AVS AVS { get; set; }
        /// <summary>
        /// The Card Security Code (3-4 digit code) verification result.
        /// </summary>
        [JsonPropertyName("cvc")]
        public CVC CVC { get; set; }
        /// <summary>
        /// Batch information.
        /// </summary>
        [JsonPropertyName("batch")]
        public Batch Batch { get; set; }
        /// <summary>
        /// Amount needed for transaction.
        /// </summary>
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        /// <summary>
        /// Amount details needed for transaction.
        /// </summary>
        [JsonPropertyName("amount_detail")]
        public AmountDetail AmountDetail { get; set; }
        /// <summary>
        /// Merchant assigned order identifier.
        /// </summary>
        [JsonPropertyName("orderid")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Orderid { get; set; }
        /// <summary>
        /// Description for the transaction
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
        /// <summary>
        /// Private comment details only visible to the merchant.
        /// </summary>
        [JsonPropertyName("comments")]
        public string Comments { get; set; }
        /// <summary>
        /// Object which holds the customer's billing address information.
        /// </summary>
        [JsonPropertyName("billing_address")]
        public Address BillingAddress { get; set; }
        /// <summary>
        /// Credit card bin detail. This will only be included if the return_bin flag is passed through.
        /// </summary>
        [JsonPropertyName("bin")]
        public Bin Bin { get; set; }
        /// <summary>
        /// Credit card fraud detail. This will only be included if the return_fraud flag is passed through.
        /// </summary>
        [JsonPropertyName("fraud")]
        public Fraud Fraud { get; set; }
        /// <summary>
        /// Amount authorized
        /// </summary>
        [JsonPropertyName("auth_amount")]
        public string AmountAuthorized { get; set; }
        /// <summary>
        /// The transaction type.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/transactioncodes/#transaction-type-codes"/>
        [JsonPropertyName("trantype")]
        public string TransactionType { get; set; }
        /// <summary>
        /// Receipt information.
        /// </summary>
        [JsonPropertyName("receipts")]
        public Receipts Receipts { get; set; }
        /// <summary>
        /// ICC information fields. This will only be included for EMV transactions.
        /// </summary>
        [JsonPropertyName("iccdata")]
        public string ICCData { get; set; }
        /// <summary>
        /// Error summary
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public partial class CreditCard
    {
        /// <summary>
        /// Category level code returned from card brand.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/cardlevelcodes/"/>
        [JsonPropertyName("category_code")]
        public string CategoryCode { get; set; }
        /// <summary>
        /// Application identifier. This will only be included for EMV transactions.
        /// </summary>
        [JsonPropertyName("aid")]
        public string ApplicationID { get; set; }
        /// <summary>
        /// How payment was presented (i.e. swipe, contactless, manually key, etc.)
        /// </summary>
        [JsonPropertyName("entry_mode")]
        public string EntryMode { get; set; }
    }

    public partial class SavedCard
    {
        /// <summary>
        /// The brand of the card (visa, mastercard, amex)
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
        /// <summary>
        /// Unique key for the card. This is the token.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
        /// <summary>
        /// Masked credit card data.
        /// </summary>
        [JsonPropertyName("cardnumber")]
        public string CardNumber { get; set; }
    }

    public class AVS
    {
        /// <summary>
        /// Address verification result code.
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/developer/docs/reference/avs"/>
        [JsonPropertyName("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// Description of result code.
        /// </summary>
        [JsonPropertyName("result")]
        public string Result { get; set; }
    }

    public class CVC
    {
        /// <summary>
        /// CVC result code
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/developer/docs/reference/cvv2"/>
        [JsonPropertyName("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// Description of result code.
        /// </summary>
        [JsonPropertyName("result")]
        public string Result { get; set; }
    }
    public class Batch : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Denotes this object is a batch.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
        /// <summary>
        /// This is the gateway generated unique identifier for the batch.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }
        /// <summary>
        /// This is the unique batch identifier. This was originally used in the SOAP API.
        /// </summary>
        //[jsonpropertyname("batchrefnum")]
        //public int batchreferencenumber { get; set; }
        /// <summary>
        /// Date and time the batch was opened. Format will be, YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonPropertyName("opened")]
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Opened { get; set; }
        /// <summary>
        /// Date and time the batch was closed. Format will be, YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonPropertyName("closed")]
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Closed { get; set; }
        /// <summary>
        /// Batch status. Options are: open, closed, and closing when the batch is in the process of closing.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }
        /// <summary>
        /// Date and time the batch is scheduled to be closed. Format will be, YYYY-MM-DD HH:MM:SS.
        /// Only shows for open batches.
        /// </summary>
        [JsonPropertyName("scheduled")]
        public string Scheduled { get; set; }
        /// <summary>
        /// The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc.
        /// </summary>
        [JsonPropertyName("sequence")]
        public string Sequence { get; set; }
        /// <summary>
        /// Total amount in dollars in the specific batch. This includes sales, voids, and refunds.
        /// </summary>
        [JsonPropertyName("total_amount")]
        public double TotalAmount { get; set; }
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
        public double SalesAmount { get; set; }
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
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long VoidsAmount { get; set; }
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
        public double RefundsAmount { get; set; }
        /// <summary>
        /// Total transaction count. This includes refunds only.
        /// Only shows when batch contains this transaction type.
        /// </summary>
        [JsonPropertyName("refunds_count")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long RefundsCount { get; set; }
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
        public string Number { get; set; }
        /// <summary>
        /// Card brand type (Visa, MasterCard, Discover, etc.)
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
        /// <summary>
        /// Card Issuer
        /// </summary>
        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }
        /// <summary>
        /// Card issuing bank.
        /// </summary>
        [JsonPropertyName("bank")]
        public string Bank { get; set; }
        /// <summary>
        /// Country abbreviation of issuing bank.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }
        /// <summary>
        /// Country name of issuing bank.
        /// </summary>
        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }
        /// <summary>
        /// Country code of issuing bank.
        /// </summary>
        [JsonPropertyName("country_iso")]
        public string CountryISO { get; set; }
        /// <summary>
        /// Bank location information
        /// </summary>
        [JsonPropertyName("location")]
        public string Location { get; set; }
        /// <summary>
        /// Bank contact information
        /// </summary>
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        /// <summary>
        /// Category level code returned from card brand.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/cardlevelcodes/"/>
        [JsonPropertyName("category")]
        public string Category { get; set; }
        /// <summary>
        /// Bin info source.
        /// </summary>
        [JsonPropertyName("data_source")]
        public string DataSource { get; set; }
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
        public string CardBlocked { get; set; }
    }

    public class Receipts
    {
        /// <summary>
        /// Will read "Mail Sent Successfully" if receipt was sent to customer.
        /// </summary>
        [JsonPropertyName("customer")]
        public string Customer { get; set; }
        /// <summary>
        /// Will read "Mail Sent Successfully" if receipt was sent to merchant.
        /// </summary>
        [JsonPropertyName("merchant")]
        public string Merchant { get; set; }
    }
    public partial class UsaEPayBatchListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
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
        public Batch[] Data { get; set; }
        /// <summary>
        /// The total amount of batches, including filtering results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
    public partial class UsaEPayBatchTransactionResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }
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
        public UsaEPayResponse[] Data { get; set; }
        /// <summary>
        /// The total amount of batches, including filtering results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }

    }
}