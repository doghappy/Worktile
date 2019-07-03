using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamServiceReference
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
