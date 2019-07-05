using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Worktile.Common
{
    /// <summary>
    /// 安全的UnixDateTimeConverter，当API返回的Unix中包含小数点时，使用此Converter
    /// </summary>
    public class SafeUnixDateTimeConverter : UnixDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Float)
            {
                string rawValue = reader.Value.ToString();
                int index = rawValue.IndexOf('.');
                string value = rawValue.Substring(0, index);
                long seconds = long.Parse(value);
                var date = DateTimeOffset.FromUnixTimeSeconds(seconds);
                if (objectType == typeof(DateTime))
                {
                    return date.LocalDateTime;
                }
                return date;
            }
            else
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
        }
    }
}
