using Newtonsoft.Json;

namespace Worktile.ApiModel.ApiProjectJoin
{
    public partial class ApiProjectJoin
    {
        [JsonProperty("oid")]
        public string Oid { get; set; }

        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("value")]
        public bool Value { get; set; }
    }
}
