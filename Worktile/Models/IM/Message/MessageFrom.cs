using Newtonsoft.Json;

namespace Worktile.Models.IM.Message
{
    public class MessageFrom
    {
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
    }
}
