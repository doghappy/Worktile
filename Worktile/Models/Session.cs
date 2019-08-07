using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;

namespace Worktile.Models
{
    public class Session : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }

        public string Avatar { get; set; }

        public string DisplayName { get; set; }

        private int _unRead;
        public int UnRead
        {
            get => _unRead;
            set
            {
                if (_unRead != value)
                {
                    _unRead = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnRead)));
                }
            }
        }

        private bool _isStar;
        public bool IsStar
        {
            get => _isStar;
            set
            {
                if (_isStar != value)
                {
                    _isStar = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsStar)));
                }
            }
        }

        public SessionType Type { get; set; }

        private DateTimeOffset _latestMessageAt;
        public DateTimeOffset LatestMessageAt
        {
            get => _latestMessageAt;
            set
            {
                if (_latestMessageAt != value)
                {
                    _latestMessageAt = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LatestMessageAt)));
                }
            }
        }

        public string LatestMessageId { get; set; }

        public bool IsAAssistant { get; set; }

        public int RefType { get; set; }

        public WtVisibility Visibility { get; set; }

        public SolidColorBrush Color { get; set; }
    }

    public enum SessionType
    {
        Session,
        Channel
    }
}
