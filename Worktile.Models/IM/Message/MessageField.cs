using Newtonsoft.Json;

namespace Worktile.Models.IM.Message
{
    public class Field
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
