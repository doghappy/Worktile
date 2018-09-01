using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.WindowsUI.Models.Mission
{
    public class MissionNav
    {
        [JsonProperty("favorites")]
        public List<string> Stick { get; set; }
        public List<MissionNavItem> Items { get; set; }
    }
}
