using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamPricingSetting
    {
        [JsonProperty("has_trail_experience")]
        public bool HasTrailExperience { get; set; }

        [JsonProperty("has_trail_notice")]
        public bool HasTrailNotice { get; set; }
    }
}
