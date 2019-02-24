using Newtonsoft.Json;

namespace Worktile.Views.Message
{
    public class Feed
    {
        [JsonProperty("id")]
        public object Id { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("type")]
        public FeedType Type { get; set; }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }
    }
}
