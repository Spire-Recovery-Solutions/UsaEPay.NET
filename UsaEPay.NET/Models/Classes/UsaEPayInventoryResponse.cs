using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayInventoryResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Object type. Successful calls will always return "product_inventory".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gateway generated inventory identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        /// <summary>
        /// Quantity on hand for this product/location combination.
        /// </summary>
        [JsonPropertyName("qtyonhand")]
        public string? QtyOnHand { get; set; }

        /// <summary>
        /// Quantity on order for this product/location combination.
        /// </summary>
        [JsonPropertyName("qtyonorder")]
        public string? QtyOnOrder { get; set; }

        /// <summary>
        /// Date product will become available.
        /// </summary>
        [JsonPropertyName("date_available")]
        public string? DateAvailable { get; set; }

        /// <summary>
        /// Product object.
        /// </summary>
        [JsonPropertyName("product")]
        public InventoryProduct? Product { get; set; }

        /// <summary>
        /// Location (warehouse) object.
        /// </summary>
        [JsonPropertyName("location")]
        public UsaEPayInventoryLocationResponse? Location { get; set; }

        /// <summary>
        /// Status of the delete operation. Returned as "success" when an inventory is deleted.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    public class InventoryProduct
    {
        /// <summary>
        /// Object type. Returns "product".
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// Gateway generated unique product identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string? Key { get; set; }

        /// <summary>
        /// Product reference number.
        /// </summary>
        [JsonPropertyName("product_refnum")]
        public string? ProductRefNum { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Product price.
        /// </summary>
        [JsonPropertyName("price")]
        public string? Price { get; set; }

        /// <summary>
        /// Whether the product is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public string? Enabled { get; set; }

        /// <summary>
        /// Whether the product is taxable.
        /// </summary>
        [JsonPropertyName("taxable")]
        public string? Taxable { get; set; }

        /// <summary>
        /// Product SKU number.
        /// </summary>
        [JsonPropertyName("sku")]
        public string? Sku { get; set; }
    }

    public class UsaEPayInventoryListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The type of object returned. Returns a list.
        /// </summary>
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        /// <summary>
        /// The maximum amount of inventories that will be included in response.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }

        /// <summary>
        /// The number of inventories skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }

        /// <summary>
        /// An array of inventories matching the request.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayInventoryResponse[]? Data { get; set; }

        /// <summary>
        /// The total amount of inventories, including filtered results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
}
