using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayDeviceResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Object type. This will always be device.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Unique device identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        /// <summary>
        /// The id of API key (source key) associated with the device.
        /// </summary>
        [JsonPropertyName("apikeyid")]
        public string? ApiKeyId { get; set; }

        /// <summary>
        /// Terminal type: "standalone" for payment engine cloud based terminal.
        /// </summary>
        [JsonPropertyName("terminal_type")]
        public string? TerminalType { get; set; }

        /// <summary>
        /// Current device status.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Developer assigned device name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Device settings.
        /// </summary>
        [JsonPropertyName("settings")]
        public DeviceSettings? Settings { get; set; }

        /// <summary>
        /// Details of terminal.
        /// </summary>
        [JsonPropertyName("terminal_info")]
        public TerminalInfo? TerminalInfo { get; set; }

        /// <summary>
        /// Terminal configuration.
        /// </summary>
        [JsonPropertyName("terminal_config")]
        public TerminalConfig? TerminalConfig { get; set; }

        /// <summary>
        /// If terminal type is 'standalone', this is the pairing code required to pair the payment device with the payment engine.
        /// </summary>
        [JsonPropertyName("pairing_code")]
        public string? PairingCode { get; set; }

        /// <summary>
        /// If terminal type is 'standalone', the expiration is the date/time that the pairing code is no longer valid.
        /// </summary>
        [JsonPropertyName("expiration")]
        public string? Expiration { get; set; }
    }

    public class UsaEPayDeviceListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Object type. This will always be list.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// The max number of items returned in each result set.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }

        /// <summary>
        /// The number of devices skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }

        /// <summary>
        /// An array of devices matching the request.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayDeviceResponse[]? Data { get; set; }

        /// <summary>
        /// The total number of devices.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
}
