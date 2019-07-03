using Newtonsoft.Json;

namespace Worktile.Models.Department
{
    public class DepartmentNodeAddition
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name_pinyin")]
        public string NamePinyin { get; set; }

        [JsonProperty("member_count")]
        public int? MemberCount { get; set; }
    }
}
