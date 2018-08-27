using Newtonsoft.Json;

namespace Worktile.WindowsUI.Models.Start
{
    public class TeamLite
    {
        public string Name { get; set; }

        public string Domain { get; set; }

        public string Desc { get; set; }

        public string Logo { get; set; }

        [JsonProperty("outside_logo")]
        public string OutsideLogo { get; set; }

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
