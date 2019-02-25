using Newtonsoft.Json;

namespace Worktile.Models.Member
{
    public class Preference
    {
        [JsonProperty("locale")]
        public string Locale { get; set; }
    }
}
