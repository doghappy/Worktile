using Newtonsoft.Json;

namespace Worktile.Message.Models
{
    public class MessageField
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("short")]
        public int Short { get; set; }
    }
}
