using System.Text.Json.Serialization;

namespace UsaEPay.NET.Models.Classes
{
    /// <summary>
    /// Represents the parameters for a USA ePay transaction.
    /// </summary>
    public class UsaEPayTransactionParams
    {
        [JsonPropertyName("Amount")]
        public decimal Amount { get; set; }

        public string? Token { get; set; }

        [JsonPropertyName("trankey")]
        public string? TransactionKey { get; set; }

        public string? Refnum { get; set; }

        public string? CustomerKey { get; set; }

        public string? PaymentMethodKey { get; set; }

        public string? PaymentKey { get; set; }

        [JsonPropertyName("FirstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string? LastName { get; set; }

        [JsonPropertyName("Address")]
        public string? Address { get; set; }

        [JsonPropertyName("Address2")]
        public string? Address2 { get; set; }

        [JsonPropertyName("City")]
        public string? City { get; set; }

        [JsonPropertyName("State")]
        public string? State { get; set; }

        [JsonPropertyName("Zip")]
        public string? Zip { get; set; }

        [JsonPropertyName("Country")]
        public string? Country { get; set; }

        [JsonPropertyName("Phone")]
        public string? Phone { get; set; }

        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("CardHolder")]
        public string? CardHolder { get; set; }

        [JsonPropertyName("AccountHolder")]
        public string? AccountHolder { get; set; }

        [JsonPropertyName("AccountNumber")]
        public string? AccountNumber { get; set; }

        [JsonPropertyName("CardNumber")]
        public string? CardNumber { get; set; }

        [JsonPropertyName("Expiration")]
        public string? Expiration { get; set; }

        [JsonPropertyName("Cvc")]
        public string? Cvc { get; set; }

        [JsonPropertyName("Invoice")]
        public string? Invoice { get; set; }

        [JsonPropertyName("OrderId")]
        public string? OrderId { get; set; }

        [JsonPropertyName("ClientIP")]
        public string? ClientIp { get; set; }

        [JsonPropertyName("Routing")]
        public string? Routing { get; set; }

        [JsonPropertyName("Account")]
        public string? Account { get; set; }

        [JsonPropertyName("AccountType")]
        public string? AccountType { get; set; }

        [JsonPropertyName("CheckNumber")]
        public string? CheckNumber { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("AuthCode")]
        public string? AuthCode { get; set; }
    }
}
