using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModels.ApiMissionVnextWorkAddon
{
    public partial class ApiMissionVnextWorkAddon
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

        [JsonProperty("addon_id")]
        public string AddonId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
