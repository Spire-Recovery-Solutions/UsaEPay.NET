using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Converter
{
    public class ParseStringToLongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return 0; // or throw an exception if null should not be treated as a default value

            var value = reader.GetString();
            if (long.TryParse(value, out long result))
                return result;

            throw new JsonException($"Cannot convert '{value}' to {typeToConvert}.");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}