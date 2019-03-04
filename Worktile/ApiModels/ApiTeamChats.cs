using System.Collections.Generic;
using Newtonsoft.Json;
using Worktile.Models.Message.Session;

namespace Worktile.ApiModels.ApiTeamChats
{
    public partial class ApiTeamChats
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("channels")]
        public List<ChannelSession> Channels { get; set; }

        [JsonProperty("groups")]
        public List<ChannelSession> Groups { get; set; }

        [JsonProperty("sessions")]
        public List<MemberSession> Sessions { get; set; }
    }
}
