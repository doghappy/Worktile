using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamSecurity
    {
        [JsonProperty("min_length")]
        public int MinLength { get; set; }

        [JsonProperty("level")]
        public int Level { get; set; }
    }
}
