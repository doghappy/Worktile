using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.Models.IM.Message
{
    public class MessageBody
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("markdown")]
        public int Markdown { get; set; }

        [JsonProperty("style")]
        public int Style { get; set; }

        [JsonProperty("attachment")]
        public BodyAttachment Attachment { get; set; }

        [JsonProperty("links")]
        public List<string> Links { get; set; }

        [JsonProperty("inline_attachment")]
        public InlineAttachment InlineAttachment { get; set; }
    }
}
