using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.Models.IM.Message
{
    public class InlineAttachment
    {
        [JsonProperty("pretext")]
        public string Pretext { get; set; }

        [JsonProperty("fallback")]
        public string Fallback { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("title_link")]
        public string TitleLink { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("fields")]
        public List<Field> Fields { get; set; }
    }
}
