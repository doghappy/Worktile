using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModels.ApiPinnedMessages
{
    public partial class ApiPinnedMessages
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("pinneds")]
        public List<Pinned> Pinneds { get; set; }

        [JsonProperty("batch")]
        public long Batch { get; set; }
    }

    public partial class Pinned
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("reference")]
        public Models.Message.Message Reference { get; set; }

        [JsonProperty("created_by")]
        public CreatedBy CreatedBy { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }
    }

    public partial class CreatedBy
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("role")]
        public long Role { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }
    }

    public partial class Preferences
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }
}
