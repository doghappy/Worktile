using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using Worktile.Enums;
using Worktile.Enums.Message;

namespace Worktile.Models.Message.Session
{
    public class ChannelSession : ISession
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_pinyin")]
        public string NamePinyin { get; set; }

        public TethysAvatar TethysAvatar { get; set; }

        [JsonProperty("starred")]
        public bool Starred { get; set; }

        [JsonProperty("latest_message_at")]
        public DateTime LatestMessageAt { get; set; }

        [JsonProperty("show")]
        public int Show { get; set; }

        [JsonProperty("unread")]
        public int UnRead { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("team")]
        public string TeamId { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("created_by")]
        public Member.Member CreatedBy { get; set; }

        [JsonProperty("members")]
        public List<Member.Member> Members { get; set; }

        [JsonProperty("is_system")]
        public bool IsSystem { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("visibility")]
        public WtVisibility Visibility { get; set; }

        [JsonProperty("disabled")]
        public bool Disabled { get; set; }

        [JsonProperty("joined")]
        public bool Joined { get; set; }

        [JsonProperty("latest_message_id")]
        public string LatestMessageId { get; set; }

        public PageType PageType => PageType.Channel;
    }
}
