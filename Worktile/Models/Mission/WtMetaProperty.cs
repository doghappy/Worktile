using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Worktile.Models.Mission
{
    class WtMetaProperty<T>
    {
        [JsonProperty("property_id")]
        public string Id { get; set; }

        [JsonProperty("value")]
        public T Value { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime UpdatedAt { get; set; }
    }
}
