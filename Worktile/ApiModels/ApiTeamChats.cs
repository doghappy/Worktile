using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Worktile.Common;
using Worktile.Enums;

namespace Worktile.ApiModels.ApiTeamChats
{
    public partial class ApiTeamChats
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("channels")]
        public List<Channel> Channels { get; set; }

        [JsonProperty("groups")]
        public List<Channel> Groups { get; set; }

        [JsonProperty("sessions")]
        public List<Session> Sessions { get; set; }
    }

    public partial class Channel
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_pinyin")]
        public string NamePinyin { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public ChannelCreatedBy CreatedBy { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }

        [JsonProperty("is_system")]
        public int IsSystem { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("visibility")]
        public Visibility Visibility { get; set; }

        [JsonProperty("disabled")]
        public int Disabled { get; set; }

        [JsonProperty("joined")]
        public int Joined { get; set; }

        [JsonProperty("starred")]
        public bool Starred { get; set; }

        [JsonProperty("latest_message_id")]
        public string LatestMessageId { get; set; }

        [JsonProperty("latest_message_at")]
        [JsonConverter(typeof(SafeUnixDateTimeConverter))]
        public DateTime LatestMessageAt { get; set; }

        [JsonProperty("unread")]
        public int UnRead { get; set; }

        [JsonProperty("show")]
        public int Show { get; set; }
    }

    public partial class ChannelCreatedBy
    {
        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }
    }

    public partial class Preferences
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public partial class Member
    {
        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }

        [JsonProperty("preference")]
        public Preference Preference { get; set; }
    }

    public partial class Preference
    {
        [JsonProperty("notify_desktop")]
        public int NotifyDesktop { get; set; }

        [JsonProperty("notify_mobile")]
        public int NotifyMobile { get; set; }
    }

    public partial class Session
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        public int CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("is_bot")]
        public bool IsBot { get; set; }

        [JsonProperty("latest_message_id")]
        public string LatestMessageId { get; set; }

        [JsonProperty("starred")]
        public bool Starred { get; set; }

        [JsonProperty("to")]
        public To To { get; set; }

        [JsonProperty("latest_message_at")]
        [JsonConverter(typeof(SafeUnixDateTimeConverter))]
        public DateTime LatestMessageAt { get; set; }

        [JsonProperty("unread")]
        public int UnRead { get; set; }

        [JsonProperty("show")]
        public int Show { get; set; }

        [JsonProperty("component")]
        public int? Component { get; set; }

        [JsonProperty("menus")]
        public List<object> Menus { get; set; }

        [JsonProperty("service_id")]
        public string ServiceId { get; set; }
    }

    public partial class To
    {
        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }
    }
}
