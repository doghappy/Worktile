using Newtonsoft.Json;

namespace Worktile.WindowsUI.Models.Start
{
    public class PricingCheckPoint
    {
        [JsonProperty("task")]
        public bool EnableTask { get; set; }

        [JsonProperty("user_count")]
        public int UserCount { get; set; }

        [JsonProperty("project_count")]
        public int ProjectCount { get; set; }

        [JsonProperty("channel_count")]
        public int ChannelCount { get; set; }

        [JsonProperty("calender")]
        public bool EnableCalender { get; set; }

        [JsonProperty("drive")]
        public bool EnableDrive { get; set; }

        [JsonProperty("service_count")]
        public int ServiceCount { get; set; }

        [JsonProperty("task_analytics")]
        public bool EnableTaskAnalytics { get; set; }

        [JsonProperty("task_analytics_customized")]
        public bool EnableTaskAnalyticsCustomized { get; set; }

        [JsonProperty("report")]
        public bool EnableReport { get; set; }

        [JsonProperty("notification_sms")]
        public bool EnableNotificationSMS { get; set; }
        
        [JsonProperty("project_extension_fields")]
        public bool EnableProjectExtensionFields { get; set; }

        [JsonProperty("calendar_organization_assistant")]
        public bool EnableCalendarOrganizationAssistant { get; set; }

        [JsonProperty("security_log")]
        public bool EnableSecurityLog { get; set; }

        [JsonProperty("approval")]
        public bool EnableApproval { get; set; }

        [JsonProperty("leave")]
        public bool EnableLeave { get; set; }

        [JsonProperty("task_workload_analytics")]
        public bool EnableTaskWorkloadAnalytics { get; set; }

        [JsonProperty("crm")]
        public bool EnableCRM { get; set; }

        [JsonProperty("ent_logo_customized")]
        public bool EnableEntLogoCustomized { get; set; }

        [JsonProperty("security_policy")]
        public bool EnableSecurityPolicy { get; set; }

        [JsonProperty("notification_content_customized")]
        public bool EnableNotifyContentCustomized { get; set; }

        [JsonProperty("user_extension_fields")]
        public bool EnableUserExtensionFields { get; set; }

        [JsonProperty("export")]
        public bool EnableExport { get; set; }

        [JsonProperty("sso_saml")]
        public bool EnableSsoSaml { get; set; }

        [JsonProperty("sso_adfs")]
        public bool EnableSSoAdfs { get; set; }

        [JsonProperty("open_api")]
        public bool EnableOpenApi { get; set; }

        [JsonProperty("team_statistic")]
        public bool EnableTeamStatistic { get; set; }

        [JsonProperty("task_gantt_chart")]
        public bool EnableTaskGanttChart { get; set; }

        [JsonProperty("dingtalk")]
        public bool EnableDingTalk { get; set; }

        [JsonProperty("bulletin")]
        public bool EnableBulletin { get; set; }

        [JsonProperty("appraisal")]
        public bool EnableAppraisal { get; set; }

        [JsonProperty("okr")]
        public bool EnableOKR { get; set; }

        [JsonProperty("portal")]
        public bool EnablePortal { get; set; }

        [JsonProperty("notification_email")]
        public bool EnableNotificationEmail { get; set; }
    }
}
