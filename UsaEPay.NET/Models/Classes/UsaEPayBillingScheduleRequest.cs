using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    /// <summary>
    /// Request model for creating a customer recurring billing schedule.
    /// </summary>
    public class UsaEPayBillingScheduleRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string? Endpoint { get; set; }

        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Key of the customer payment method that should be charged for this billing schedule.
        /// </summary>
        [JsonPropertyName("paymethod_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? PaymentMethodKey { get; set; }

        /// <summary>
        /// Total amount charged for recurring billing. (Required)
        /// </summary>
        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Amount { get; set; }

        /// <summary>
        /// Currency numerical code. Defaults to 840 (USD).
        /// </summary>
        [JsonPropertyName("currency_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? CurrencyCode { get; set; }

        /// <summary>
        /// Describes recurring payment.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Description { get; set; }

        /// <summary>
        /// Set to true to enable the recurring schedules. Defaults to false.
        /// </summary>
        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Enabled { get; set; }

        /// <summary>
        /// How often the recurring billing schedule should charge. Possible values are: weekly, monthly, or yearly. (Required)
        /// </summary>
        [JsonPropertyName("frequency")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Frequency { get; set; }

        /// <summary>
        /// The next time you would like the recurring billing schedule to run. Format is: YYYY-MM-DD. (Required)
        /// </summary>
        [JsonPropertyName("next_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? NextDate { get; set; }

        /// <summary>
        /// The number of times you would like the recurring billing schedule to run. Set to -1 for indefinite. (Required)
        /// </summary>
        [JsonPropertyName("numleft")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? NumLeft { get; set; }

        /// <summary>
        /// Merchant assigned order ID.
        /// </summary>
        [JsonPropertyName("orderid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? OrderId { get; set; }

        /// <summary>
        /// Message to include on the customer's receipt.
        /// </summary>
        [JsonPropertyName("receipt_note")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? ReceiptNote { get; set; }

        /// <summary>
        /// Set to true to send a receipt when the transaction runs. Defaults to false.
        /// </summary>
        [JsonPropertyName("send_receipt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool SendReceipt { get; set; }

        /// <summary>
        /// Set the source for the recurring billing schedule. Defaults to the Recurring source.
        /// </summary>
        [JsonPropertyName("source")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Source { get; set; }

        /// <summary>
        /// The start date for the recurring schedule. Can be set prior to the next date. Format is: YYYY-MM-DD. (Required)
        /// </summary>
        [JsonPropertyName("start_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? StartDate { get; set; }

        /// <summary>
        /// The portion of the amount that is tax for the transaction.
        /// </summary>
        [JsonPropertyName("tax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Tax { get; set; }

        /// <summary>
        /// Gateway generated User ID for the user who set up the recurring schedule.
        /// </summary>
        [JsonPropertyName("user")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? User { get; set; }

        /// <summary>
        /// Username for the user who set up the recurring schedule.
        /// </summary>
        [JsonPropertyName("username")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Username { get; set; }

        /// <summary>
        /// Frequency interval. If frequency is monthly: set to 1 to charge every month, set to 2 to charge every other month, etc. (Required)
        /// </summary>
        [JsonPropertyName("skip_count")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? SkipCount { get; set; }

        /// <summary>
        /// Array of billing schedule rules. (Required)
        /// </summary>
        [JsonPropertyName("rules")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<UsaEPayBillingScheduleRule>? Rules { get; set; }
    }
}
