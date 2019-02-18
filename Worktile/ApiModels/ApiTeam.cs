﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Worktile.ApiModels.ApiTeam
{

    public partial class ApiTeam
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("expired_at")]
        public long ExpiredAt { get; set; }

        [JsonProperty("user_limit")]
        public long UserLimit { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("is_paid")]
        public long IsPaid { get; set; }

        [JsonProperty("is_trial")]
        public long IsTrial { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("outside_logo")]
        public string OutsideLogo { get; set; }

        [JsonProperty("preferences")]
        public DataPreferences Preferences { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("industry")]
        public long Industry { get; set; }

        [JsonProperty("scale")]
        public long Scale { get; set; }

        [JsonProperty("from")]
        public long From { get; set; }

        [JsonProperty("security")]
        public Security Security { get; set; }

        [JsonProperty("pricing_checkpoints")]
        public Dictionary<string, long> PricingCheckpoints { get; set; }

        [JsonProperty("pricing_setting")]
        public PricingSetting PricingSetting { get; set; }

        [JsonProperty("is_mission_vnext")]
        public long IsMissionVnext { get; set; }

        [JsonProperty("pricing_products")]
        public List<long> PricingProducts { get; set; }

        [JsonProperty("pricing_features")]
        public List<object> PricingFeatures { get; set; }

        [JsonProperty("is_new")]
        public long IsNew { get; set; }

        [JsonProperty("large_mode_enabled")]
        public bool LargeModeEnabled { get; set; }

        [JsonProperty("members")]
        public List<Member> Members { get; set; }

        [JsonProperty("member_count")]
        public long MemberCount { get; set; }

        [JsonProperty("services")]
        public List<Service> Services { get; set; }

        [JsonProperty("apps")]
        public List<App> Apps { get; set; }

        [JsonProperty("privileges")]
        public Privileges Privileges { get; set; }

        [JsonProperty("team_permission")]
        public long TeamPermission { get; set; }

        [JsonProperty("function_limitation")]
        public long FunctionLimitation { get; set; }

        [JsonProperty("checkpoints")]
        public string Checkpoints { get; set; }

        [JsonProperty("modules")]
        public Modules Modules { get; set; }
    }

    public partial class App
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("integration_id")]
        public string IntegrationId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("privileges")]
        public Bulletin Privileges { get; set; }
    }

    public partial class Bulletin
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("scope")]
        public BulletinScope Scope { get; set; }
    }

    public partial class BulletinScope
    {
        [JsonProperty("view", NullValueHandling = NullValueHandling.Ignore)]
        public long? View { get; set; }

        [JsonProperty("manage", NullValueHandling = NullValueHandling.Ignore)]
        public List<long> Manage { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public partial class Member
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
        
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("preferences")]
        public MemberPreferences Preferences { get; set; }

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

        [JsonProperty("short_code", NullValueHandling = NullValueHandling.Ignore)]
        public string ShortCode { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }
    }

    public partial class MemberPreferences
    {
        public string Locale { get; set; }
    }

    public partial class Modules
    {
        [JsonProperty("chat")]
        public Chat Chat { get; set; }

        [JsonProperty("mission")]
        public Mission Mission { get; set; }

        [JsonProperty("calendar")]
        public CalendarClass Calendar { get; set; }

        [JsonProperty("drive")]
        public Drive Drive { get; set; }

        [JsonProperty("crm")]
        public CalendarClass Crm { get; set; }

        [JsonProperty("approval")]
        public CalendarClass Approval { get; set; }

        [JsonProperty("okr")]
        public Okr Okr { get; set; }
    }

    public partial class CalendarClass
    {
        [JsonProperty("permission")]
        public long Permission { get; set; }
    }

    public partial class Chat
    {
        [JsonProperty("permission")]
        public long Permission { get; set; }

        [JsonProperty("permissions")]
        public ChatPermissions Permissions { get; set; }
    }

    public partial class ChatPermissions
    {
        [JsonProperty("message_post_general")]
        public long MessagePostGeneral { get; set; }

        [JsonProperty("message_editing")]
        public long MessageEditing { get; set; }

        [JsonProperty("message_deleting")]
        public long MessageDeleting { get; set; }
    }

    public partial class Drive
    {
        [JsonProperty("permission")]
        public long Permission { get; set; }

        [JsonProperty("permissions")]
        public DrivePermissions Permissions { get; set; }
    }

    public partial class DrivePermissions
    {
        [JsonProperty("team_root_permission")]
        public long TeamRootPermission { get; set; }

        [JsonProperty("personal_root_permission")]
        public long PersonalRootPermission { get; set; }
    }

    public partial class Mission
    {
        [JsonProperty("permissions")]
        public MissionPermissions Permissions { get; set; }
    }

    public partial class MissionPermissions
    {
        [JsonProperty("is_workload_enabled")]
        public bool IsWorkloadEnabled { get; set; }

        [JsonProperty("is_gantt_enabled")]
        public bool IsGanttEnabled { get; set; }
    }

    public partial class Okr
    {
        [JsonProperty("limits")]
        public Limits Limits { get; set; }

        [JsonProperty("operation_objective_differential")]
        public double OperationObjectiveDifferential { get; set; }
    }

    public partial class Limits
    {
        [JsonProperty("company_objective")]
        public long CompanyObjective { get; set; }

        [JsonProperty("department_objective")]
        public long DepartmentObjective { get; set; }

        [JsonProperty("direct_objective")]
        public long DirectObjective { get; set; }

        [JsonProperty("key_result")]
        public long KeyResult { get; set; }
    }

    public partial class DataPreferences
    {
        [JsonProperty("project_nav_mode")]
        public long ProjectNavMode { get; set; }

        [JsonProperty("modules_order_mode")]
        public long ModulesOrderMode { get; set; }

        [JsonProperty("external_links")]
        public long ExternalLinks { get; set; }

        [JsonProperty("display_policy")]
        public long DisplayPolicy { get; set; }

        [JsonProperty("username_policy")]
        public string UsernamePolicy { get; set; }

        [JsonProperty("default_channels")]
        public List<string> DefaultChannels { get; set; }

        [JsonProperty("email_domain")]
        public string EmailDomain { get; set; }

        [JsonProperty("discovery_mode")]
        public long DiscoveryMode { get; set; }

        [JsonProperty("allow_self_edit_job")]
        public long AllowSelfEditJob { get; set; }
    }

    public partial class PricingSetting
    {
        [JsonProperty("has_trail_experience")]
        public long HasTrailExperience { get; set; }

        [JsonProperty("has_trail_notice")]
        public long HasTrailNotice { get; set; }
    }

    public partial class Privileges
    {
        [JsonProperty("admin")]
        public Admin Admin { get; set; }

        [JsonProperty("message")]
        public Admin Message { get; set; }

        [JsonProperty("mission")]
        public Admin Mission { get; set; }

        [JsonProperty("calendar")]
        public Admin Calendar { get; set; }

        [JsonProperty("drive")]
        public Admin Drive { get; set; }

        [JsonProperty("report")]
        public LeaveClass Report { get; set; }

        [JsonProperty("crm")]
        public Bulletin Crm { get; set; }

        [JsonProperty("approval")]
        public LeaveClass Approval { get; set; }

        [JsonProperty("leave")]
        public LeaveClass Leave { get; set; }

        [JsonProperty("appraisal")]
        public Admin Appraisal { get; set; }

        [JsonProperty("bulletin")]
        public Bulletin Bulletin { get; set; }

        [JsonProperty("okr")]
        public Bulletin Okr { get; set; }

        [JsonProperty("portal")]
        public Admin Portal { get; set; }

        [JsonProperty("mission_vnext")]
        public Admin MissionVnext { get; set; }
    }

    public partial class Admin
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("scope")]
        public AdminScope Scope { get; set; }
    }

    public partial class AdminScope
    {
    }

    public partial class LeaveClass
    {
        [JsonProperty("value")]
        public long Value { get; set; }

        [JsonProperty("scope")]
        public ApprovalScope Scope { get; set; }
    }

    public partial class ApprovalScope
    {
        [JsonProperty("view")]
        public long View { get; set; }
    }

    public partial class Security
    {
        [JsonProperty("min_length")]
        public long MinLength { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }
    }

    public partial class Service
    {
        [JsonProperty("service_id")]
        public string ServiceId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("integration")]
        public string Integration { get; set; }

        [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("avatar", NullValueHandling = NullValueHandling.Ignore)]
        public string Avatar { get; set; }

        [JsonProperty("reference", NullValueHandling = NullValueHandling.Ignore)]
        public Reference Reference { get; set; }

        [JsonProperty("addition_id", NullValueHandling = NullValueHandling.Ignore)]
        public string AdditionId { get; set; }
    }

    public partial class Reference
    {
        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}