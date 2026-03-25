using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Converter
{
    public class ParseStringToDecimalConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return 0m;

                case JsonTokenType.String:
                    if (decimal.TryParse(reader.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                        return value;
                    break;

                case JsonTokenType.Number:
                    return reader.GetDecimal();
            }

            throw new JsonException($"Cannot convert to decimal");
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
