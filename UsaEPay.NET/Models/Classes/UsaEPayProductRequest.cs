using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayProductRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string? Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Product name. (Required)
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Name { get; set; }

        /// <summary>
        /// Product cost.
        /// </summary>
        [JsonPropertyName("price")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Price { get; set; }

        /// <summary>
        /// If set to true, product is enabled. Defaults to false if not passed.
        /// </summary>
        [JsonPropertyName("enabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Enabled { get; set; }

        /// <summary>
        /// If set to true, product is taxable. Defaults to false if not passed.
        /// </summary>
        [JsonPropertyName("taxable")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? Taxable { get; set; }

        /// <summary>
        /// Gateway generated category identifier.
        /// </summary>
        [JsonPropertyName("categoryid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? CategoryId { get; set; }

        /// <summary>
        /// Commodity code for product.
        /// </summary>
        [JsonPropertyName("commodity_code")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? CommodityCode { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        [JsonPropertyName("description")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Description { get; set; }

        /// <summary>
        /// Recommended listing price.
        /// </summary>
        [JsonPropertyName("list_price")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? ListPrice { get; set; }

        /// <summary>
        /// Manufacturer of product.
        /// </summary>
        [JsonPropertyName("manufacturer")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Manufacturer { get; set; }

        /// <summary>
        /// Merchant assigned product identifier.
        /// </summary>
        [JsonPropertyName("merch_productid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? MerchProductId { get; set; }

        /// <summary>
        /// When the inventory reaches this quantity, you will see a low inventory flag on this product.
        /// </summary>
        [JsonPropertyName("min_quantity")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int? MinQuantity { get; set; }

        /// <summary>
        /// Product model.
        /// </summary>
        [JsonPropertyName("model")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Model { get; set; }

        /// <summary>
        /// If set to true the product is Physical (eg. a Hard cover book). If set to false the product is Virtual (eg. an eBook). Defaults to false.
        /// </summary>
        [JsonPropertyName("physicalgood")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? PhysicalGood { get; set; }

        /// <summary>
        /// This is the product's weight adjusted for packing and shipping purposes.
        /// </summary>
        [JsonPropertyName("ship_weight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? ShipWeight { get; set; }

        /// <summary>
        /// This is the product's Stock Keeping Unit number.
        /// </summary>
        [JsonPropertyName("sku")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Sku { get; set; }

        /// <summary>
        /// Product tax class.
        /// </summary>
        [JsonPropertyName("taxclass")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? TaxClass { get; set; }

        /// <summary>
        /// Unit of measure.
        /// </summary>
        [JsonPropertyName("um")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Um { get; set; }

        /// <summary>
        /// This is the product's Universal Product Code.
        /// </summary>
        [JsonPropertyName("upc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Upc { get; set; }

        /// <summary>
        /// Product URL.
        /// </summary>
        [JsonPropertyName("url")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Url { get; set; }

        /// <summary>
        /// If set to true, product is available to all locations.
        /// </summary>
        [JsonPropertyName("available_all")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? AvailableAll { get; set; }

        /// <summary>
        /// Date the product became available to all locations.
        /// </summary>
        [JsonPropertyName("available_all_date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? AvailableAllDate { get; set; }

        /// <summary>
        /// Date the product becomes available.
        /// </summary>
        [JsonPropertyName("date_available")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? DateAvailable { get; set; }

        /// <summary>
        /// If set to true, allows price override on the product.
        /// </summary>
        [JsonPropertyName("allow_override")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool? AllowOverride { get; set; }

        /// <summary>
        /// This is the product's weight.
        /// </summary>
        [JsonPropertyName("weight")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? Weight { get; set; }

        /// <summary>
        /// Wholesale price.
        /// </summary>
        [JsonPropertyName("wholesale_price")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal? WholesalePrice { get; set; }
    }
}
