using System.ComponentModel;

namespace Worktile.Main.Models
{
    public class WtApp : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }

        private int _unreadCount;
        public int UnreadCount
        {
            get => _unreadCount;
            set
            {
                if (_unreadCount != value)
                {
                    _unreadCount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnreadCount)));
                }
            }
        }
    }
}
