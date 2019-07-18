using Newtonsoft.Json;

namespace Worktile.WebUI.Models.Message
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

    public enum FromType
    {
        User = 1,
        Service,
        Addition
    }
}
