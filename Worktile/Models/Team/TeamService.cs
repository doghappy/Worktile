using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamService : IMemberBase
    {
        [JsonProperty("service_id")]
        public string ServiceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("integration")]
        public string Integration { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("reference")]
        public TeamServiceReference Reference { get; set; }

        [JsonProperty("addition_id")]
        public string AdditionId { get; set; }
    }
}
