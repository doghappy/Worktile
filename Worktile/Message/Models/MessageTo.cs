﻿using Newtonsoft.Json;

namespace Worktile.Message.Models
{
    public class MessageTo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public ToType Type { get; set; }
    }

    public enum ToType
    {
        Channel = 1,
        Session
    }
}
