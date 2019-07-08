using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Models
{
    public class Entity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public User CreatedBy { get; set; }

        [JsonProperty("team")]
        public string TeamId { get; set; }

        public Client Client { get; set; }

        [JsonProperty("shared")]
        public bool IsShared { get; set; }

        public WtMessage.MessageType Type { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        public WtMessage.AttachmentAddition Addition { get; set; }
    }
}
