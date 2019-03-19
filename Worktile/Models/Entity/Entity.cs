using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Worktile.Models.Entity
{
    public class Entity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public Member.Member CreatedBy { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("references")]
        public List<EntityReference> References { get; set; }

        [JsonProperty("shared")]
        public int Shared { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("client")]
        public int Client { get; set; }

        [JsonProperty("addition")]
        public EntityAddition Addition { get; set; }

        [JsonProperty("comments")]
        public List<object> Comments { get; set; }

        public TethysAvatar Avatar { get; set; }
        public string Icon { get; set; }
        public bool IsEnableDelete { get; set; }
        public bool IsEnableDownload { get; set; }
    }
}
