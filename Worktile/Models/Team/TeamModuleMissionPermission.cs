using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamModuleMissionPermission
    {
        [JsonProperty("is_workload_enabled")]
        public bool IsWorkloadEnabled { get; set; }

        [JsonProperty("is_gantt_enabled")]
        public bool IsGanttEnabled { get; set; }
    }
}
