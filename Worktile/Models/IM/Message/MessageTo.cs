using Newtonsoft.Json;

namespace Worktile.Models.IM.Message
{
    public class MessageTo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
