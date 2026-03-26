using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    /// <summary>
    /// Response model for a customer recurring billing schedule.
    /// </summary>
    public class UsaEPayBillingScheduleResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Gateway generated customer billing schedule identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// The object type. Successful calls will always return billingschedule.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Key of the customer payment method that will be charged for this billing schedule.
        /// </summary>
        [JsonPropertyName("paymethod_key")]
        public string PaymentMethodKey { get; set; }

        /// <summary>
        /// Merchant designated name for the payment method that will be charged for this billing schedule.
        /// </summary>
        [JsonPropertyName("method_name")]
        public string MethodName { get; set; }

        /// <summary>
        /// Total amount charged for recurring billing.
        /// </summary>
        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        /// <summary>
        /// Currency numerical code. Defaults to 840 (USD).
        /// </summary>
        [JsonPropertyName("currency_code")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Describes recurring payment.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// If 1 is returned, schedule is enabled. If 0 is returned, schedule is not enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public string Enabled { get; set; }

        /// <summary>
        /// How often the recurring billing schedule should charge. Possible values are: weekly, monthly, or yearly.
        /// </summary>
        [JsonPropertyName("frequency")]
        public string Frequency { get; set; }

        /// <summary>
        /// The next time the recurring billing schedule will run. Format is: YYYY-MM-DD.
        /// </summary>
        [JsonPropertyName("next_date")]
        public string NextDate { get; set; }

        /// <summary>
        /// The number of times the recurring billing schedule will run. If -1 is returned the schedule will run indefinitely.
        /// </summary>
        [JsonPropertyName("numleft")]
        public string NumLeft { get; set; }

        /// <summary>
        /// Merchant assigned order ID.
        /// </summary>
        [JsonPropertyName("orderid")]
        public string OrderId { get; set; }

        /// <summary>
        /// Message to include on the customer's receipt.
        /// </summary>
        [JsonPropertyName("receipt_note")]
        public string ReceiptNote { get; set; }

        /// <summary>
        /// If 1 is returned, receipt will be sent. If 0 is returned, receipt will not be sent.
        /// </summary>
        [JsonPropertyName("send_receipt")]
        public string SendReceipt { get; set; }

        /// <summary>
        /// Source associated with the recurring billing schedule.
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// The start date for the recurring schedule. Format is: YYYY-MM-DD.
        /// </summary>
        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// The portion of the amount that is tax for the transaction.
        /// </summary>
        [JsonPropertyName("tax")]
        public string Tax { get; set; }

        /// <summary>
        /// Gateway generated User ID for the user who set up the recurring schedule.
        /// </summary>
        [JsonPropertyName("user")]
        public string User { get; set; }

        /// <summary>
        /// Username for the user who set up the recurring schedule.
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Frequency interval. If frequency is monthly: set to 1 to charge every month, set to 2 to charge every other month, etc.
        /// </summary>
        [JsonPropertyName("skip_count")]
        public string SkipCount { get; set; }

        /// <summary>
        /// Array of billing schedule rules.
        /// </summary>
        [JsonPropertyName("rules")]
        public List<UsaEPayBillingScheduleRule> Rules { get; set; }
    }

    /// <summary>
    /// Billing schedule rule defining when the schedule runs.
    /// </summary>
    public class UsaEPayBillingScheduleRule
    {
        /// <summary>
        /// Gateway generated billing schedule rule identifier.
        /// </summary>
        [JsonPropertyName("key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Key { get; set; }

        /// <summary>
        /// The object type. Successful calls will return billingschedulerule.
        /// </summary>
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type { get; set; }

        /// <summary>
        /// Day offset for the billing schedule rule.
        /// </summary>
        [JsonPropertyName("day_offset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DayOffset { get; set; }

        /// <summary>
        /// Month offset for the billing schedule rule.
        /// </summary>
        [JsonPropertyName("month_offset")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string MonthOffset { get; set; }

        /// <summary>
        /// Subject for the billing schedule rule (e.g. "Day", "wed").
        /// </summary>
        [JsonPropertyName("subject")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Subject { get; set; }
    }

    /// <summary>
    /// Response model for a list of customer recurring billing schedules.
    /// </summary>
    public partial class UsaEPayBillingScheduleListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The type of object returned. Returns a list.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The maximum amount of schedules that will be included in response.
        /// </summary>
        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// The number of schedules skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// An array of billing schedules matching the request.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayBillingScheduleResponse[] Data { get; set; }

        /// <summary>
        /// The total amount of schedules, including filtered results.
        /// </summary>
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}
