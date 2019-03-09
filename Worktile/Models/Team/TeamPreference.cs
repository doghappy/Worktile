using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.Models.Team
{
    public class TeamPreference
    {
        [JsonProperty("project_nav_mode")]
        public int ProjectNavMode { get; set; }

        [JsonProperty("modules_order_mode")]
        public int ModulesOrderMode { get; set; }

        [JsonProperty("external_links")]
        public int ExternalLinks { get; set; }

        [JsonProperty("display_policy")]
        public int DisplayPolicy { get; set; }

        [JsonProperty("username_policy")]
        public string UsernamePolicy { get; set; }

        [JsonProperty("default_channels")]
        public List<string> DefaultChannels { get; set; }

        [JsonProperty("email_domain")]
        public string EmailDomain { get; set; }

        [JsonProperty("discovery_mode")]
        public int DiscoveryMode { get; set; }

        [JsonProperty("allow_self_edit_job")]
        public int AllowSelfEditJob { get; set; }
    }
}
