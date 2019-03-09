using Newtonsoft.Json;

namespace Worktile.Models.Team
{
    public class TeamModuleOkrPermission
    {
        [JsonProperty("limits")]
        public TeamModuleOkrPermissionLimit Limits { get; set; }

        [JsonProperty("operation_objective_differential")]
        public double OperationObjectiveDifferential { get; set; }
    }
}
