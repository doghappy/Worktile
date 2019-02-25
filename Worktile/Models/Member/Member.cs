using Newtonsoft.Json;

namespace Worktile.Models.Member
{
    public class Member
    {
        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("preferences")]
        public Preference Preferences { get; set; }
    }
}
