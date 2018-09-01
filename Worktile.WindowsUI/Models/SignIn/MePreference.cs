using Newtonsoft.Json;

namespace Worktile.WindowsUI.Models.SignIn
{
    public class MePreference
    {
        [JsonProperty("notify_desktop")]
        public int NotifyDesktop { get; set; }

        [JsonProperty("notify_mobile")]
        public int NotifyMobile { get; set; }

        [JsonProperty("notify_email")]
        public int NotifyEmail { get; set; }

        [JsonProperty("notify_desktop_sound")]
        public int NotifyDesktopSound { get; set; }

        [JsonProperty("emoji_style")]
        public int EmojiStyle { get; set; }

        [JsonProperty("emoji_size")]
        public int EmojiSize { get; set; }

        public string Locale { get; set; }

        public string Timezone { get; set; }

        public string Theme { get; set; }

        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        [JsonProperty("highlight_words")]
        public string[] HighlightWords { get; set; }

        [JsonProperty("is_new_user")]
        public bool IsNewUser { get; set; }

        [JsonProperty("latest_version")]
        public string LatestVersion { get; set; }
    }
}
