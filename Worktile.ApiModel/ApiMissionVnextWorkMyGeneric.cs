using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Worktile.ApiModel.ApiMissionVnextWorkMyGeneric
{
    public partial class ApiMissionVnextWorkMyGeneric
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
        public List<ValueElement> Value { get; set; }

        [JsonProperty("references")]
        public References References { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("page_index")]
        public long PageIndex { get; set; }

        [JsonProperty("page_size")]
        public long PageSize { get; set; }

        [JsonProperty("page_count")]
        public long PageCount { get; set; }
    }

    public partial class References
    {
        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        [JsonProperty("lookups")]
        public Lookups Lookups { get; set; }

        [JsonProperty("task_types")]
        public List<TaskType> TaskTypes { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }

        [JsonProperty("projects")]
        public List<Project> Projects { get; set; }
    }

    public partial class Lookups
    {
        [JsonProperty("task_states")]
        public List<TaskState> TaskStates { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }
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

        [JsonProperty("mobile_area", NullValueHandling = NullValueHandling.Ignore)]
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
    }

    public partial class Preferences
    {
        [JsonProperty("locale", NullValueHandling = NullValueHandling.Ignore)]
        public string Locale { get; set; }
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
        public long Type { get; set; }
    }

    public partial class Project
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public object Icon { get; set; }

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
        public long Type { get; set; }

        [JsonProperty("from")]
        public long From { get; set; }

        [JsonProperty("lookup", NullValueHandling = NullValueHandling.Ignore)]
        public string Lookup { get; set; }
    }

    public partial class TaskType
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        [JsonProperty("permission_prop_ids")]
        public List<string> PermissionPropIds { get; set; }
    }

    public partial class ValueElement
    {
        [JsonProperty("properties")]
        public Properties Properties { get; set; }

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

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; set; }

        [JsonProperty("parent_ids")]
        public List<string> ParentIds { get; set; }

        [JsonProperty("parent", NullValueHandling = NullValueHandling.Ignore)]
        public Parent Parent { get; set; }
    }

    public partial class Parent
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("relationship")]
        public Relationship Relationship { get; set; }
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

    public partial class Properties
    {
        [JsonProperty("assignee")]
        public Assignee Assignee { get; set; }

        [JsonProperty("due")]
        public Due Due { get; set; }
    }

    public partial class Assignee
    {
        [JsonProperty("property_id")]
        public string PropertyId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }
    }

    public partial class Due
    {
        [JsonProperty("property_id")]
        public string PropertyId { get; set; }

        [JsonProperty("value")]
        public DueValue Value { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }
    }

    public partial class DueValue
    {
        [JsonProperty("date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? Date { get; set; }

        [JsonProperty("with_time")]
        public long WithTime { get; set; }
    }
}
