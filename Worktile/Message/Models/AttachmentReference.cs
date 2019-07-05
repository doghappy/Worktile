using Newtonsoft.Json;

namespace Worktile.Message.Models
{
    public class AttachmentReference
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}
