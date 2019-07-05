using Newtonsoft.Json;

namespace Worktile.Message.Models
{
    public class MessageFrom
    {
        [JsonProperty("type")]
        public FromType Type { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
