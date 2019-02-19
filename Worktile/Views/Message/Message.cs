using System;

namespace Worktile.Views.Message
{
    public class Message
    {
        public string Id { get; set; }
        public TethysAvatar Avatar { get; set; }
        public MessageType Type { get; set; }
        public DateTime Time { get; set; }
        //public string Title { get; set; }
        public string Content { get; set; }
    }
}
