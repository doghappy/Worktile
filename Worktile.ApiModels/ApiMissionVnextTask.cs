using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Worktile.Enums;

namespace Worktile.ApiModel.ApiMissionVnextTask
{

    public partial class ApiMissionVnextTask
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
        [JsonProperty("value")]
        public DataValue Value { get; set; }

        [JsonProperty("references")]
        public References References { get; set; }
    }

    public partial class References
    {
        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        [JsonProperty("lookups")]
        public Lookups Lookups { get; set; }

        [JsonProperty("task_types")]
        public List<TaskType> TaskTypes { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("securities")]
        public List<object> Securities { get; set; }
    }

    public partial class Lookups
    {
        [JsonProperty("task_states")]
        public List<TaskState> TaskStates { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }

        [JsonProperty("priorities")]
        public List<PriorityElement> Priorities { get; set; }

        [JsonProperty("task_iteration_sprint")]
        public List<object> TaskIterationSprint { get; set; }

        [JsonProperty("task_data_sources")]
        public List<object> TaskDataSources { get; set; }

        [JsonProperty("tags")]
        public List<object> Tags { get; set; }

        [JsonProperty("attachments")]
        public List<object> Attachments { get; set; }
    }

    public partial class Member
    {
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }

        [JsonProperty("role")]
        public long Role { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("mobile_area")]
        public string MobileArea { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }
    }

    public partial class Preferences
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public partial class PriorityElement
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    public partial class TaskState
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }

    public partial class Project
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public object Icon { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("visibility")]
        public Visibility Visibility { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("task_identifier_prefix")]
        public string TaskIdentifierPrefix { get; set; }
    }

    public partial class Property
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("raw_key")]
        public string RawKey { get; set; }

        [JsonProperty("property_key")]
        public string PropertyKey { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("type")]
        public WtTaskPropertyType Type { get; set; }

        [JsonProperty("from")]
        public long From { get; set; }

        [JsonProperty("attribute")]
        public Attribute Attribute { get; set; }

        [JsonProperty("lookup", NullValueHandling = NullValueHandling.Ignore)]
        public string Lookup { get; set; }
    }

    public partial class Attribute
    {
        [JsonProperty("default_value")]
        public object DefaultValue { get; set; }

        [JsonProperty("placeholder")]
        public string Placeholder { get; set; }

        [JsonProperty("position")]
        public List<long> Position { get; set; }

        [JsonProperty("priority_mode_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PriorityModeId { get; set; }

        [JsonProperty("is_reference", NullValueHandling = NullValueHandling.Ignore)]
        public long? IsReference { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<Item> Items { get; set; }

        [JsonProperty("data_source_id")]
        public object DataSourceId { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }
    }

    public partial class TaskType
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("permission_prop_ids")]
        public List<object> PermissionPropIds { get; set; }
    }

    public partial class DataValue
    {
        [JsonProperty("properties")]
        public JObject Properties { get; set; }

        [JsonProperty("prop_permissions")]
        public string PropPermissions { get; set; }

        [JsonProperty("static_permissions")]
        public string StaticPermissions { get; set; }

        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("task_type_id")]
        public string TaskTypeId { get; set; }

        [JsonProperty("task_state_id")]
        public string TaskStateId { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("completed_at")]
        public object CompletedAt { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("is_archived")]
        public long IsArchived { get; set; }

        [JsonProperty("comments")]
        public List<object> Comments { get; set; }

        [JsonProperty("comment_count")]
        public long CommentCount { get; set; }

        [JsonProperty("likes")]
        public List<object> Likes { get; set; }

        [JsonProperty("like_count")]
        public long LikeCount { get; set; }

        [JsonProperty("security_ids")]
        public List<object> SecurityIds { get; set; }

        [JsonProperty("is_deleted")]
        public long IsDeleted { get; set; }

        [JsonProperty("relations")]
        public List<Relation> Relations { get; set; }
    }

    public partial class Relation
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("allow_add")]
        public long AllowAdd { get; set; }

        [JsonProperty("allow_select")]
        public long AllowSelect { get; set; }

        [JsonProperty("relationships")]
        public List<Relationship> Relationships { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("task_count")]
        public long TaskCount { get; set; }
    }

    public partial class Relationship
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inward")]
        public string Inward { get; set; }

        [JsonProperty("outward")]
        public string Outward { get; set; }
    }
}
