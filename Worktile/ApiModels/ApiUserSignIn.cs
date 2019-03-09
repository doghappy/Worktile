using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.ApiModels.ApiUserSignIn
{
    public partial class ApiUserSignIn
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("role")]
        public long Role { get; set; }

        [JsonProperty("desc")]
        public string Desc { get; set; }

        [JsonProperty("short_code")]
        public string ShortCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("display_name_pinyin")]
        public string DisplayNamePinyin { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("department")]
        public string Department { get; set; }

        [JsonProperty("departments")]
        public List<string> Departments { get; set; }

        [JsonProperty("two_factor")]
        public TwoFactor TwoFactor { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("team_roles")]
        public List<string> TeamRoles { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public partial class Preferences
    {
        [JsonProperty("notify_desktop")]
        public long NotifyDesktop { get; set; }

        [JsonProperty("notify_desktop_sound")]
        public long NotifyDesktopSound { get; set; }

        [JsonProperty("notify_mobile")]
        public long NotifyMobile { get; set; }

        [JsonProperty("notify_mobile_sound")]
        public long NotifyMobileSound { get; set; }

        [JsonProperty("notify_preview")]
        public long NotifyPreview { get; set; }

        [JsonProperty("state_online")]
        public string StateOnline { get; set; }

        [JsonProperty("state_offline")]
        public string StateOffline { get; set; }

        [JsonProperty("state_leave")]
        public string StateLeave { get; set; }

        [JsonProperty("state_busy")]
        public string StateBusy { get; set; }

        [JsonProperty("mobile_push_mode")]
        public long MobilePushMode { get; set; }

        [JsonProperty("mobile_push_due")]
        public long MobilePushDue { get; set; }

        [JsonProperty("notify_email")]
        public long NotifyEmail { get; set; }

        [JsonProperty("notify_email_misc")]
        public long NotifyEmailMisc { get; set; }

        [JsonProperty("notify_email_weekly")]
        public long NotifyEmailWeekly { get; set; }

        [JsonProperty("highlight_words")]
        public List<object> HighlightWords { get; set; }

        [JsonProperty("message_mark_read")]
        public long MessageMarkRead { get; set; }

        [JsonProperty("emoji_style")]
        public long EmojiStyle { get; set; }

        [JsonProperty("emoji_size")]
        public long EmojiSize { get; set; }

        [JsonProperty("is_new_user")]
        public long IsNewUser { get; set; }

        [JsonProperty("theme")]
        public string Theme { get; set; }

        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("subscriptions")]
        public List<Subscription> Subscriptions { get; set; }

        [JsonProperty("snooze")]
        public Snooze Snooze { get; set; }

        [JsonProperty("avoid_disturb")]
        public AvoidDisturb AvoidDisturb { get; set; }

        [JsonProperty("notify_email_settings")]
        public NotifyEmailSettings NotifyEmailSettings { get; set; }

        [JsonProperty("latest_version")]
        public string LatestVersion { get; set; }
    }

    public partial class AvoidDisturb
    {
        [JsonProperty("enable")]
        public long Enable { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("end")]
        public long End { get; set; }
    }

    public partial class NotifyEmailSettings
    {
        [JsonProperty("mission")]
        public Approval Mission { get; set; }

        [JsonProperty("calendar")]
        public Approval Calendar { get; set; }

        [JsonProperty("drive")]
        public Approval Drive { get; set; }

        [JsonProperty("report")]
        public Approval Report { get; set; }

        [JsonProperty("approval")]
        public Approval Approval { get; set; }

        [JsonProperty("crm")]
        public Approval Crm { get; set; }
    }

    public partial class Approval
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Snooze
    {
        [JsonProperty("enable")]
        public long Enable { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }
    }

    public partial class Subscription
    {
        [JsonProperty("last_requested_on")]
        public long LastRequestedOn { get; set; }

        [JsonProperty("type")]
        public long Type { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public partial class TwoFactor
    {
        [JsonProperty("enabled")]
        public long Enabled { get; set; }

        [JsonProperty("recoveries")]
        public List<object> Recoveries { get; set; }
    }
}
