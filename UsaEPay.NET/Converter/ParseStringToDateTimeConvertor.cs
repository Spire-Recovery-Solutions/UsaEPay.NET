using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsaEPay.NET.Converter
{
    public class ParseStringToDateTimeConvertor : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(DateTime) || t == typeof(DateTime?);
        private const string _dateFormat = "yyyy-MM-dd HH:mm:ss";


        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is DateTime dateTime)
            {
                writer.WriteValue(dateTime.ToString(_dateFormat));
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null && reader.Value is string stringValue)
            {
                if (DateTime.TryParseExact(stringValue, _dateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            return null; 
        }

        public static readonly ParseStringToDateTimeConvertor Singleton = new ParseStringToDateTimeConvertor();
    }
}
