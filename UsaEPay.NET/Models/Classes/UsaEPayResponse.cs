using Newtonsoft.Json;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayResponse : IUsaEPayResponse
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
        public string ReferenceNumber { get; set; }
        /// <summary>
        /// Unique transaction reference number.
        /// </summary>
        [JsonProperty("trantype_code")]
        public string TrantypeCode { get; set; }
        /// <summary>
        /// Reference number returned from the check processor. This will not be returned by all ACH processors.
        /// </summary>
        [JsonProperty("proc_refnum")]
        public string ProcessorReferenceNumber { get; set; }
        /// <summary>
        /// If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
        /// </summary>
        [JsonProperty("is_duplicate")]
        public string IsDuplicate { get; set; }
        /// <summary>
        /// Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
        /// </summary>
        [JsonProperty("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// Description of above result_code (Approved, etc)
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
        /// <summary>
        /// Authorization code
        /// </summary>
        [JsonProperty("authcode")]
        public string AuthCode { get; set; }
        /// <summary>
        /// Status code
        /// </summary>
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
        /// <summary>
        /// Description of the status 
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }
        /// <summary>
        /// Object holding credit card information
        /// </summary>
        [JsonProperty("creditcard")]
        public CreditCard CreditCard { get; set; }
        /// <summary>
        /// Object containing saved/tokenized credit card information
        /// </summary>
        [JsonProperty("savedcard")]
        public SavedCard SavedCard { get; set; }
        /// <summary>
        /// Custom Invoice Number to easily retrieve sale details.
        /// </summary>
        [JsonProperty("invoice")]
        public string Invoice { get; set; }
        /// <summary>
        /// The Address Verification System (AVS) result.
        /// </summary>
        [JsonProperty("avs")]
        public AVS AVS { get; set; }
        /// <summary>
        /// The Card Security Code (3-4 digit code) verification result.
        /// </summary>
        [JsonProperty("cvc")]
        public CVC CVC { get; set; }
        /// <summary>
        /// Batch information.
        /// </summary>
        [JsonProperty("batch")]
        public Batch Batch { get; set; }
        /// <summary>
        /// Amount needed for transaction.
        /// </summary>
        [JsonProperty("amount")]
        public string Amount { get; set; }
        /// <summary>
        /// Amount details needed for transaction.
        /// </summary>
        [JsonProperty("amount_detail")]
        public AmountDetail AmountDetail { get; set; }
        /// <summary>
        /// Merchant assigned order identifier.
        /// </summary>
        [JsonProperty("orderid")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Orderid { get; set; }
        /// <summary>
        /// Description for the transaction
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// Private comment details only visible to the merchant.
        /// </summary>
        [JsonProperty("comments")]
        public string Comments { get; set; }
        /// <summary>
        /// Object which holds the customer's billing address information.
        /// </summary>
        [JsonProperty("billing_address")]
        public Address BillingAddress { get; set; }
        /// <summary>
        /// Credit card bin detail. This will only be included if the return_bin flag is passed through.
        /// </summary>
        [JsonProperty("bin")]
        public Bin Bin { get; set; }
        /// <summary>
        /// Credit card fraud detail. This will only be included if the return_fraud flag is passed through.
        /// </summary>
        [JsonProperty("fraud")]
        public Fraud Fraud { get; set; }
        /// <summary>
        /// Amount authorized
        /// </summary>
        [JsonProperty("auth_amount")]
        public string AmountAuthorized { get; set; }
        /// <summary>
        /// The transaction type.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/transactioncodes/#transaction-type-codes"/>
        [JsonProperty("trantype")]
        public string TransactionType { get; set; }
        /// <summary>
        /// Receipt information.
        /// </summary>
        [JsonProperty("receipts")]
        public Receipts Receipts { get; set; }
        /// <summary>
        /// ICC information fields. This will only be included for EMV transactions.
        /// </summary>
        [JsonProperty("iccdata")]
        public string ICCData { get; set; }
        /// <summary>
        /// Error summary
        /// </summary>
        [JsonProperty("error")]
        public string Error { get; set; }
    }

    public partial class CreditCard
    {
        /// <summary>
        /// Category level code returned from card brand.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/cardlevelcodes/"/>
        [JsonProperty("category_code")]
        public string CategoryCode { get; set; }
        /// <summary>
        /// Application identifier. This will only be included for EMV transactions.
        /// </summary>
        [JsonProperty("aid")]
        public string ApplicationID { get; set; }
        /// <summary>
        /// How payment was presented (i.e. swipe, contactless, manually key, etc.)
        /// </summary>
        [JsonProperty("entry_mode")]
        public string EntryMode { get; set; }
    }

    public partial class SavedCard
    {
        /// <summary>
        /// The brand of the card (visa, mastercard, amex)
        /// </summary>
        [JsonProperty("string")]
        public string Type { get; set; }
        /// <summary>
        /// Unique key for the card. This is the token.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
        /// <summary>
        /// Masked credit card data.
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }
    }

    public class AVS
    {
        /// <summary>
        /// Address verification result code.
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/developer/docs/reference/avs"/>
        [JsonProperty("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// Description of result code.
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
    }

    public class CVC
    {
        /// <summary>
        /// CVC result code
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/developer/docs/reference/cvv2"/>
        [JsonProperty("result_code")]
        public string ResultCode { get; set; }
        /// <summary>
        /// Description of result code.
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }
    }
    public class Batch
    {
        /// <summary>
        /// Denotes this object is a batch.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// This is the gateway generated unique identifier for the batch.
        /// </summary>
        [JsonProperty("key")]
        public string Key { get; set; }
        /// <summary>
        /// This is the unique batch identifier. This was originally used in the SOAP API.
        /// </summary>
        [JsonProperty("batchrefnum")]
        public int BatchReferenceNumber { get; set; }
        /// <summary>
        /// Date and time the batch was opened. Format will be, YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonProperty("opened")]
        public DateTimeOffset Opened { get; set; }
        /// <summary>
        /// Date and time the batch was closed. Format will be, YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonProperty("closed")]
        public DateTimeOffset? Closed { get; set; }
        /// <summary>
        /// The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc.
        /// </summary>
        [JsonProperty("sequence")]
        public string Sequence { get; set; }
    }

    public class Bin
    {
        /// <summary>
        /// Six digit credit card bin.
        /// </summary>
        [JsonProperty("bin")]
        public string CardBin { get; set; }
        /// <summary>
        /// Masked credit card number.
        /// </summary>
        [JsonProperty("number")]
        public string Number { get; set; }
        /// <summary>
        /// Card brand type (Visa, MasterCard, Discover, etc.)
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// Card Issuer
        /// </summary>
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        /// <summary>
        /// Card issuing bank.
        /// </summary>
        [JsonProperty("bank")]
        public string Bank { get; set; }
        /// <summary>
        /// Country abbreviation of issuing bank.
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }
        /// <summary>
        /// Country name of issuing bank.
        /// </summary>
        [JsonProperty("country_name")]
        public string CountryName { get; set; }
        /// <summary>
        /// Country code of issuing bank.
        /// </summary>
        [JsonProperty("country_iso")]
        public string CountryISO { get; set; }
        /// <summary>
        /// Bank location information
        /// </summary>
        [JsonProperty("location")]
        public string Location { get; set; }
        /// <summary>
        /// Bank contact information
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
        /// <summary>
        /// Category level code returned from card brand.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/cardlevelcodes/"/>
        [JsonProperty("category")]
        public string Category { get; set; }
        /// <summary>
        /// Bin info source.
        /// </summary>
        [JsonProperty("data_source")]
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
        [JsonProperty("card_blocked")]
        public string CardBlocked { get; set; }
    }

    public class Receipts
    {
        /// <summary>
        /// Will read "Mail Sent Successfully" if receipt was sent to customer.
        /// </summary>
        [JsonProperty("customer")]
        public string Customer { get; set; }
        /// <summary>
        /// Will read "Mail Sent Successfully" if receipt was sent to merchant.
        /// </summary>
        [JsonProperty("merchant")]
        public string Merchant { get; set; }
    }
    public partial class UsaEPayBatchListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// The maximum amount of batches that will be included.
        /// </summary>
        [JsonProperty("limit")]
        public long Limit { get; set; }
        /// <summary>
        /// The number of batches skipped from the results.
        /// </summary>
        [JsonProperty("offset")]
        public long Offset { get; set; }
        /// <summary>
        /// An array of batches matching the search.
        /// </summary>
        [JsonProperty("data")]
        public Batch[] Data { get; set; }
        /// <summary>
        /// The total amount of batches, including filtering results.
        /// </summary>
        [JsonProperty("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
    public partial class UsaEPayBatchTransactionResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }
        /// <summary>
        /// The maximum amount of batches that will be included.
        /// </summary>
        [JsonProperty("limit")]
        public long Limit { get; set; }
        /// <summary>
        /// The number of batches skipped from the results.
        /// </summary>
        [JsonProperty("offset")]
        public long Offset { get; set; }
        /// <summary>
        /// An array of batches matching the search.
        /// </summary>
        [JsonProperty("data")]
        public UsaEPayResponse[] Data { get; set; }
        /// <summary>
        /// The total amount of batches, including filtering results.
        /// </summary>
        [JsonProperty("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }

    }
}
