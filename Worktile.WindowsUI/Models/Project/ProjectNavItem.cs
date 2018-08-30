using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.WindowsUI.Models.Project
{
    public class ProjectNavItem
    {
        [JsonProperty("nav_id")]
        public string Id { get; set; }

        [JsonProperty("nav_type")]
        public int Type { get; set; }

        public string Name { get; set; }

        public List<ProjectNavItem> Items { get; set; }
    }
}
