using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModels.Message.ApiMessages
{

    public partial class ApiMessages
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("messages")]
        public List<Models.Message.Message> Messages { get; set; }

        [JsonProperty("more")]
        public bool More { get; set; }

        [JsonProperty("latest_id")]
        public string LatestId { get; set; }
    }
}
