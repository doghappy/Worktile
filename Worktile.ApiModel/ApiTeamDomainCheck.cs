using Newtonsoft.Json;

namespace Worktile.ApiModel.ApiTeamDomainCheck
{
    public partial class ApiTeamDomainCheck
    {
        [JsonProperty("data")]
        public bool Data { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }
}