using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Worktile.Models.Team;

namespace Worktile.ApiModels.SignIn.SignInByPassword
{
    public class SignInByPassword
    {
        [JsonProperty("oid")]
        public Guid Oid { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("pass_token")]
        public Guid PassToken { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("clientIdentifier")]
        public string ClientIdentifier { get; set; }
    }
}
