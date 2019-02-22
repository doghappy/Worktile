using Newtonsoft.Json;
using System.Collections.Generic;
using Worktile.Views.Message;

namespace Worktile.Models.Department
{
    public class DepartmentNode
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public DepartmentNodeType Type { get; set; }

        [JsonProperty("addition")]
        public DepartmentNodeAddition Addition { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("children")]
        public List<DepartmentNode> Children { get; set; }

        public TethysAvatar Avatar { get; set; }
    }
}
