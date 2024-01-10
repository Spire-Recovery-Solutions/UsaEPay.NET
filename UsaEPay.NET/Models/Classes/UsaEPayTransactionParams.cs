using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    /// <summary>
    /// Represents the parameters for a USA ePay transaction.
    /// </summary>
    public class UsaEPayTransactionParams
    {
        [JsonPropertyName("Amount")]
        public double Amount { get; set; }

        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("Address")]
        public string Address { get; set; }

        [JsonPropertyName("Address2")]
        public string Address2 { get; set; }

        [JsonPropertyName("City")]
        public string City { get; set; }

        [JsonPropertyName("State")]
        public string State { get; set; }

        [JsonPropertyName("Zip")]
        public string Zip { get; set; }

        [JsonPropertyName("Country")]
        public string Country { get; set; }

        [JsonPropertyName("Phone")]
        public string Phone { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; }

        [JsonPropertyName("CardHolder")]
        public string CardHolder { get; set; }

        [JsonPropertyName("AccountHolder")]
        public string AccountHolder { get; set; }

        [JsonPropertyName("AccountNumber")]
        public string AccountNumber { get; set; }

        [JsonPropertyName("CardNumber")]
        public string CardNumber { get; set; }

        [JsonPropertyName("Expiration")]
        public string Expiration { get; set; }

        [JsonPropertyName("Cvc")]
        public int Cvc { get; set; }

        [JsonPropertyName("Invoice")]
        public string Invoice { get; set; }

        [JsonPropertyName("OrderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("ClientIP")]
        public string ClientIP { get; set; }

        [JsonPropertyName("Routing")]
        public string Routing { get; set; }

        [JsonPropertyName("Account")]
        public string Account { get; set; }

        [JsonPropertyName("AccountType")]
        public string AccountType { get; set; }

        [JsonPropertyName("CheckNumber")]
        public string CheckNumber { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}