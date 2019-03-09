using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamModuleChatPermission
    {
        [JsonProperty("message_post_general")]
        public int MessagePostGeneral { get; set; }

        [JsonProperty("message_editing")]
        public int MessageEditing { get; set; }

        [JsonProperty("message_deleting")]
        public int MessageDeleting { get; set; }
    }
}
