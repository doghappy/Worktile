using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Worktile.ApiModels.Upload
{
    public partial class ApiEntitiesUpload
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
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

    public partial class Addition
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
    }

    public partial class CreatedBy
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("role")]
        public int Role { get; set; }
    }

    public partial class Reference
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
