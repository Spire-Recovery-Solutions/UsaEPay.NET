using Newtonsoft.Json;

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
        /// The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc.
        /// </summary>
        [JsonProperty("sequence")]
        public string Sequence { get; set; }
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
}
