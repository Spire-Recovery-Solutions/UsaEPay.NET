using System.Globalization;
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
                    var text = reader.GetString();
                    // USAePay sends "" for an absent number (e.g. error_code on an approval); read it the way null is read.
                    if (string.IsNullOrWhiteSpace(text))
                        return 0;
                    if (long.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out long result))
                        return result;
                    break;

                case JsonTokenType.Number:
                    if (reader.TryGetInt64(out result))
                        return result;
                    break;
            }

            throw new JsonException($"Cannot convert token type '{reader.TokenType}' to {typeToConvert}.");
        }

        public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}