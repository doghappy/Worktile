using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Worktile.Enums;
using Worktile.Enums.Message;

namespace Worktile.Models.Message
{
    class SendMessageRequestBody
    {
        [JsonProperty("fromType")]
        public FromType FromType { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("toType")]
        public ToType ToType { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("messageType")]
        public MessageType MessageType { get; set; }

        [JsonProperty("client")]
        public Client Client { get; set; }

        [JsonProperty("markdown")]
        public bool IsMarkdown { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
