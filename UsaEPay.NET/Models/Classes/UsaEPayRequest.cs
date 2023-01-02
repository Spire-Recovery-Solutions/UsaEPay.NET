using Newtonsoft.Json;
using RestSharp;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayRequest : IUsaEPayRequest
    {
        public string Endpoint { get; set; }
        //public Type ResponseType { get; set; }
        public Method RequestType { get; set; }

        /// <summary>
        /// This must be set to sale for a credit or debit card sale (Required)
        /// </summary>
        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)] 
        public string Command { get; set; }
        /// <summary>
        /// One time use token provided by Client JS Library
        /// </summary>
        [JsonProperty("payment_key", NullValueHandling = NullValueHandling.Ignore)] 
        public string PaymentKey { get; set; }
        /// <summary>
        /// Unique gateway generated transaction key, for the transaction you are referencing with this QuickSale
        /// </summary>
        [JsonProperty("trankey", NullValueHandling = NullValueHandling.Ignore)]
        public string TransactionKey { get; set; }
        /// <summary>
        /// Custom Invoice Number to easily retrieve sale details.
        /// </summary>
        [JsonProperty("invoice", NullValueHandling = NullValueHandling.Ignore)] 
        public string Invoice { get; set; }
        /// <summary>
        /// Customer's purchase order number. Required for level 3 processing.
        /// </summary>
        [JsonProperty("ponum", NullValueHandling = NullValueHandling.Ignore)] 
        public string Ponum { get; set; }
        /// <summary>
        /// Merchant assigned order identifier.
        /// </summary>
        [JsonProperty("orderid", NullValueHandling = NullValueHandling.Ignore)] 
        public string OrderId { get; set; }
        /// <summary>
        /// Public description of the transaction.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)] 
        public string Description { get; set; }
        /// <summary>
        /// Private comment details only visible to the merchant.
        /// </summary>
        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)] 
        public string Comments { get; set; }
        /// <summary>
        /// Customer's email address
        /// </summary>
        [JsonProperty("email", NullValueHandling = NullValueHandling.Ignore)] 
        public string Email { get; set; }
        /// <summary>
        /// If set, this parameter will send an email receipt to the customer's email.
        /// </summary>
        // TODO: While these are bools here and are listed to be bools in the docs, they use 1 and 0 instead of true and false. Double checkerino these.
        [JsonProperty("send_receipt", NullValueHandling = NullValueHandling.Ignore)]
        public bool SendReceipt { get; set; }
        /// <summary>
        /// Set to true to bypass duplicate detection/folding.
        /// </summary>
        [JsonProperty("ignore_duplicate", NullValueHandling = NullValueHandling.Ignore)]
        public bool IgnoreDuplicate { get; set; }
        /// <summary>
        /// Email where merchant receipt should be sent.
        /// </summary>
        [JsonProperty("merchemailaddr", NullValueHandling = NullValueHandling.Ignore)]
        public string MerchEmailAddress { get; set; }
        /// <summary>
        /// Total transaction amount (Including tax, tip, shipping, etc.) (Required)
        /// </summary>
        [JsonProperty("amount", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Amount { get; set; }
        /// <summary>
        /// Set to true to save the customer information to the customer database
        /// </summary>
        [JsonProperty("save_customer", NullValueHandling = NullValueHandling.Ignore)]
        public bool SaveCustomer { get; set; }
        /// <summary>
        /// Set to true to save the customer payment method to customer profile. You must either have the save_customer flag set to true in the transaction OR pass in the custkey to attach transaction to existing customer.
        /// </summary>
        [JsonProperty("save_customer_paymethod", NullValueHandling = NullValueHandling.Ignore)]
        public bool SaveCustomerPaymentMethod { get; set; }
        /// <summary>
        /// Currency numerical code. Defaults to 840 (USD).
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/currencycodes/#currency-codes"/>
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; }
        /// <summary>
        /// Terminal identifier (i.e. multilane)
        /// </summary>
        [JsonProperty("terminal", NullValueHandling = NullValueHandling.Ignore)]
        public string Terminal { get; set; }
        /// <summary>
        /// Clerk/Cashier/Server name
        /// </summary>
        [JsonProperty("clerk", NullValueHandling = NullValueHandling.Ignore)]
        public string Clerk { get; set; }
        /// <summary>
        /// IP address of client. Used in conjunction with the Block By Host or IP fraud module.
        /// </summary>
        [JsonProperty("clientip", NullValueHandling = NullValueHandling.Ignore)]
        public string ClientIP { get; set; }
        /// <summary>
        /// Software name and version (useful for troubleshooting)
        /// </summary>
        [JsonProperty("software", NullValueHandling = NullValueHandling.Ignore)]
        public string Software { get; set; }
        /// <summary>
        /// The name of the receipt template that should be used when sending a customer receipt.
        /// </summary>
        [JsonProperty("receipt-custemail", NullValueHandling = NullValueHandling.Ignore)]
        public string ReceiptCustomerEmail { get; set; }
        /// <summary>
        /// The name of the receipt template that should be used when sending a merchant receipt.
        /// </summary>
        [JsonProperty("receipt-merchemail", NullValueHandling = NullValueHandling.Ignore)]
        public string ReceiptMerchantEmail { get; set; }
        /// <summary>
        /// Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
        /// </summary>
        [JsonProperty("amount_detail", NullValueHandling = NullValueHandling.Ignore)]
        public AmountDetail AmountDetail { get; set; }

        /// <summary>
        /// Object holding credit card/token information for card payments 
        /// </summary>
        [JsonProperty("creditcard", NullValueHandling = NullValueHandling.Ignore)]
        public CreditCard CreditCard { get; set; }

        /// <summary>
        /// Object holding check information for check payments
        /// </summary>
        [JsonProperty("check", NullValueHandling = NullValueHandling.Ignore)]
        public Check Check { get; set; }

        /// <summary>
        /// Set to true to tokenize the card used to process the transaction.
        /// </summary>
        [JsonProperty("save_card", NullValueHandling = NullValueHandling.Ignore)]
        public bool SaveCard { get; set; }

        /// <summary>
        /// An object holding transaction characteristics.
        /// </summary>
        [JsonProperty("traits", NullValueHandling = NullValueHandling.Ignore)]
        public Traits Traits { get; set; }

        /// <summary>
        /// Customer key for a previously saved customer. Unique gateway generated key.
        /// </summary>
        [JsonProperty("custkey", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomerKey { get; set; }

        /// <summary>
        /// Object which holds the customer's billing address information.
        /// </summary>
        [JsonProperty("billing_address", NullValueHandling = NullValueHandling.Ignore)]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Object which holds the customer's shipping address information.
        /// </summary>
        [JsonProperty("shipping_address", NullValueHandling = NullValueHandling.Ignore)]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Array of line items attached to the transaction.
        /// </summary>
        [JsonProperty("lineitems", NullValueHandling = NullValueHandling.Ignore)]
        public List<LineItem> LineItems { get; set; }

        /// <summary>
        /// Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
        /// </summary>
        [JsonProperty("custom_fields", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> CustomFields { get; set; }
    }

    public partial class AmountDetail
    {
        /// <summary>
        /// This field is optional, but if it is sent, it must be consistent with the following equation: amount = subtotal - discount + shipping + duty + tax + tip.
        /// </summary>
        [JsonProperty("subtotal", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Subtotal { get; set; }

        /// <summary>
        /// The amount of tax collected.
        /// </summary>
        [JsonProperty("tax", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Tax { get; set; }

        /// <summary>
        /// Transaction is non taxable 
        /// </summary>
        [JsonProperty("nontaxable", NullValueHandling = NullValueHandling.Ignore)]
        public bool NonTaxable { get; set; }

        /// <summary>
        /// Amount of tip collected.
        /// </summary>
        [JsonProperty("tip", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Tip { get; set; }

        /// <summary>
        /// Amount of discount applied to total transaction.
        /// </summary>
        [JsonProperty("discount", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Discount { get; set; }

        /// <summary>
        /// Amount of shipping fees collected.
        /// </summary>
        [JsonProperty("shipping", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Shipping { get; set; }

        /// <summary>
        /// Amount of duty collected.
        /// </summary>
        [JsonProperty("duty", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Duty { get; set; }

        /// <summary>
        /// Enable partial amount authorization. If the available card balance is less than the amount request, the balance will be authorized and the POS must prompt the customer for another payment to cover the remainder. The result_code will be "P" and auth_amount will contain the partial amount that was approved.
        /// </summary>
        [JsonProperty("enable_partialauth", NullValueHandling = NullValueHandling.Ignore)]
        public bool EnablePartialAuth { get; set; }
    }

    public partial class Address
    {
        /// <summary>
        /// Company or Organization Name
        /// </summary>
        [JsonProperty("company", NullValueHandling = NullValueHandling.Ignore)]
        public string Company { get; set; }
        /// <summary>
        /// First name associated with billing address
        /// </summary>
        [JsonProperty("firstname", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        /// <summary>
        /// Last name associated with billing address
        /// </summary>
        [JsonProperty("lastname", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }
        /// <summary>
        /// Primary street number/address information. (i.e. 1234 Main Street)
        /// </summary>
        [JsonProperty("street", NullValueHandling = NullValueHandling.Ignore)]
        public string Street { get; set; }
        /// <summary>
        /// Additional address information such as apartment number, building number, suite information, etc.
        /// </summary>
        [JsonProperty("street2", NullValueHandling = NullValueHandling.Ignore)]
        public string Street2 { get; set; }
        /// <summary>
        /// Billing city
        /// </summary>
        [JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }
        /// <summary>
        /// Two-letter State abbreviation or full state name.
        /// </summary>
        [JsonProperty("state", NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }
        /// <summary>
        /// Zip code
        /// </summary>
        [JsonProperty("postalcode", NullValueHandling = NullValueHandling.Ignore)]
        public string PostalCode { get; set; }
        /// <summary>
        /// Three-letter country code. 
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3"/>
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }
        /// <summary>
        /// The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
        /// </summary>
        [JsonProperty("phone", NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }
        /// <summary>
        /// The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
        /// </summary>
        [JsonProperty("fax", NullValueHandling = NullValueHandling.Ignore)]
        public string Fax { get; set; }
    }

    public partial class CreditCard
    {
        /// <summary>
        /// Name of the Cardholder
        /// </summary>
        [JsonProperty("cardholder", NullValueHandling = NullValueHandling.Ignore)]
        public string CardHolder { get; set; }
        /// <summary>
        /// Credit card number or token. (Required)
        /// </summary>
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        /// <summary>
        /// Credit card expiration date. All numbers, and needs to be formatted as MMYY. (Required)
        /// </summary>
        [JsonProperty("expiration", NullValueHandling = NullValueHandling.Ignore)]
        public string Expiration { get; set; }
        /// <summary>
        /// Card verification code on back of card. Its format should be ### or ####. When tokenizing a credit card payment, this does NOT get saved and MUST be included with token transactions.
        /// </summary>
        [JsonProperty("cvc", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long? Cvc { get; set; }
        /// <summary>
        /// Street address for address verification
        /// </summary>
        [JsonProperty("avs_street", NullValueHandling = NullValueHandling.Ignore)]
        public string AvsStreet { get; set; }
        /// <summary>
        /// Postal (Zip) code for address verification
        /// </summary>
        [JsonProperty("avs_zip", NullValueHandling = NullValueHandling.Ignore)]
        public string AvsZip { get; set; }
    }

    public partial class Check
    {
        /// <summary>
        /// Account holder name (Required)
        /// </summary>
        [JsonProperty("accountholder", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountHolder { get; set; }
        /// <summary>
        /// Bank Routing Number (Required)
        /// </summary>
        [JsonProperty("routing", NullValueHandling = NullValueHandling.Ignore)]
        public string Routing { get; set; }
        /// <summary>
        /// Bank Account Number (Required)
        /// </summary>
        [JsonProperty("account", NullValueHandling = NullValueHandling.Ignore)]
        public string Account { get; set; }
        /// <summary>
        /// Checking or Savings
        /// </summary>
        [JsonProperty("account_type", NullValueHandling = NullValueHandling.Ignore)]
        public string AccountType { get; set; }
        /// <summary>
        /// Check number
        /// </summary>
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string Number { get; set; }
        /// <summary>
        /// SEC Record type
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/checkformat/"/>
        [JsonProperty("format", NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }
        /// <summary>
        /// Comma delimited list of special check process flags. Not needed for most scenarios. Available flags: prenote, sameday
        /// </summary>
        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public string Flags { get; set; }
    }

    public partial class LineItem
    {
        /// <summary>
        /// Gateway generated unique product identifier. Will only be included if the line item is a product from the database.
        /// </summary>
        [JsonProperty("product_key", NullValueHandling = NullValueHandling.Ignore)]
        public string ProductKey { get; set; }
        /// <summary>
        /// Product name. (Required)
        /// </summary>
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        /// <summary>
        /// Cost of line item. (Required)
        /// </summary>
        [JsonProperty("cost", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Cost { get; set; }
        /// <summary>
        /// Quantity of products. (Required)
        /// </summary>
        [JsonProperty("qty", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long? Quantity { get; set; }
        /// <summary>
        /// Line item description.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        /// <summary>
        /// This is the product’s Stock Keeping Unit number.
        /// </summary>
        [JsonProperty("sku", NullValueHandling = NullValueHandling.Ignore)] 
        public string StockKeepingUnitNumber { get; set; }
        /// <summary>
        /// Denotes if line item is taxable.
        /// </summary>
        [JsonProperty("taxable", NullValueHandling = NullValueHandling.Ignore)] 
        public bool Taxable { get; set; }
        /// <summary>
        /// Tax amount that should be applied to line item price.
        /// </summary>
        [JsonProperty("tax_amount", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal TaxAmount { get; set; }
        /// <summary>
        /// Tax percentage that should be applied to line item amount.
        /// </summary>
        [JsonProperty("tax_rate", NullValueHandling = NullValueHandling.Ignore)] 
        public string TaxRate { get; set; }
        /// <summary>
        /// Discount percentage that should be applied to line item amount.
        /// </summary>
        [JsonProperty("discount_rate", NullValueHandling = NullValueHandling.Ignore)] 
        public string DiscountRate { get; set; }
        /// <summary>
        /// Discount amount that should be applied to line item amount.
        /// </summary>
        [JsonProperty("discount_amount", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal DiscountAmount { get; set; }
        /// <summary>
        /// Unique identifier for warehouse location.
        /// </summary>
        [JsonProperty("location_key", NullValueHandling = NullValueHandling.Ignore)]
        public string LocationKey { get; set; }
        /// <summary>
        /// Commodity code for product.
        /// </summary>
        [JsonProperty("commodity_code", NullValueHandling = NullValueHandling.Ignore)] 
        public string CommodityCode { get; set; }
        /// <summary>
        /// Unit of measure (Required for Level 3 processing)
        /// </summary>
        [JsonProperty("um", NullValueHandling = NullValueHandling.Ignore)] 
        public string UnitMeasure { get; set; }
    }

    public partial class Traits
    {
        /// <summary>
        /// Set to true if this transaction is to pay an existing debt. Click link in see more for more information. Defaults to false.
        /// </summary>
        /// <see href="https://help.usaepay.info/developer/reference/existingdebt/"/>
        [JsonProperty("is_debt", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsDebt { get; set; }
        /// <summary>
        /// Set to true if this transaction is a bill pay transaction. Defaults to false.
        /// </summary>
        [JsonProperty("is_bill_pay", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsBillPay { get; set; }
        /// <summary>
        /// Set to true if this transaction is a recurring transaction. Defaults to false.
        /// </summary>
        [JsonProperty("is_recurring", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRecurring { get; set; }
        /// <summary>
        /// Set to true if this transaction is a healthcare transaction. Defaults to false.
        /// </summary>
        [JsonProperty("is_healthcare", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsHealthcare { get; set; }
        /// <summary>
        /// Set to true if this transaction contains a cash advance. Defaults to false.
        /// </summary>
        [JsonProperty("is_cash_advance", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCashAdvance { get; set; }
        /// <summary>
        /// Pass through UCAF Collection Indicator here for MasterCard secure transactions.
        /// </summary>
        [JsonProperty("secure_collection", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long? SecureCollection { get; set; }
        /// <summary>
        /// This flag indicates either that merchant is about to store the card data for future use or that the current transaction is being run using data from a card stored in the merchant’s system. When the card is being stored this flag indicates what the intended future use will be. Options are available at the provided link.
        /// </summary>
        /// <see href="https://help.usaepay.info/api/rest/?#stored-credential"/>
        [JsonProperty("stored_credential", NullValueHandling = NullValueHandling.Ignore)] 
        public string StoredCredential { get; set; }
    }
}
