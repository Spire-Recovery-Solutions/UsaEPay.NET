using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayProductResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The object type. Successful calls will always return product.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gateway generated product identifier.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// Gateway generated product identifier.
        /// </summary>
        [JsonPropertyName("product_refnum")]
        public string ProductRefnum { get; set; }

        /// <summary>
        /// Product name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Product cost.
        /// </summary>
        [JsonPropertyName("price")]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Price { get; set; }

        /// <summary>
        /// If set to Y, product is enabled.
        /// </summary>
        [JsonPropertyName("enabled")]
        public string Enabled { get; set; }

        /// <summary>
        /// If set to Y, product is taxable.
        /// </summary>
        [JsonPropertyName("taxable")]
        public string Taxable { get; set; }

        /// <summary>
        /// If set to Y, product is available.
        /// </summary>
        [JsonPropertyName("available_all")]
        public string AvailableAll { get; set; }

        /// <summary>
        /// Date product will become available. Format is YYYY-MM-DD.
        /// </summary>
        [JsonPropertyName("available_all_date")]
        public string AvailableAllDate { get; set; }

        /// <summary>
        /// Gateway generated category identifier.
        /// </summary>
        [JsonPropertyName("categoryid")]
        public string CategoryId { get; set; }

        /// <summary>
        /// Commodity code for product.
        /// </summary>
        [JsonPropertyName("commodity_code")]
        public string CommodityCode { get; set; }

        /// <summary>
        /// Date product will become available.
        /// </summary>
        [JsonPropertyName("date_available")]
        public string DateAvailable { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// URL where product image is hosted.
        /// </summary>
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Recommended listing price.
        /// </summary>
        [JsonPropertyName("list_price")]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal ListPrice { get; set; }

        /// <summary>
        /// Manufacturer of product.
        /// </summary>
        [JsonPropertyName("manufacturer")]
        public string Manufacturer { get; set; }

        /// <summary>
        /// Merchant assigned product identifier.
        /// </summary>
        [JsonPropertyName("merch_productid")]
        public string MerchProductId { get; set; }

        /// <summary>
        /// When the inventory reaches this quantity, you will see a low inventory flag on this product.
        /// </summary>
        [JsonPropertyName("min_quantity")]
        public string MinQuantity { get; set; }

        /// <summary>
        /// Product model.
        /// </summary>
        [JsonPropertyName("model")]
        public string Model { get; set; }

        /// <summary>
        /// If set to Y the product is Physical. If set to N the product is Virtual.
        /// </summary>
        [JsonPropertyName("physicalgood")]
        public string PhysicalGood { get; set; }

        /// <summary>
        /// This is the product's weight adjusted for packing and shipping purposes.
        /// </summary>
        [JsonPropertyName("ship_weight")]
        public decimal ShipWeight { get; set; }

        /// <summary>
        /// This is the product's Stock Keeping Unit number.
        /// </summary>
        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        /// <summary>
        /// Product tax class.
        /// </summary>
        [JsonPropertyName("taxclass")]
        public string TaxClass { get; set; }

        /// <summary>
        /// Unit of measure.
        /// </summary>
        [JsonPropertyName("um")]
        public string Um { get; set; }

        /// <summary>
        /// This is the product's Universal Product Code.
        /// </summary>
        [JsonPropertyName("upc")]
        public string Upc { get; set; }

        /// <summary>
        /// Product URL.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// This is the product's weight.
        /// </summary>
        [JsonPropertyName("weight")]
        public decimal Weight { get; set; }

        /// <summary>
        /// Wholesale price.
        /// </summary>
        [JsonPropertyName("wholesale_price")]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal WholesalePrice { get; set; }

        /// <summary>
        /// Set to true to allow users to change product price in console.
        /// </summary>
        [JsonPropertyName("allow_override")]
        public bool AllowOverride { get; set; }

        /// <summary>
        /// Array of inventory objects associated with the product.
        /// </summary>
        [JsonPropertyName("inventory")]
        public List<ProductInventory> Inventory { get; set; }

        /// <summary>
        /// Array of modifier objects associated with the product.
        /// </summary>
        [JsonPropertyName("modifiers")]
        public List<object> Modifiers { get; set; }

        /// <summary>
        /// Date and time the product was created.
        /// </summary>
        [JsonPropertyName("created")]
        public string Created { get; set; }

        /// <summary>
        /// Date and time the product was last modified.
        /// </summary>
        [JsonPropertyName("modified")]
        public string Modified { get; set; }

        /// <summary>
        /// Indicates if the product has inventory.
        /// </summary>
        [JsonPropertyName("has_inventory")]
        public bool? HasInventory { get; set; }

        /// <summary>
        /// If the response is a delete status result.
        /// </summary>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Error message if the request failed.
        /// </summary>
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }

    public class ProductInventory
    {
        /// <summary>
        /// Location identifier.
        /// </summary>
        [JsonPropertyName("locationid")]
        public string LocationId { get; set; }

        /// <summary>
        /// Merchant assigned location identifier.
        /// </summary>
        [JsonPropertyName("merch_locationid")]
        public string MerchLocationId { get; set; }

        /// <summary>
        /// Location name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Location description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Inventory identifier.
        /// </summary>
        [JsonPropertyName("inventoryid")]
        public string InventoryId { get; set; }

        /// <summary>
        /// Product identifier.
        /// </summary>
        [JsonPropertyName("productid")]
        public string ProductId { get; set; }

        /// <summary>
        /// Quantity on hand.
        /// </summary>
        [JsonPropertyName("qtyonhand")]
        public string QtyOnHand { get; set; }

        /// <summary>
        /// Quantity on order.
        /// </summary>
        [JsonPropertyName("qtyonorder")]
        public string QtyOnOrder { get; set; }

        /// <summary>
        /// Date inventory is available.
        /// </summary>
        [JsonPropertyName("date_available")]
        public string DateAvailable { get; set; }

        /// <summary>
        /// Inventory key.
        /// </summary>
        [JsonPropertyName("key")]
        public string Key { get; set; }

        /// <summary>
        /// Location key.
        /// </summary>
        [JsonPropertyName("location_key")]
        public string LocationKey { get; set; }
    }

    public class UsaEPayProductListResponse : IUsaEPayResponse
    {
        /// <summary>
        /// Timestamp for the response.
        /// </summary>
        [JsonConverter(typeof(USAePayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// The type of object returned. Returns a list.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        /// <summary>
        /// The maximum amount of products that will be included in response.
        /// </summary>
        [JsonPropertyName("limit")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Limit { get; set; }

        /// <summary>
        /// The number of products skipped from the results.
        /// </summary>
        [JsonPropertyName("offset")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Offset { get; set; }

        /// <summary>
        /// An array of products matching the request.
        /// </summary>
        [JsonPropertyName("data")]
        public UsaEPayProductResponse[] Data { get; set; }

        /// <summary>
        /// The total amount of products, including filtered results.
        /// </summary>
        [JsonPropertyName("total")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Total { get; set; }
    }
}
