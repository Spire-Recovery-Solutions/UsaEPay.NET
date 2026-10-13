using System.Buffers;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Converter
{
    /// <summary>
    /// Reads a string member without throwing when the API sends something other than a string.
    /// A JSON string is returned verbatim (including an empty string), a number is returned as its
    /// text, and null stays null. Any other token (boolean, object, array) is skipped and read as
    /// <see langword="null"/>. Use it on informational fields where a type surprise should degrade to
    /// a missing value instead of failing the whole response.
    /// </summary>
    public class LenientStringConverter : JsonConverter<string?>
    {
        public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return reader.GetString();

                case JsonTokenType.Number:
                    return ReadNumberText(ref reader);

                case JsonTokenType.Null:
                    return null;

                default:
                    reader.Skip();
                    return null;
            }
        }

        public override void Write(Utf8JsonWriter writer, string? value, JsonSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStringValue(value);
        }

        /// <summary>
        /// Returns the text of the JSON number the reader is positioned on. Integers are formatted
        /// invariantly; anything else (fractions, exponents) keeps its original JSON text.
        /// </summary>
        internal static string ReadNumberText(ref Utf8JsonReader reader)
        {
            if (reader.TryGetInt64(out var integer))
            {
                return integer.ToString(CultureInfo.InvariantCulture);
            }

            return Encoding.UTF8.GetString(reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan);
        }
    }
}
