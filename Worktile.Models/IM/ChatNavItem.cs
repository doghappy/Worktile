using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using System.ComponentModel;

namespace Worktile.Models.IM
{
    public class ChatNavItem : INotifyPropertyChanged
    {
        public ChatNavItem()
        {
            //MessageGroup = new ObservableCollection<MessageGroup>();
            Messages = new ObservableCollection<Message.Message>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Key { get; set; }
        public string Name { get; set; }
        public int FilterType { get; set; }
        //public ObservableCollection<MessageGroup> MessageGroup { get; }
        public ObservableCollection<Message.Message> Messages { get; }
        public bool? HasMore { get; set; }
        public string Next { get; set; }
        public string LatestId { get; set; }
    }
}
