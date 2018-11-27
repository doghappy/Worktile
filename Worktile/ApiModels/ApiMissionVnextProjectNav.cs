using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModels.ApiMissionVnextProjectNav
{

    public partial class ApiMissionVnextProjectNav
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("project_nav")]
        public ProjectNav ProjectNav { get; set; }

        [JsonProperty("projects")]
        public List<Project> Projects { get; set; }

        [JsonProperty("project_nav_mode")]
        public long ProjectNavMode { get; set; }
    }

    public partial class ProjectNav
    {
        [JsonProperty("favorites")]
        public List<string> Favorites { get; set; }

        [JsonProperty("items")]
        public List<ProjectNavItem> Items { get; set; }
    }

    public partial class ProjectNavItem
    {
        [JsonProperty("nav_type")]
        public long NavType { get; set; }

        [JsonProperty("nav_id")]
        public string NavId { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<ItemItem> Items { get; set; }
    }

    public partial class ItemItem
    {
        [JsonProperty("nav_type")]
        public long NavType { get; set; }

        [JsonProperty("nav_id")]
        public string NavId { get; set; }
    }

    public partial class Project
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_pinyin")]
        public string NamePinyin { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public object Icon { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("visibility")]
        public long Visibility { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("archive")]
        public Archive Archive { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("task_identifier_prefix")]
        public string TaskIdentifierPrefix { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }

        [JsonProperty("is_favorite")]
        public long IsFavorite { get; set; }

        [JsonProperty("static_permission")]
        public string StaticPermission { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }
    }

    public partial class Archive
    {
        [JsonProperty("is_archived")]
        public long IsArchived { get; set; }
    }

    public partial class Member
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }
}
