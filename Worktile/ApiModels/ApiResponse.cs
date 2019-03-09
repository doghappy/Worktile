using Newtonsoft.Json;

namespace Worktile.ApiModels
{
    class ApiResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }
    }
}
