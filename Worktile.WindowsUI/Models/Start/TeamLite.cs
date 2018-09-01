using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Worktile.WindowsUI.Models.Start
{
    public class TeamLite
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("is_trial")]
        public bool IsTrial { get; set; }

        [JsonProperty("expired_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ExpiredAt { get; set; }

        [JsonProperty("user_limit")]
        public int UserLimit { get; set; }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string Desc { get; set; }

        public string Logo { get; set; }

        [JsonProperty("outside_logo")]
        public string OutsideLogo { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreateAt { get; set; }

        [JsonProperty("created_by")]
        public string CreateBy { get; set; }

        public string Locale { get; set; }

        public string Timezone { get; set; }

        [JsonProperty("pricing_version")]
        public int PricingVersion { get; set; }

        public string EmailDomain { get; set; }

        [JsonProperty("is_expired")]
        public int IsExpired { get; set; }

        [JsonProperty("pricing_checkpoints")]
        public PricingCheckPoint PricingCheckPoint { get; set; }
    }
}
