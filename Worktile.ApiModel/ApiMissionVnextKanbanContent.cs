using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Worktile.ApiModel.ApiMissionVnextKanbanContent
{

    public partial class ApiMissionVnextKanbanContent
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
    }

    public partial class References
    {
        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        [JsonProperty("lookups")]
        public Lookups Lookups { get; set; }

        [JsonProperty("task_types")]
        public List<TaskType> TaskTypes { get; set; }

        [JsonProperty("view")]
        public View View { get; set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; set; }
    }

    public partial class Group
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("project_addon_id")]
        public string ProjectAddonId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("position")]
        public long Position { get; set; }

        [JsonProperty("is_archived")]
        public long IsArchived { get; set; }

        [JsonProperty("task_ids")]
        public List<string> TaskIds { get; set; }

        [JsonProperty("is_watched")]
        public long IsWatched { get; set; }
    }

    public partial class Lookups
    {
        [JsonProperty("task_states")]
        public List<PriorityElement> TaskStates { get; set; }

        [JsonProperty("priorities")]
        public List<PriorityElement> Priorities { get; set; }
    }

    public partial class PriorityElement
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long? Type { get; set; }
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

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("permission_prop_ids")]
        public List<string> PermissionPropIds { get; set; }

        [JsonProperty("show_settings")]
        public List<ShowSetting> ShowSettings { get; set; }
    }

    public partial class ShowSetting
    {
        [JsonProperty("task_property_id")]
        public string TaskPropertyId { get; set; }
    }

    public partial class View
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("group_by")]
        public string GroupBy { get; set; }

        [JsonProperty("group_type")]
        public long GroupType { get; set; }

        [JsonProperty("sort_by")]
        public object SortBy { get; set; }

        [JsonProperty("sort_type")]
        public long SortType { get; set; }

        [JsonProperty("conditions")]
        public List<object> Conditions { get; set; }
    }

    public partial class ValueElement
    {
        [JsonProperty("properties")]
        public Properties Properties { get; set; }

        [JsonProperty("prop_permissions")]
        public string PropPermissions { get; set; }

        [JsonProperty("static_permissions")]
        public long StaticPermissions { get; set; }

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

        [JsonProperty("parent_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> ParentIds { get; set; }
    }

    public partial class Properties
    {
        [JsonProperty("start")]
        public Start Start { get; set; }

        [JsonProperty("attachment")]
        public Attachment Attachment { get; set; }

        [JsonProperty("priority")]
        public PropertiesPriority Priority { get; set; }
    }

    public partial class Attachment
    {
        [JsonProperty("property_id")]
        public string PropertyId { get; set; }

        [JsonProperty("value")]
        public List<object> Value { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }
    }

    public partial class PropertiesPriority
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

    public partial class Start
    {
        [JsonProperty("property_id")]
        public string PropertyId { get; set; }

        [JsonProperty("value")]
        public StartValue Value { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }
    }

    public partial class StartValue
    {
        [JsonProperty("date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? Date { get; set; }

        [JsonProperty("with_time")]
        public long WithTime { get; set; }
    }
}
