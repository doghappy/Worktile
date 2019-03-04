using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using Worktile.Common;
using Worktile.Enums.Message;

namespace Worktile.Models.Message.Session
{
    public class MemberSession : ISession
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        public string NamePinyin { get; set; }

        public TethysAvatar TethysAvatar { get; set; }

        [JsonProperty("starred")]
        public bool Starred { get; set; }

        [JsonProperty("latest_message_at")]
        [JsonConverter(typeof(SafeUnixDateTimeConverter))]
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

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("to")]
        public Member.Member To { get; set; }

        [JsonProperty("component")]
        public int? Component { get; set; }

        public PageType PageType => IsBot ? PageType.Assistant : PageType.Member;
    }
}
