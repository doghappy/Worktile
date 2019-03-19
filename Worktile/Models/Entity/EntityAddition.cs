using Newtonsoft.Json;
using System;

namespace Worktile.Models.Entity
{
    public class EntityAddition
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("thumb_height")]
        public int ThumbHeight { get; set; }

        [JsonProperty("thumb_width")]
        public int ThumbWidth { get; set; }

        [JsonProperty("public_url")]
        public string PublicUrl { get; set; }

        [JsonProperty("current_version")]
        public int CurrentVersion { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("ext")]
        public string Ext { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("is_draft")]
        public int? IsDraft { get; set; }
    }
}
