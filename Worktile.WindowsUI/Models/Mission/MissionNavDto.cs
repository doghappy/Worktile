using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.WindowsUI.Models.Mission
{
    public class MissionNavDto
    {
        [JsonProperty("project_nav")]
        public MissionNav MissionNav { get; set; }

        [JsonProperty("projects")]
        public List<MissionItem> MissionItems { get; set; }
    }
}
