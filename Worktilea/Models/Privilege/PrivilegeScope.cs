﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace Worktile.Models.Privilege
{
    public class PrivilegeScope
    {
        [JsonProperty("view")]
        public int View { get; set; }

        [JsonProperty("manage")]
        public List<int?> Manage { get; set; }
    }
}
