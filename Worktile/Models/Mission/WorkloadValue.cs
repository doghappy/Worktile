using Newtonsoft.Json;

namespace Worktile.Models.Mission
{
    public class WorkloadValue
    {
        [JsonProperty("reported_total")]
        public int Actual { get; set; }

        public WorkloadEstimate Estimated { get; set; }
    }
}
