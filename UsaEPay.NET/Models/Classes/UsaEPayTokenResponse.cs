using System.Text.Json.Serialization;

namespace UsaEPay.NET.Models.Classes
{
    /// <summary>
    /// Response from token detail endpoints (GET /tokens/:token: and POST /tokens).
    /// </summary>
    public class UsaEPayTokenResponse : IUsaEPayResponse
    {
        public DateTimeOffset? Timestamp { get; set; }

        [JsonPropertyName("cardref")]
        public string CardRef { get; set; }

        [JsonPropertyName("masked_card_number")]
        public string MaskedCardNumber { get; set; }

        [JsonPropertyName("card_type")]
        public string CardType { get; set; }
    }

    /// <summary>
    /// Individual result from a bulk token creation request.
    /// </summary>
    public class UsaEPayBulkTokenResult
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}
