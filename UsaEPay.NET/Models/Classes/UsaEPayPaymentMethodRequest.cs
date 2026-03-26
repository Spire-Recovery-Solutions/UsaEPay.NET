using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayPaymentMethodRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Payment method type. Possible values: cc, check, giftcard, transaction.
        /// </summary>
        [JsonPropertyName("pay_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string PayType { get; set; }

        /// <summary>
        /// Payment nickname.
        /// </summary>
        [JsonPropertyName("method_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string MethodName { get; set; }

        /// <summary>
        /// Name on card for credit cards. Name on bank account for ACH payment methods.
        /// Will default to customer name if not included in request.
        /// </summary>
        [JsonPropertyName("cardholder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Cardholder { get; set; }

        /// <summary>
        /// Credit/debit card number or giftcard number.
        /// </summary>
        [JsonPropertyName("number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Number { get; set; }

        /// <summary>
        /// Credit/debit card expiration date. Preferred format is MMYY.
        /// </summary>
        [JsonPropertyName("expiration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Expiration { get; set; }

        /// <summary>
        /// Street address for address verification.
        /// </summary>
        [JsonPropertyName("avs_street")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AvsStreet { get; set; }

        /// <summary>
        /// Postal (Zip) code for address verification.
        /// </summary>
        [JsonPropertyName("avs_postalcode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AvsPostalCode { get; set; }

        /// <summary>
        /// ACH routing number. Required for check method.
        /// </summary>
        [JsonPropertyName("routing")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Routing { get; set; }

        /// <summary>
        /// Bank account number. Required for check method.
        /// </summary>
        [JsonPropertyName("account")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Account { get; set; }

        /// <summary>
        /// Bank account type. Possible values are: checking or savings. Required for check method.
        /// </summary>
        [JsonPropertyName("account_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AccountType { get; set; }

        /// <summary>
        /// Account file format (SEC Record type).
        /// </summary>
        [JsonPropertyName("account_format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AccountFormat { get; set; }

        /// <summary>
        /// Driver's license number for bank account holder.
        /// </summary>
        [JsonPropertyName("dl")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Dl { get; set; }

        /// <summary>
        /// State for driver's license number for bank account holder.
        /// </summary>
        [JsonPropertyName("dl_state")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DlState { get; set; }

        /// <summary>
        /// Set to true to set as default payment. Defaults to false if not included in request.
        /// </summary>
        [JsonPropertyName("default")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Default { get; set; }

        /// <summary>
        /// Sort order for the payment method.
        /// </summary>
        [JsonPropertyName("sortord")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int SortOrder { get; set; }

        /// <summary>
        /// Transaction key to reference when saving a payment method from a transaction.
        /// </summary>
        [JsonPropertyName("transaction_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string TransactionKey { get; set; }
    }
}
