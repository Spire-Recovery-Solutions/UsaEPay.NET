using System.Text.Json.Serialization;

namespace UsaEPay.NET.Models.Classes
{
    /// <summary>
    /// Response wrapper from POST /tokens (create token from transaction).
    /// The API returns {"token": {"cardref": "...", "masked_card_number": "...", "card_type": "..."}}.
    /// </summary>
    public class UsaEPayTokenResponse : IUsaEPayResponse
    {
        public DateTimeOffset? Timestamp { get; set; }

        [JsonPropertyName("token")]
        public TokenDetail? Token { get; set; }
    }

    /// <summary>
    /// Token detail fields returned by the API.
    /// </summary>
    public class TokenDetail
    {
        [JsonPropertyName("cardref")]
        public string? CardRef { get; set; }

        [JsonPropertyName("masked_card_number")]
        public string? MaskedCardNumber { get; set; }

        [JsonPropertyName("card_type")]
        public string? CardType { get; set; }
    }

    /// <summary>
    /// Individual result from a bulk token creation request.
    /// </summary>
    public class UsaEPayBulkTokenResult
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("key")]
        public string? Key { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
