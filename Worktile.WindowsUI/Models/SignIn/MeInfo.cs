using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Worktile.WindowsUI.Models.SignIn
{
    public class MeInfo
    {
        public string Uid { get; set; }

        public string Name { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Title { get; set; }

        public string Avatar { get; set; }

        public string ImToken { get; set; }

        public int Role { get; set; }

        [JsonProperty("team")]
        public string TeamId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
