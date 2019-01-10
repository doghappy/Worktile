using System;
using System.Collections.ObjectModel;

namespace Worktile.Models.IM
{
    public class ChatNavItem
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public Type SourcePageType { get; set; }
        public ObservableCollection<Message.Message> Messages { get; }
        public bool HasMore { get; set; }
    }
}
