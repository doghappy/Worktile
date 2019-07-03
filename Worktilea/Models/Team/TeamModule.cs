using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamModule
    {
        [JsonProperty("chat")]
        public TeamModulePermission<TeamModuleChatPermission> Chat { get; set; }

        [JsonProperty("mission")]
        public TeamModulePermission<TeamModuleMissionPermission> Mission { get; set; }

        [JsonProperty("calendar")]
        public TeamModulePermission<object> Calendar { get; set; }

        [JsonProperty("drive")]
        public TeamModulePermission<TeamModuleDrivePermission> Drive { get; set; }

        [JsonProperty("crm")]
        public TeamModulePermission<object> Crm { get; set; }

        [JsonProperty("approval")]
        public TeamModulePermission<object> Approval { get; set; }

        [JsonProperty("okr")]
        public TeamModuleOkrPermission Okr { get; set; }
    }
}
