using Newtonsoft.Json;
using Windows.UI;
using Worktile.Common;

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
        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                Background = AvatarHelper.GetColor(value);
                Initials = AvatarHelper.GetInitials(value);
            }
        }

        public Color Background { get; private set; }
        public string Initials { get; private set; }
    }
}
