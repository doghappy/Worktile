using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.Models.IM.Message
{
    public class BodyAttachment
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("references")]
        public List<AttachmentReference> References { get; set; }

        [JsonProperty("shared")]
        public int Shared { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("client")]
        public int Client { get; set; }

        [JsonProperty("addition")]
        public AttachmentAddition Addition { get; set; }
    }
}
