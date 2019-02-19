using Newtonsoft.Json;

namespace Worktile.ApiModels
{
    class ApiDataResponse<T> : ApiResponse
    {
        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
