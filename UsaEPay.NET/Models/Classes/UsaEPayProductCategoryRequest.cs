using System.Text.Json.Serialization;
using RestSharp;

namespace UsaEPay.NET.Models.Classes
{
    public class UsaEPayProductCategoryRequest : IUsaEPayRequest
    {
        [JsonIgnore]
        public string Endpoint { get; set; }
        [JsonIgnore]
        public Method RequestType { get; set; }

        /// <summary>
        /// Product category name. (Required)
        /// </summary>
        [JsonPropertyName("name")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Name { get; set; }

        /// <summary>
        /// Array of modifiers associated with the category.
        /// </summary>
        [JsonPropertyName("modifiers")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public List<object> Modifiers { get; set; }
    }
}
