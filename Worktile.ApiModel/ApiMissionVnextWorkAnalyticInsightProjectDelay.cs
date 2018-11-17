using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModel.ApiMissionVnextWorkAnalyticInsightProjectDelay
{


    public partial class ApiMissionVnextWorkAnalyticInsightProjectDelay
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
        [JsonProperty("pending")]
        public int Pending { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("item_count")]
        public int ItemCount { get; set; }

        [JsonProperty("delay_count")]
        public int DelayCount { get; set; }

        [JsonProperty("point")]
        public double Point { get; set; }

        [JsonProperty("follow")]
        public int Follow { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("visibility")]
        public int Visibility { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("progress")]
        public int Progress { get; set; }

        [JsonProperty("pending")]
        public int Pending { get; set; }

        [JsonProperty("delay_count")]
        public int DelayCount { get; set; }

        [JsonProperty("point")]
        public double Point { get; set; }

        [JsonProperty("follow")]
        public int Follow { get; set; }
    }
}
