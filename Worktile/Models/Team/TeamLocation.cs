using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamLocation
    {
        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
