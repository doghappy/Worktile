﻿using Newtonsoft.Json;

namespace Worktile.Models.Message
{
    public class AttachmentReference
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
