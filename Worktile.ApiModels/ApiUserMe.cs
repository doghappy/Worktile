using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Worktile.ApiModels.ApiUserMe
{
    public partial class ApiUserMe
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("me")]
        public Me Me { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }
    }

    public partial class Config
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("feed")]
        public Feed Feed { get; set; }

        [JsonProperty("baseUrl")]
        public Uri BaseUrl { get; set; }

        [JsonProperty("outerSiteOrigin")]
        public Uri OuterSiteOrigin { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("hook")]
        public Hook Hook { get; set; }

        [JsonProperty("botId")]
        public string BotId { get; set; }

        [JsonProperty("serviceCallBack")]
        public Uri ServiceCallBack { get; set; }

        [JsonProperty("serviceCallBackOld")]
        public Uri ServiceCallBackOld { get; set; }

        [JsonProperty("weiboClientId")]
        public string WeiboClientId { get; set; }

        [JsonProperty("basecampClientId")]
        public string BasecampClientId { get; set; }

        [JsonProperty("asanaClientId")]
        public string AsanaClientId { get; set; }

        [JsonProperty("youdaonoteClientId")]
        public string YoudaonoteClientId { get; set; }

        [JsonProperty("worktileClientId")]
        public string WorktileClientId { get; set; }

        [JsonProperty("teambitionClientId")]
        public Guid TeambitionClientId { get; set; }

        [JsonProperty("wunderlistClientId")]
        public string WunderlistClientId { get; set; }

        [JsonProperty("wunderlistRedirectUri")]
        public Uri WunderlistRedirectUri { get; set; }

        [JsonProperty("githubClientId")]
        public string GithubClientId { get; set; }

        [JsonProperty("onedriveClientId")]
        public string OnedriveClientId { get; set; }

        [JsonProperty("onenoteClientId")]
        public string OnenoteClientId { get; set; }

        [JsonProperty("boxClientId")]
        public string BoxClientId { get; set; }

        [JsonProperty("shimoClientId")]
        public string ShimoClientId { get; set; }

        [JsonProperty("processonClientId")]
        public string ProcessonClientId { get; set; }

        [JsonProperty("cdnRoot")]
        public Uri CdnRoot { get; set; }

        [JsonProperty("isIndependent")]
        public bool IsIndependent { get; set; }

        [JsonProperty("isSoftDistribution")]
        public bool IsSoftDistribution { get; set; }

        [JsonProperty("paymentPeriod")]
        public long PaymentPeriod { get; set; }

        [JsonProperty("box")]
        public Box Box { get; set; }

        [JsonProperty("browserDownloadAddress")]
        public BrowserDownloadAddress BrowserDownloadAddress { get; set; }
    }

    public partial class Box
    {
        [JsonProperty("baseUrl")]
        public Uri BaseUrl { get; set; }

        [JsonProperty("serviceUrl")]
        public Uri ServiceUrl { get; set; }

        [JsonProperty("avatarUrl")]
        public Uri AvatarUrl { get; set; }

        [JsonProperty("logoUrl")]
        public Uri LogoUrl { get; set; }
    }

    public partial class BrowserDownloadAddress
    {
        [JsonProperty("chrome")]
        public Uri Chrome { get; set; }

        [JsonProperty("firefox")]
        public string Firefox { get; set; }
    }

    public partial class Feed
    {
        [JsonProperty("host")]
        public Uri Host { get; set; }

        [JsonProperty("newHost")]
        public Uri NewHost { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public partial class Hook
    {
        [JsonProperty("host")]
        public Uri Host { get; set; }
    }

    public partial class Me
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("imToken")]
        public string ImToken { get; set; }

        [JsonProperty("role")]
        public long Role { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }

        [JsonProperty("created_at")]
        public long CreatedAt { get; set; }

        [JsonProperty("preferences")]
        public Preferences Preferences { get; set; }
    }

    public partial class Preferences
    {
        [JsonProperty("notify_desktop")]
        public long NotifyDesktop { get; set; }

        [JsonProperty("notify_mobile")]
        public long NotifyMobile { get; set; }

        [JsonProperty("notify_email")]
        public long NotifyEmail { get; set; }

        [JsonProperty("notify_desktop_sound")]
        public long NotifyDesktopSound { get; set; }

        [JsonProperty("emoji_style")]
        public long EmojiStyle { get; set; }

        [JsonProperty("emoji_size")]
        public long EmojiSize { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("theme")]
        public string Theme { get; set; }

        [JsonProperty("background_image")]
        public string BackgroundImage { get; set; }

        [JsonProperty("highlight_words")]
        public List<object> HighlightWords { get; set; }

        [JsonProperty("avoid_disturb")]
        public AvoidDisturb AvoidDisturb { get; set; }

        [JsonProperty("snooze")]
        public Snooze Snooze { get; set; }

        [JsonProperty("is_new_user")]
        public long IsNewUser { get; set; }

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

    public partial class Snooze
    {
        [JsonProperty("enable")]
        public long Enable { get; set; }

        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }
    }
}