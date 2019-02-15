using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Worktile.Models
{
    public class WtDate
    {
        [JsonProperty("date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? Date { get; set; }

        [JsonProperty("with_time")]
        public bool WithTime { get; set; }
    }
}
