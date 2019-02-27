using Worktile.Enums.Message;

namespace Worktile.Models.Message
{
    public class MessageContent
    {
        public string Summary { get; set; }
        public string Detail { get; set; }
        public MessageAction Action { get; set; }
        public object Parameter { get; set; }
    }
}
