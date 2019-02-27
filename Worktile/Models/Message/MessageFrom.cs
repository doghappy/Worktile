using Newtonsoft.Json;
using Windows.UI;
using Worktile.Enums.Message;
using Worktile.Views.Message;

namespace Worktile.Models.Message
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

        public Color Background { get; set; }
        public string Initials { get; set; }
    }
}
