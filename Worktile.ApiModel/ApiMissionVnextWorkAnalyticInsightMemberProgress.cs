using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModel.ApiMissionVnextWorkAnalyticInsightMemberProgress
{
    public partial class ApiMissionVnextWorkAnalyticInsightMemberProgress
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("completed")]
        public int Completed { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("pending")]
        public int Pending { get; set; }

        [JsonProperty("point")]
        public double Point { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("pending")]
        public int Pending { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("completed")]
        public int Completed { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("point")]
        public double Point { get; set; }
    }
}