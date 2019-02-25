using Newtonsoft.Json;

namespace Worktile.Views.Message
{
    public class Feed
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("type")]
        public FeedType Type { get; set; }

        [JsonProperty("channel_id")]
        public string ChannelId { get; set; }
    }
}
