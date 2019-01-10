using System;
using System.Collections.ObjectModel;

namespace Worktile.Models.IM
{
    public class ChatNavItem
    {
        public ChatNavItem()
        {
            Messages = new ObservableCollection<Message.Message>();
        }

        public string Key { get; set; }
        public string Name { get; set; }
        public int FilterType { get; set; }
        public Type SourcePageType { get; set; }
        public ObservableCollection<Message.Message> Messages { get; }
        public bool? HasMore { get; set; }
        public string Next { get; set; }
    }
}
