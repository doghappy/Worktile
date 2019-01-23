using System.Collections.Generic;
using Newtonsoft.Json;
using Worktile.Models.IM.Message;

namespace Worktile.ApiModels.IM.ApiMessages
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
        public List<Message> Messages { get; set; }

        [JsonProperty("more")]
        public bool More { get; set; }

        [JsonProperty("latest_id")]
        public string LatestId { get; set; }
    }
}
