using System.Collections.Generic;
using Newtonsoft.Json;
using Worktile.Models.Entity;

namespace Worktile.ApiModels.Message.ApiMessageFiles
{
    class ApiMessageFiles
    {
        [JsonProperty("data")]
        public Data Data { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }
    }

    class Data
    {
        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }
    }
}
