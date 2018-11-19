using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModel.ApiMissionVnextProjectsDetail
{
    public partial class ApiMissionVnextProjectsDetail
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
        [JsonProperty("references")]
        public References References { get; set; }

        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public partial class References
    {
        [JsonProperty("addons")]
        public List<Addon> Addons { get; set; }

        [JsonProperty("roles")]
        public List<Role> Roles { get; set; }
    }

    public partial class Addon
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("addon_id")]
        public string AddonId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("views")]
        public List<View> Views { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        [JsonProperty("static_permission")]
        public string StaticPermission { get; set; }
    }

    public partial class View
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("is_global")]
        public long IsGlobal { get; set; }

        [JsonProperty("is_show", NullValueHandling = NullValueHandling.Ignore)]
        public long? IsShow { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public TypeUnion? Type { get; set; }
    }

    public partial class Role
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("ref_id")]
        public object RefId { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("permission_value")]
        public string PermissionValue { get; set; }

        [JsonProperty("mode_id")]
        public string ModeId { get; set; }
    }

    public partial class Value
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("name_pinyin")]
        public string NamePinyin { get; set; }

        [JsonProperty("description")]
        public object Description { get; set; }

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

        [JsonProperty("group")]
        public object Group { get; set; }

        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        [JsonProperty("static_permission")]
        public string StaticPermission { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }
    }

    public partial class Archive
    {
        [JsonProperty("is_archived")]
        public long IsArchived { get; set; }
    }

    public partial class Member
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("role")]
        public long Role { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("mobile_area")]
        public string MobileArea { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }
    }

    public partial class Preferences
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public partial struct TypeUnion
    {
        public long? Integer;
        public string String;

        public static implicit operator TypeUnion(long Integer) => new TypeUnion { Integer = Integer };
        public static implicit operator TypeUnion(string String) => new TypeUnion { String = String };
    }
}