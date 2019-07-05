using Newtonsoft.Json;

namespace Worktile.Main.Models
{
    public class WtService
    {
        [JsonProperty("service_id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public WtServiceType Type { get; set; }

        [JsonProperty("integration")]
        public string IntegrationId { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        public string Avatar { get; set; }
    }

    public enum WtServiceType
    {
        Inbound = 1,
        Outbound,
        Rss,
        RemoteBot,
        Weixin,
        Mail,
        Weibo,
        Cmd,
        Flurry,
        Trello,
        Jobdeer,
        AppStore,
        Slash,
        Wooyun,
        YangCong,
        Worktiler,
        YoudaoNote,
        Tuling,
        Inbox,
        Sqs,
        MindStore,
        UMeng,
        ZuiMeia,
        EverNote,
        YinXiang,
        BaseCamp,
        Asana,
        Nextkr,
        Worktile,
        Github,
        Quip,
        SendCloud,
        Teambition,
        CustomBot,
        WunderList,
        OneDrive,
        OneNote,
        ApiCloud,
        ShiMo,
        TeamWork,
        Box,
        Processon,
        Plugin,
        Hubot
    }
}
