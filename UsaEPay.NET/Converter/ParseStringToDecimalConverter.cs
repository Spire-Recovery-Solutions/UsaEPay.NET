using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsaEPay.NET.Converter
{
    public class ParseStringToDecimalConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(decimal) || t == typeof(decimal?);

        public override object ReadJson(JsonReader reader, Type t, object? existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            decimal l;
            if (decimal.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type decimal");
        }

        public override void WriteJson(JsonWriter writer, object? untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (decimal)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringToDecimalConverter Singleton = new ParseStringToDecimalConverter();
    }
}
