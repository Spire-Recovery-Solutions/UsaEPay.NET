using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Converter
{
    public class ParseStringToDoubleConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
                return 0;

            if (reader.TokenType == JsonTokenType.String && double.TryParse(reader.GetString(), out var value))
                return value;

            throw new JsonException("Cannot convert to double");
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}