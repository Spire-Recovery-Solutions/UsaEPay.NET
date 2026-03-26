using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models
{
    public interface IUsaEPayResponse
    {   /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }
    }
}
