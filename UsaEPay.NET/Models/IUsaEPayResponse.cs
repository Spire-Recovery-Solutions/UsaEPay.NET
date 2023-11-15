using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models
{
    public interface IUsaEPayResponse
    {   /// <summary>
        /// Timestamp for transaction.
        /// </summary>
        [JsonConverter(typeof(DateTimeOffsetToUtcMillisecondStringConverter))]
        public DateTimeOffset Timestamp { get; set; }
    }
}
