using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using Worktile.Enums;

namespace Worktile.Models.Team
{
    public class Team
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("expired_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime ExpiredAt { get; set; }

        [JsonProperty("user_limit")]
        public int UserLimit { get; set; }

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

        [JsonProperty("preferences")]
        public TeamPreference Preferences { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("location")]
        public TeamLocation Location { get; set; }

        [JsonProperty("industry")]
        public int Industry { get; set; }

        [JsonProperty("scale")]
        public int Scale { get; set; }

        [JsonProperty("from")]
        public int From { get; set; }

        [JsonProperty("security")]
        public TeamSecurity Security { get; set; }

        [JsonProperty("pricing_version")]
        public WorktileVersion PricingVersion { get; set; }

        [JsonProperty("pricing_checkpoints")]
        public Dictionary<string, long> PricingCheckpoints { get; set; }

        [JsonProperty("pricing_setting")]
        public TeamPricingSetting PricingSetting { get; set; }

        [JsonProperty("is_mission_vnext")]
        public int IsMissionVnext { get; set; }

        [JsonProperty("pricing_products")]
        public List<long> PricingProducts { get; set; }

        [JsonProperty("pricing_features")]
        public List<object> PricingFeatures { get; set; }

        [JsonProperty("is_new")]
        public int IsNew { get; set; }

        [JsonProperty("large_mode_enabled")]
        public bool LargeModeEnabled { get; set; }

        [JsonProperty("members")]
        public List<Member.Member> Members { get; set; }

        [JsonProperty("member_count")]
        public int MemberCount { get; set; }

        [JsonProperty("services")]
        public List<TeamService> Services { get; set; }

        [JsonProperty("apps")]
        public List<WtApp> Apps { get; set; }

        [JsonProperty("privileges")]
        public Privilege.Privilege Privileges { get; set; }

        [JsonProperty("team_permission")]
        public int TeamPermission { get; set; }

        [JsonProperty("function_limitation")]
        public int FunctionLimitation { get; set; }

        [JsonProperty("checkpoints")]
        public string Checkpoints { get; set; }

        [JsonProperty("modules")]
        public TeamModule Modules { get; set; }
    }
}
