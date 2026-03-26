using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayPaymentMethodResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The object type. Successful calls will always return "customerpaymentmethod".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gateway generated customer payment method identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        /// <summary>
        /// Payment nickname. Will not be sent to processor unless cardholder field is left blank.
        /// </summary>
        [JsonPropertyName("method_name")]
        public string? MethodName { get; set; }

        /// <summary>
        /// Name on card for credit cards or bank account for ACH payment methods.
        /// </summary>
        [JsonPropertyName("cardholder")]
        public string? Cardholder { get; set; }

        /// <summary>
        /// Expiration date for credit/debit card.
        /// </summary>
        [JsonPropertyName("expires")]
        public string? Expires { get; set; }

        /// <summary>
        /// Card brand type (Visa, MasterCard, Discover, etc.)
        /// </summary>
        [JsonPropertyName("card_type")]
        public string? CardType { get; set; }

        /// <summary>
        /// Last 4 digits of credit/debit card.
        /// </summary>
        [JsonPropertyName("ccnum4last")]
        public string? Ccnum4Last { get; set; }

        /// <summary>
        /// Street address for address verification.
        /// </summary>
        [JsonPropertyName("avs_street")]
        public string? AvsStreet { get; set; }

        /// <summary>
        /// Postal (Zip) code for address verification.
        /// </summary>
        [JsonPropertyName("avs_postalcode")]
        public string? AvsPostalCode { get; set; }

        /// <summary>
        /// Payment type (e.g. "cc" for credit card, "check" for ACH).
        /// </summary>
        [JsonPropertyName("pay_type")]
        public string? PayType { get; set; }

        /// <summary>
        /// Bank account number (masked). Returned for ACH payment methods.
        /// </summary>
        [JsonPropertyName("account_number")]
        public string? AccountNumber { get; set; }

        /// <summary>
        /// Bank routing number (masked). Returned for ACH payment methods.
        /// </summary>
        [JsonPropertyName("routing_number")]
        public string? RoutingNumber { get; set; }

        /// <summary>
        /// Sort order for the payment method.
        /// </summary>
        [JsonPropertyName("sortord")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long SortOrder { get; set; }

        /// <summary>
        /// Date and time the payment method was added to the customer. Format is YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonPropertyName("added")]
        public string? Added { get; set; }

        /// <summary>
        /// Date and time the payment method was last updated. Format is YYYY-MM-DD HH:MM:SS.
        /// </summary>
        [JsonPropertyName("updated")]
        public string? Updated { get; set; }

        /// <summary>
        /// Status of the operation. Returned on delete operations.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class UsaEPayPaymentMethodListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The type of object returned. Returns a list.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// The maximum amount of payment methods that will be included in response.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }

        /// <summary>
        /// The number of payment methods skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }

        /// <summary>
        /// An array of payment methods matching the request.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayPaymentMethodResponse[]? Data { get; set; }

        /// <summary>
        /// The total amount of payment methods, including filtered results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
}
