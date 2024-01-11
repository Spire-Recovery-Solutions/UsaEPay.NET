using System.Text.Json.Serialization;
using RestSharp;
using UsaEPay.NET.Converter;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        //public Type ResponseType { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// This must be set to sale for a credit or debit card sale (Required)
        /// </summary>
        [JsonPropertyName("command")]
        //[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Command { get; set; }
        /// <summary>
        /// One time use token provided by Client JS Library
        /// </summary>
        [JsonPropertyName("payment_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string PaymentKey { get; set; }
        /// <summary>
        /// Unique gateway generated transaction key, for the transaction you are referencing with this QuickSale
        /// </summary>
        [JsonPropertyName("trankey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string TransactionKey { get; set; }
        /// <summary>
        /// Authorization code received from processor for posting a payment
        /// </summary>
        [JsonPropertyName("authcode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AuthCode { get; set; }
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
        /// Customer's email address
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Email { get; set; }
        /// <summary>
        /// If set, this parameter will send an email receipt to the customer's email.
        /// </summary>
        // TODO: While these are bools here and are listed to be bools in the docs, they use 1 and 0 instead of true and false. Double checkerino these.
        [JsonPropertyName("send_receipt")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool SendReceipt { get; set; }
        /// <summary>
        /// Set to true to bypass duplicate detection/folding.
        /// </summary>
        [JsonPropertyName("ignore_duplicate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IgnoreDuplicate { get; set; }
        /// <summary>
        /// Email where merchant receipt should be sent.
        /// </summary>
        [JsonPropertyName("merchemailaddr")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string MerchEmailAddress { get; set; }
        /// <summary>
        /// Total transaction amount (Including tax, tip, shipping, etc.) (Required)
        /// </summary>
        [JsonPropertyName("amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public double Amount { get; set; }
        /// <summary>
        /// Set to true to save the customer information to the customer database
        /// </summary>
        [JsonPropertyName("save_customer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool SaveCustomer { get; set; }
        /// <summary>
        /// Set to true to save the customer payment method to customer profile. You must either have the save_customer flag set to true in the transaction OR pass in the custkey to attach transaction to existing customer.
        /// </summary>
        [JsonPropertyName("save_customer_paymethod")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool SaveCustomerPaymentMethod { get; set; }
        /// <summary>
        /// Currency numerical code. Defaults to 840 (USD).
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/currencycodes/#currency-codes"/>
        [JsonPropertyName("currency")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Currency { get; set; }
        /// <summary>
        /// Terminal identifier (i.e. multilane)
        /// </summary>
        [JsonPropertyName("terminal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Terminal { get; set; }
        /// <summary>
        /// Clerk/Cashier/Server name
        /// </summary>
        [JsonPropertyName("clerk")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Clerk { get; set; }
        /// <summary>
        /// IP address of client. Used in conjunction with the Block By Host or IP fraud module.
        /// </summary>
        [JsonPropertyName("clientip")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ClientIP { get; set; }
        /// <summary>
        /// Software name and version (useful for troubleshooting)
        /// </summary>
        [JsonPropertyName("software")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Software { get; set; }
        /// <summary>
        /// The name of the receipt template that should be used when sending a customer receipt.
        /// </summary>
        [JsonPropertyName("receipt-custemail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ReceiptCustomerEmail { get; set; }
        /// <summary>
        /// The name of the receipt template that should be used when sending a merchant receipt.
        /// </summary>
        [JsonPropertyName("receipt-merchemail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ReceiptMerchantEmail { get; set; }
        /// <summary>
        /// Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
        /// </summary>
        [JsonPropertyName("amount_detail")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public AmountDetail AmountDetail { get; set; }

        /// <summary>
        /// Object holding credit card/token information for card payments 
        /// </summary>
        [JsonPropertyName("creditcard")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public CreditCard CreditCard { get; set; }

        /// <summary>
        /// Object holding check information for check payments
        /// </summary>
        [JsonPropertyName("check")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Check Check { get; set; }

        /// <summary>
        /// Set to true to tokenize the card used to process the transaction.
        /// </summary>
        [JsonPropertyName("save_card")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool SaveCard { get; set; }

        /// <summary>
        /// An object holding transaction characteristics.
        /// </summary>
        [JsonPropertyName("traits")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Traits Traits { get; set; }

        /// <summary>
        /// Customer key for a previously saved customer. Unique gateway generated key.
        /// </summary>
        [JsonPropertyName("custkey")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string CustomerKey { get; set; }

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
        /// Array of line items attached to the transaction.
        /// </summary>
        [JsonPropertyName("lineitems")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<LineItem> LineItems { get; set; }

        /// <summary>
        /// Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
        /// </summary>
        [JsonPropertyName("custom_fields")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Dictionary<string, string> CustomFields { get; set; }
    }

    public partial class AmountDetail
    {
        /// <summary>
        /// This field is optional, but if it is sent, it must be consistent with the following equation: amount = subtotal - discount + shipping + duty + tax + tip.
        /// </summary>
        [JsonPropertyName("subtotal")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double Subtotal { get; set; }

        /// <summary>
        /// The amount of tax collected.
        /// </summary>
        [JsonPropertyName("tax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double Tax { get; set; }

        /// <summary>
        /// Transaction is non taxable 
        /// </summary>
        [JsonPropertyName("nontaxable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool NonTaxable { get; set; }

        /// <summary>
        /// Amount of tip collected.
        /// </summary>
        [JsonPropertyName("tip")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double Tip { get; set; }

        /// <summary>
        /// Amount of discount applied to total transaction.
        /// </summary>
        [JsonPropertyName("discount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double Discount { get; set; }

        /// <summary>
        /// Amount of shipping fees collected.
        /// </summary>
        [JsonPropertyName("shipping")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double Shipping { get; set; }

        /// <summary>
        /// Amount of duty collected.
        /// </summary>
        [JsonPropertyName("duty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double Duty { get; set; }

        /// <summary>
        /// Enable partial amount authorization. If the available card balance is less than the amount request, the balance will be authorized and the POS must prompt the customer for another payment to cover the remainder. The result_code will be "P" and auth_amount will contain the partial amount that was approved.
        /// </summary>
        [JsonPropertyName("enable_partialauth")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool EnablePartialAuth { get; set; }
    }

    public partial class Address
    {
        /// <summary>
        /// Company or Organization Name
        /// </summary>
        [JsonPropertyName("company")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Company { get; set; }
        /// <summary>
        /// First name associated with billing address
        /// </summary>
        [JsonPropertyName("first_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name associated with billing address
        /// </summary>
        [JsonPropertyName("last_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string LastName { get; set; }
        /// <summary>
        /// Primary street number/address information. (i.e. 1234 Main Street)
        /// </summary>
        [JsonPropertyName("street")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Street { get; set; }
        /// <summary>
        /// Additional address information such as apartment number, building number, suite information, etc.
        /// </summary>
        [JsonPropertyName("street2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Street2 { get; set; }
        /// <summary>
        /// Billing city
        /// </summary>
        [JsonPropertyName("city")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string City { get; set; }
        /// <summary>
        /// Two-letter State abbreviation or full state name.
        /// </summary>
        [JsonPropertyName("state")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string State { get; set; }
        /// <summary>
        /// Zip code
        /// </summary>
        [JsonPropertyName("postalcode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string PostalCode { get; set; }
        /// <summary>
        /// Three-letter country code. 
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3"/>
        [JsonPropertyName("country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Country { get; set; }
        /// <summary>
        /// The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
        /// </summary>
        [JsonPropertyName("phone")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Phone { get; set; }
        /// <summary>
        /// The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
        /// </summary>
        [JsonPropertyName("fax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Fax { get; set; }
    }

    public partial class CreditCard
    {
        /// <summary>
        /// Name of the Cardholder
        /// </summary>
        [JsonPropertyName("cardholder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string CardHolder { get; set; }
        /// <summary>
        /// Credit card number or token. (Required)
        /// </summary>
        [JsonPropertyName("number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Number { get; set; }
        /// <summary>
        /// Credit card expiration date. All numbers, and needs to be formatted as MMYY. (Required)
        /// </summary>
        [JsonPropertyName("expiration")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Expiration { get; set; }
        /// <summary>
        /// Card verification code on back of card. Its format should be ### or ####. When tokenizing a credit card payment, this does NOT get saved and MUST be included with token transactions.
        /// </summary>
        [JsonPropertyName("cvc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Cvc { get; set; }
        /// <summary>
        /// Street address for address verification
        /// </summary>
        [JsonPropertyName("avs_street")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AvsStreet { get; set; }
        /// <summary>
        /// Postal (Zip) code for address verification
        /// </summary>
        [JsonPropertyName("avs_zip")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AvsZip { get; set; }
    }

    public partial class Check
    {
        /// <summary>
        /// Account holder name (Required)
        /// </summary>
        [JsonPropertyName("accountholder")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AccountHolder { get; set; }
        /// <summary>
        /// Bank Routing Number (Required)
        /// </summary>
        [JsonPropertyName("routing")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Routing { get; set; }
        /// <summary>
        /// Bank Account Number (Required)
        /// </summary>
        [JsonPropertyName("account")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Account { get; set; }
        /// <summary>
        /// Checking or Savings
        /// </summary>
        [JsonPropertyName("account_type")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string AccountType { get; set; }
        /// <summary>
        /// Check number
        /// </summary>
        [JsonPropertyName("number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Number { get; set; }
        /// <summary>
        /// SEC Record type
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/checkformat/"/>
        [JsonPropertyName("format")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Format { get; set; }
        /// <summary>
        /// Comma delimited list of special check process flags. Not needed for most scenarios. Available flags: prenote, sameday
        /// </summary>
        [JsonPropertyName("flags")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Flags { get; set; }
    }

    public partial class LineItem
    {
        /// <summary>
        /// Gateway generated unique product identifier. Will only be included if the line item is a product from the database.
        /// </summary>
        [JsonPropertyName("product_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string ProductKey { get; set; }
        /// <summary>
        /// Product name. (Required)
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }
        /// <summary>
        /// Cost of line item. (Required)
        /// </summary>
        [JsonPropertyName("cost")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double Cost { get; set; }
        /// <summary>
        /// Quantity of products. (Required)
        /// </summary>
        [JsonPropertyName("qty")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Quantity { get; set; }
        /// <summary>
        /// Line item description.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Description { get; set; }
        /// <summary>
        /// This is the product’s Stock Keeping Unit number.
        /// </summary>
        [JsonPropertyName("sku")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string StockKeepingUnitNumber { get; set; }
        /// <summary>
        /// Denotes if line item is taxable.
        /// </summary>
        [JsonPropertyName("taxable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Taxable { get; set; }
        /// <summary>
        /// Tax amount that should be applied to line item price.
        /// </summary>
        [JsonPropertyName("tax_amount")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double TaxAmount { get; set; }
        /// <summary>
        /// Tax percentage that should be applied to line item amount.
        /// </summary>
        [JsonPropertyName("tax_rate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string TaxRate { get; set; }
        /// <summary>
        /// Discount percentage that should be applied to line item amount.
        /// </summary>
        [JsonPropertyName("discount_rate")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string DiscountRate { get; set; }
        /// <summary>
        /// Discount amount that should be applied to line item amount.
        /// </summary>
        [JsonPropertyName("discount_amount")]
        [JsonConverter(typeof(ParseStringToDoubleConverter))]
        public double DiscountAmount { get; set; }
        /// <summary>
        /// Unique identifier for warehouse location.
        /// </summary>
        [JsonPropertyName("location_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string LocationKey { get; set; }
        /// <summary>
        /// Commodity code for product.
        /// </summary>
        [JsonPropertyName("commodity_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string CommodityCode { get; set; }
        /// <summary>
        /// Unit of measure (Required for Level 3 processing)
        /// </summary>
        [JsonPropertyName("um")]
        public string UnitMeasure { get; set; }
    }

    public partial class Traits
    {
        /// <summary>
        /// Set to true if this transaction is to pay an existing debt. Click link in see more for more information. Defaults to false.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/existingdebt/"/>
        [JsonPropertyName("is_debt")]
        public bool? IsDebt { get; set; }
        /// <summary>
        /// Set to true if this transaction is a bill pay transaction. Defaults to false.
        /// </summary>
        [JsonPropertyName("is_bill_pay")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? IsBillPay { get; set; }
        /// <summary>
        /// Set to true if this transaction is a recurring transaction. Defaults to false.
        /// </summary>
        [JsonPropertyName("is_recurring")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? IsRecurring { get; set; }
        /// <summary>
        /// Set to true if this transaction is a healthcare transaction. Defaults to false.
        /// </summary>
        [JsonPropertyName("is_healthcare")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? IsHealthcare { get; set; }
        /// <summary>
        /// Set to true if this transaction contains a cash advance. Defaults to false.
        /// </summary>
        [JsonPropertyName("is_cash_advance")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? IsCashAdvance { get; set; }
        /// <summary>
        /// Pass through UCAF Collection Indicator here for MasterCard secure transactions.
        /// </summary>
        [JsonPropertyName("secure_collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        [JsonConverter(typeof(ParseStringToLongConverter))]

        public long SecureCollection { get; set; }
        /// <summary>
        /// This flag indicates either that merchant is about to store the card data for future use or that the current transaction is being run using data from a card stored in the merchant’s system. When the card is being stored this flag indicates what the intended future use will be. Options are available at the provided link.
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/?#stored-credential"/>
        [JsonPropertyName("stored_credential")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string StoredCredential { get; set; }
    }
}