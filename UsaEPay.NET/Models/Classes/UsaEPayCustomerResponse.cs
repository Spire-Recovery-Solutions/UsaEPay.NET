using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayCustomerResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Object type. Successful calls will always return "customer".
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gateway generated customer identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// Merchant assigned customer identifier.
        /// </summary>
        [JsonPropertyName("customerid")]
        public string CustomerId { get; set; }

        /// <summary>
        /// Gateway assigned customer identifier. This was originally used in SOAP API.
        /// </summary>
        [JsonPropertyName("custid")]
        public string CustId { get; set; }

        /// <summary>
        /// Company or Organization Name.
        /// </summary>
        [JsonPropertyName("company")]
        public string Company { get; set; }

        /// <summary>
        /// First name associated with the customer.
        /// </summary>
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name associated with the customer.
        /// </summary>
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Primary street number/address information.
        /// </summary>
        [JsonPropertyName("street")]
        public string Street { get; set; }

        /// <summary>
        /// Additional address information such as apartment number, building number, suite information, etc.
        /// </summary>
        [JsonPropertyName("street2")]
        public string Street2 { get; set; }

        /// <summary>
        /// Billing City.
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }

        /// <summary>
        /// Two-letter State abbreviation or full state name.
        /// </summary>
        [JsonPropertyName("state")]
        public string State { get; set; }

        /// <summary>
        /// Postal code.
        /// </summary>
        [JsonPropertyName("postalcode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Three-letter country code.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        /// The phone number associated with the customer.
        /// </summary>
        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        /// <summary>
        /// The fax number associated with the customer.
        /// </summary>
        [JsonPropertyName("fax")]
        public string Fax { get; set; }

        /// <summary>
        /// Email contact for customer.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Customer's website.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Merchant notes about this customer.
        /// </summary>
        [JsonPropertyName("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Customer description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class UsaEPayCustomerListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The type of object returned. Returns a list.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The maximum amount of customers that will be included in response.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }

        /// <summary>
        /// The number of customers skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }

        /// <summary>
        /// An array of customers matching the request.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayCustomerResponse[] Data { get; set; }

        /// <summary>
        /// The total amount of customers, including filtered results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
}
