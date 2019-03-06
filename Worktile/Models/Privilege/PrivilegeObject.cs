using Newtonsoft.Json;

namespace Worktile.Models.Privilege
{
    class PrivilegeObject
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("scope")]
        public PrivilegeScope Scope { get; set; }
    }
}
