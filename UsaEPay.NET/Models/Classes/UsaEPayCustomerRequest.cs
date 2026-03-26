using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayCustomerRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string? Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Merchant assigned customer identifier.
        /// </summary>
        [JsonPropertyName("customerid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? CustomerId { get; set; }

        /// <summary>
        /// Company or Organization Name.
        /// </summary>
        [JsonPropertyName("company")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Company { get; set; }

        /// <summary>
        /// First name associated with the customer.
        /// </summary>
        [JsonPropertyName("first_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? FirstName { get; set; }

        /// <summary>
        /// Last name associated with the customer.
        /// </summary>
        [JsonPropertyName("last_name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? LastName { get; set; }

        /// <summary>
        /// Primary street number/address information.
        /// </summary>
        [JsonPropertyName("street")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Street { get; set; }

        /// <summary>
        /// Additional address information such as apartment number, building number, suite information, etc.
        /// </summary>
        [JsonPropertyName("street2")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Street2 { get; set; }

        /// <summary>
        /// Billing City.
        /// </summary>
        [JsonPropertyName("city")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? City { get; set; }

        /// <summary>
        /// Two-letter State abbreviation or full state name.
        /// </summary>
        [JsonPropertyName("state")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? State { get; set; }

        /// <summary>
        /// Postal code.
        /// </summary>
        [JsonPropertyName("postalcode")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? PostalCode { get; set; }

        /// <summary>
        /// Three-letter country code.
        /// </summary>
        [JsonPropertyName("country")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Country { get; set; }

        /// <summary>
        /// The phone number associated with the customer.
        /// </summary>
        [JsonPropertyName("phone")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Phone { get; set; }

        /// <summary>
        /// The fax number associated with the customer.
        /// </summary>
        [JsonPropertyName("fax")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Fax { get; set; }

        /// <summary>
        /// Email contact for customer.
        /// </summary>
        [JsonPropertyName("email")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Email { get; set; }

        /// <summary>
        /// Customer's website.
        /// </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Url { get; set; }

        /// <summary>
        /// Merchant notes about this customer.
        /// </summary>
        [JsonPropertyName("notes")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Notes { get; set; }

        /// <summary>
        /// Customer description.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Description { get; set; }

        /// <summary>
        /// The transaction key to reference when creating a customer from an existing transaction.
        /// </summary>
        [JsonPropertyName("transaction_key")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? TransactionKey { get; set; }
    }
}
