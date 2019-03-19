using Newtonsoft.Json;

namespace Worktile.Models.Entity
{
    public class EntityReference
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
