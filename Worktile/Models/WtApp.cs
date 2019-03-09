using Newtonsoft.Json;
using Worktile.Models.Privilege;

namespace Worktile.Models
{
    public class WtApp
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("privileges")]
        public PrivilegeObject Privileges { get; set; }

        public string Icon { get; set; }
    }
}
