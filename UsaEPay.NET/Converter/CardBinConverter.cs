using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Converter
{
    /// <summary>
    /// Reads the card BIN (<c>bin.bin</c>) leniently. USAePay documents it as a string such as
    /// <c>"444455"</c>, but the API has been seen to return JSON <c>null</c>, and a numeric or empty
    /// value must not break deserialization of the whole transaction either. Accepts a JSON string,
    /// a number or null and never throws; a blank or unusable value (empty string, whitespace,
    /// boolean, object, array) reads as <see langword="null"/>. The BIN is kept as a string so that
    /// leading zeros are preserved.
    /// </summary>
    public class CardBinConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var text = reader.GetString();
                    return string.IsNullOrWhiteSpace(text) ? null : text.Trim();

                case JsonTokenType.Number:
                    return LenientStringConverter.ReadNumberText(ref reader);

                case JsonTokenType.Null:
                    return null;

                default:
                    // true/false, objects and arrays carry no usable BIN. Consume the value and move on.
                    reader.Skip();
                    return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value);
        }
    }
}
