using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModel.ApiMissionVnextWorkAddonsKeyGroups
{
    public partial class ApiMissionVnextWorkAddonsKeyGroups
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

        [JsonProperty("is_global")]
        public long IsGlobal { get; set; }

        [JsonProperty("views")]
        public List<View> Views { get; set; }
    }

    public partial class View
    {
        [JsonProperty("view_id")]
        public string ViewId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
