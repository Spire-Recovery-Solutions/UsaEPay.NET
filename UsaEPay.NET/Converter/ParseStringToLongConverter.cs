using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Converter
{
    public class ParseStringToLongConverter : JsonConverter<long>
    {
        public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return 0; // or throw an exception if null should not be treated as a default value

                case JsonTokenType.String:
                    if (long.TryParse(reader.GetString(), out long result))
                        return result;
                    break;

                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out result))
                        return result;
                    break;
            }

            throw new JsonException($"Cannot convert '{reader.GetString()}' to {typeToConvert}.");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}