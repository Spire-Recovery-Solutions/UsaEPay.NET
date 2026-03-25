using System.Text.Json.Serialization;

namespace UsaEPay.NET.Models.Events
{
    public class ProductInventoryEventResponse : BaseEventResponse
    {
        [JsonPropertyName("event_body")]
        public ProductInventoryEventBody EventBody { get; set; }
    }

    public class ProductInventoryEventBody : BaseEventBody
    {
        [JsonPropertyName("object")]
        public ProductInventoryObject Object { get; set; }
    }

    public class ProductInventoryObject
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("locationid")]
        public string LocationId { get; set; }

        [JsonPropertyName("qtyonhand")]
        public long QtyOnHand { get; set; }

        [JsonPropertyName("qtyonhand_change")]
        public string QtyOnHandChange { get; set; }

        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
