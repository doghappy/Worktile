using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamModulePermission<T>
    {
        [JsonProperty("permission")]
        public int? Permission { get; set; }

        [JsonProperty("permissions")]
        public T Permissions { get; set; }
    }
}
