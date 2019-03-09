using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamModuleOkrPermissionLimit
    {
        [JsonProperty("company_objective")]
        public int CompanyObjective { get; set; }

        [JsonProperty("department_objective")]
        public int DepartmentObjective { get; set; }

        [JsonProperty("direct_objective")]
        public int DirectObjective { get; set; }

        [JsonProperty("key_result")]
        public int KeyResult { get; set; }
    }
}
