using Newtonsoft.Json;

namespace Worktile.ApiModels
{
    class ApiResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
