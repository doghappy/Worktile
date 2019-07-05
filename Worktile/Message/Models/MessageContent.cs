namespace Worktile.Message.Models
{
    public class MessageContent
    {
        public string Summary { get; set; }
        public string Detail { get; set; }
        public MessageAction Action { get; set; }
        public object Parameter { get; set; }
    }
}
