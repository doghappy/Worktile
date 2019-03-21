using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.Models.Member
{
    public class ServiceInfo
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("remark")]
        public string Remark { get; set; }

        [JsonProperty("has_binding")]
        public bool HasBinding { get; set; }

        [JsonProperty("menus")]
        public List<object> Menus { get; set; }
    }
}
