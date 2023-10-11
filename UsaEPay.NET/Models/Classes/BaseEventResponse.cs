using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class BaseEventResponse
    {
        /// <summary>
        /// Object type. This will always be transaction.
        /// </summary>
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        /// <summary>
        /// The timestamp indicating when the event was triggered.
        /// </summary>
        [JsonProperty("event_triggered", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringToDateTimeConvertor))]
        public DateTime EventTriggered { get; set; }

        /// <summary>
        /// Describes the type of the event, e.g., "ach.voided."
        /// </summary>
        [JsonProperty("event_type", NullValueHandling = NullValueHandling.Ignore)]
        public string EventType { get; set; }

        /// <summary>
        /// Unique identifier for the event.
        /// </summary>
        [JsonProperty("event_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EventId { get; set; }
    }
}
