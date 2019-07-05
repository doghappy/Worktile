using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Worktile.Models
{
    public class Team
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string Desc { get; set; }

        public string Logo { get; set; }

        [JsonProperty("outside_logo")]
        public string OutsideLogo { get; set; }

        public string Locale { get; set; }

        public string Timezone { get; set; }

        [JsonProperty("pricing_version")]
        public PricingVersion PricingVersion { get; set; }

        [JsonProperty("expired_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset ExpiredAt { get; set; }

        [JsonProperty("user_limit")]
        public int UserLimit { get; set; }
    }

    public class PricingCheckpoints
    {

    }

    public enum PricingVersion
    {
        Trail = -1,
        Free = 1,
        Professional = 2,
        Ultimate = 4
    }
}
