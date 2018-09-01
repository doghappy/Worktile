using Newtonsoft.Json;

namespace Worktile.WindowsUI.Models.Mission
{
    public class Archive
    {
        [JsonProperty("is_archived")]
        public bool IsArchived { get; set; }
    }
}
