using System.Collections.Generic;
using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayPayRequestRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Device key. If device key is not specified, the device associated with current source key is used.
        /// </summary>
        [JsonPropertyName("devicekey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DeviceKey { get; set; }

        /// <summary>
        /// This must be set to sale for a credit or debit card sale (Required).
        /// </summary>
        [JsonPropertyName("command")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Command { get; set; }

        /// <summary>
        /// Total transaction amount (Including tax, tip, shipping, etc.) (Required).
        /// </summary>
        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal Amount { get; set; }

        /// <summary>
        /// Time in seconds to wait for payment. Default is 180 seconds.
        /// </summary>
        [JsonPropertyName("timeout")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Timeout { get; set; }

        /// <summary>
        /// If true, the payment request will return an error right away if the device is offline.
        /// </summary>
        [JsonPropertyName("block_offline")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool BlockOffline { get; set; }

        /// <summary>
        /// Bypass duplicate detection/folding if it has been configured on the api key.
        /// </summary>
        [JsonPropertyName("ignore_duplicate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IgnoreDuplicate { get; set; }

        /// <summary>
        /// Set to true to tokenize the card used to process the transaction.
        /// </summary>
        [JsonPropertyName("save_card")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool SaveCard { get; set; }

        /// <summary>
        /// If true, an option will be displayed to manually key the transaction.
        /// </summary>
        [JsonPropertyName("manual_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool ManualKey { get; set; }

        /// <summary>
        /// Customer will be prompted to leave a tip.
        /// </summary>
        [JsonPropertyName("prompt_tip")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool PromptTip { get; set; }

        /// <summary>
        /// Object containing a more detailed breakdown of the amount.
        /// </summary>
        [JsonPropertyName("amount_detail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public AmountDetail AmountDetail { get; set; }

        /// <summary>
        /// Custom Invoice Number to easily retrieve sale details.
        /// </summary>
        [JsonPropertyName("invoice")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Invoice { get; set; }

        /// <summary>
        /// Customer's purchase order number. Required for level 3 processing.
        /// </summary>
        [JsonPropertyName("ponum")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Ponum { get; set; }

        /// <summary>
        /// Merchant assigned order identifier.
        /// </summary>
        [JsonPropertyName("orderid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string OrderId { get; set; }

        /// <summary>
        /// Public description of the transaction.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Description { get; set; }

        /// <summary>
        /// Private comment details only visible to the merchant.
        /// </summary>
        [JsonPropertyName("comments")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Comments { get; set; }

        /// <summary>
        /// Terminal identifier (i.e. multilane).
        /// </summary>
        [JsonPropertyName("terminal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Terminal { get; set; }

        /// <summary>
        /// Clerk/Cashier/Server name.
        /// </summary>
        [JsonPropertyName("clerk")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Clerk { get; set; }

        /// <summary>
        /// Object which holds the customer's billing address information.
        /// </summary>
        [JsonPropertyName("billing_address")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Object which holds the customer's shipping address information.
        /// </summary>
        [JsonPropertyName("shipping_address")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Customer's email address.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Email { get; set; }

        /// <summary>
        /// Array custom fields attached to the transaction.
        /// </summary>
        [JsonPropertyName("custom_fields")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Dictionary<string, string> CustomFields { get; set; }

        /// <summary>
        /// Array of line items attached to the transaction.
        /// </summary>
        [JsonPropertyName("lineitems")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<LineItem> LineItems { get; set; }

        /// <summary>
        /// Allows you to customize the order in which each payment screen is displayed on the device.
        /// </summary>
        [JsonPropertyName("custom_flow")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string CustomFlow { get; set; }

        /// <summary>
        /// A parameter for custom_flow: options. Custom options for AddOn or Tip screens.
        /// </summary>
        [JsonPropertyName("custom_options")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<CustomOption> CustomOptions { get; set; }
    }

    public class CustomOption
    {
        /// <summary>
        /// Display text shown on the device screen.
        /// </summary>
        [JsonPropertyName("display")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Display { get; set; }

        /// <summary>
        /// Array of denomination choices.
        /// </summary>
        [JsonPropertyName("denominations")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<CustomOptionDenomination> Denominations { get; set; }

        /// <summary>
        /// Option type: "AddOn" or "Tip".
        /// </summary>
        [JsonPropertyName("type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Type { get; set; }

        /// <summary>
        /// SKU for AddOn line items.
        /// </summary>
        [JsonPropertyName("sku")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Sku { get; set; }
    }

    public class CustomOptionDenomination
    {
        /// <summary>
        /// Amount for this denomination.
        /// </summary>
        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Amount { get; set; }

        /// <summary>
        /// Display text for this denomination.
        /// </summary>
        [JsonPropertyName("display")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Display { get; set; }

        /// <summary>
        /// If true, allows the customer to enter a custom amount.
        /// </summary>
        [JsonPropertyName("amount_other")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool AmountOther { get; set; }
    }
}
