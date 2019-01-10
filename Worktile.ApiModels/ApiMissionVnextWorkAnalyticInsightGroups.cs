using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModels.ApiMissionVnextWorkAnalyticInsightGroups
{

    public partial class ApiMissionVnextWorkAnalyticInsightGroups
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
        public List<Value> Value { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("insights")]
        public List<Insight> Insights { get; set; }
    }

    public partial class Insight
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("insight_id")]
        public string InsightId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("is_global")]
        public long IsGlobal { get; set; }
    }
}