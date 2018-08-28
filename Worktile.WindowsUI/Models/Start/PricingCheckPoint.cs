using Newtonsoft.Json;
using Worktile.WindowsUI.Extensions;

namespace Worktile.WindowsUI.Models.Start
{
    public class PricingCheckPoint
    {
        [JsonProperty("task")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableTask { get; set; }

        [JsonProperty("user_count")]
        public long UserCount { get; set; }

        [JsonProperty("project_count")]
        public long ProjectCount { get; set; }

        [JsonProperty("channel_count")]
        public long ChannelCount { get; set; }

        [JsonProperty("calender")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableCalender { get; set; }

        [JsonProperty("drive")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableDrive { get; set; }

        [JsonProperty("service_count")]
        public long ServiceCount { get; set; }

        [JsonProperty("task_analytics")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableTaskAnalytics { get; set; }

        [JsonProperty("task_analytics_customized")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableTaskAnalyticsCustomized { get; set; }

        [JsonProperty("report")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableReport { get; set; }

        [JsonProperty("notification_sms")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableNotificationSMS { get; set; }
        
        [JsonProperty("project_extension_fields")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableProjectExtensionFields { get; set; }

        [JsonProperty("calendar_organization_assistant")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableCalendarOrganizationAssistant { get; set; }

        [JsonProperty("security_log")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableSecurityLog { get; set; }

        [JsonProperty("approval")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableApproval { get; set; }

        [JsonProperty("leave")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableLeave { get; set; }

        [JsonProperty("task_workload_analytics")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableTaskWorkloadAnalytics { get; set; }

        [JsonProperty("crm")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableCRM { get; set; }

        [JsonProperty("ent_logo_customized")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableEntLogoCustomized { get; set; }

        [JsonProperty("security_policy")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableSecurityPolicy { get; set; }

        [JsonProperty("notification_content_customized")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableNotifyContentCustomized { get; set; }

        [JsonProperty("user_extension_fields")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableUserExtensionFields { get; set; }

        [JsonProperty("export")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableExport { get; set; }

        [JsonProperty("sso_saml")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableSsoSaml { get; set; }

        [JsonProperty("sso_adfs")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableSSoAdfs { get; set; }

        [JsonProperty("open_api")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableOpenApi { get; set; }

        [JsonProperty("team_statistic")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableTeamStatistic { get; set; }

        [JsonProperty("task_gantt_chart")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableTaskGanttChart { get; set; }

        [JsonProperty("dingtalk")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableDingTalk { get; set; }

        [JsonProperty("bulletin")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableBulletin { get; set; }

        [JsonProperty("appraisal")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableAppraisal { get; set; }

        [JsonProperty("okr")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableOKR { get; set; }

        [JsonProperty("portal")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnablePortal { get; set; }

        [JsonProperty("notification_email")]
        //[JsonConverter(typeof(BoolConverter))]
        public bool EnableNotificationEmail { get; set; }
    }
}
