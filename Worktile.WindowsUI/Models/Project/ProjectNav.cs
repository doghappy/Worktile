using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.WindowsUI.Models.Project
{
    public class ProjectNav
    {
        [JsonProperty("favorites")]
        public List<string> Stick { get; set; }
        public List<ProjectNavItem> Items { get; set; }
    }
}
