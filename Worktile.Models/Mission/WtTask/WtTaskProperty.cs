using Newtonsoft.Json;
using Worktile.Enums;

namespace Worktile.Models.Mission.WtTask
{
    public class WtTaskProperty
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty("raw_key")]
        public string RawKey { get; set; }

        [JsonProperty("property_key")]
        public string PropertyKey { get; set; }

        public string Key { get; set; }

        public WtTaskPropertyType Type { get; set; }

        public WtTaskPropertyFrom From { get; set; }

        public string Lookup { get; set; }
    }
}
