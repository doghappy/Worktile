using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Worktile.Models.Mission
{
    public class WorkloadEstimate
    {
        public int Duration { get; set; }

        [JsonProperty("estimated_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? EstimatedAt { get; set; }

        [JsonProperty("estimated_by")]
        public string EstimatedBy { get; set; }
    }
}
