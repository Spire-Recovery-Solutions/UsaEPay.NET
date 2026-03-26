using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaEPay.NET.Converter
{
    public class UsaEPayStringToDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
    {
        public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            var dateTimeString = reader.GetString();
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return null;
            }

            // Parse the date and time with the appropriate format and a default timezone if necessary
            return DateTimeOffset.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue();
            }
            else
            {
                // Write back in the desired format
                writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}
