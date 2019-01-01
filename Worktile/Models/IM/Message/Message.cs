﻿using Newtonsoft.Json;

namespace Worktile.Models.IM.Message
{
    public  class Message
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("from")]
        public MessageFrom From { get; set; }

        [JsonProperty("to")]
        public MessageTo To { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("body")]
        public MessageBody Body { get; set; }

        [JsonProperty("client")]
        public int Client { get; set; }

        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("component")]
        public int Component { get; set; }

        [JsonProperty("category_filter")]
        public int CategoryFilter { get; set; }

        [JsonProperty("category_trigger")]
        public int CategoryTrigger { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("is_star")]
        public int IsStar { get; set; }

        [JsonProperty("is_pinned")]
        public int IsPinned { get; set; }

        [JsonProperty("is_pending")]
        public int IsPending { get; set; }

        [JsonProperty("is_unread")]
        public int IsUnread { get; set; }
    }
}
