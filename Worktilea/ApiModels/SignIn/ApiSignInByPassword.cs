using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Worktile.Models.Team;

namespace Worktile.ApiModels.SignIn.ApiSignInByPassword
{
    public class ApiSignInByPassword
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("pass_token")]
        public string PassToken { get; set; }

        [JsonProperty("teams")]
        public List<Team> Teams { get; set; }

        [JsonProperty("clientIdentifier")]
        public string ClientIdentifier { get; set; }
    }
}
