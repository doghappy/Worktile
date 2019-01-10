using System.Collections.Generic;
using Newtonsoft.Json;
using Worktile.Models.IM.Message;

namespace Worktile.ApiModels.IM.ApiPigeonMessages
{

    public partial class ApiPigeonMessages
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

        [JsonProperty("unread_length")]
        public int UnreadLength { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }
    }
}
