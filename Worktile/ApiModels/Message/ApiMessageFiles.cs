using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Worktile.ApiModels.Message.ApiMessageFiles
{
    class ApiMessageFiles
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

    class Data
    {
        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
    }

    class Entity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public CreatedBy CreatedBy { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("references")]
        public List<Reference> References { get; set; }

        [JsonProperty("shared")]
        public int Shared { get; set; }

        [JsonProperty("comment_count")]
        public int CommentCount { get; set; }

        [JsonProperty("client")]
        public int Client { get; set; }

        [JsonProperty("addition")]
        public Addition Addition { get; set; }
    }

    class Addition
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("thumb_height")]
        public int? ThumbHeight { get; set; }

        [JsonProperty("thumb_width")]
        public int? ThumbWidth { get; set; }

        [JsonProperty("public_url")]
        public string PublicUrl { get; set; }

        [JsonProperty("current_version")]
        public int CurrentVersion { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("path")]
        public Guid? Path { get; set; }

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

    public class CreatedBy
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

    public class Preferences
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public class Reference
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
