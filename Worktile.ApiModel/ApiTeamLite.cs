using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModel.ApiTeamLite
{
    public partial class ApiTeamLite
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("is_paid")]
        public long IsPaid { get; set; }

        [JsonProperty("is_trial")]
        public long IsTrial { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("outside_logo")]
        public string OutsideLogo { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("security")]
        public Security Security { get; set; }

        [JsonProperty("pricing_products")]
        public List<long> PricingProducts { get; set; }

        [JsonProperty("pricing_features")]
        public List<object> PricingFeatures { get; set; }

        [JsonProperty("pricing_checkpoints")]
        public Dictionary<string, long> PricingCheckpoints { get; set; }

        [JsonProperty("email_domain")]
        public string DataEmailDomain { get; set; }

        [JsonProperty("emailDomain")]
        public string EmailDomain { get; set; }

        [JsonProperty("mode")]
        public long Mode { get; set; }

        [JsonProperty("is_expired")]
        public long IsExpired { get; set; }

        [JsonProperty("function_limitation")]
        public long FunctionLimitation { get; set; }

        [JsonProperty("checkpoints")]
        public string Checkpoints { get; set; }

        [JsonProperty("sso")]
        public List<object> Sso { get; set; }
    }

    public partial class Security
    {
        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("min_length")]
        public long MinLength { get; set; }
    }
}
