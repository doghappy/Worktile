using Newtonsoft.Json;
using System.Collections.Generic;
using Worktile.WindowsUI.Models.General;

namespace Worktile.WindowsUI.Models.Project
{
    public class WorkAddon
    {
        //[JsonProperty("_id")]
        //public string Id { get; set; }

        [JsonProperty("addon_id")]
        public string Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string Icon { get; set; }

        public List<KeyName> Features { get; set; }
    }
}
