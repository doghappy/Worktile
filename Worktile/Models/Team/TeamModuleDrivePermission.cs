using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamModuleDrivePermission
    {
        [JsonProperty("team_root_permission")]
        public int TeamRootPermission { get; set; }

        [JsonProperty("personal_root_permission")]
        public int PersonalRootPermission { get; set; }
    }
}
