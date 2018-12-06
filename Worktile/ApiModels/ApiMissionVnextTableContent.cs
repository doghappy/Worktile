﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Worktile.Models.Mission.WtTask;

namespace Worktile.ApiModel.ApiMissionVnextTableContent
{
    public partial class ApiMissionVnextTableContent
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

        [JsonProperty("page_count")]
        public object PageCount { get; set; }
    }

    public partial class References
    {
        [JsonProperty("properties")]
        public List<WtTaskProperty> Properties { get; set; }

        [JsonProperty("lookups")]
        public Lookups Lookups { get; set; }

        [JsonProperty("task_types")]
        public List<TaskType> TaskTypes { get; set; }

        [JsonProperty("view")]
        public View View { get; set; }

        [JsonProperty("columns")]
        public List<string> Columns { get; set; }
    }

    public partial class Lookups
    {
        [JsonProperty("task_states")]
        public List<TaskState> TaskStates { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }

        [JsonProperty("priorities")]
        public List<PriorityElement> Priorities { get; set; }

        [JsonProperty("tags")]
        public List<TagElement> Tags { get; set; }

        [JsonProperty("task_iteration_sprint")]
        public List<IterationElement> TaskIterationSprint { get; set; }

        [JsonProperty("task_data_sources")]
        public List<TaskDataSource> TaskDataSources { get; set; }
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

    public partial class TagElement
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("mode_id")]
        public string ModeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }
    }

    public partial class IterationElement
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }
    }

    public partial class TaskDataSource
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
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

        [JsonProperty("level")]
        public long Level { get; set; }
    }

    public partial class ValueElement
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
        public string  CreatedBy { get; set; }

        [JsonProperty("updated_at")]
        public long UpdatedAt { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("parent_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ParentIds { get; set; }

        [JsonProperty("ref_task_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> RefTaskIds { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ParentId { get; set; }

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
}
