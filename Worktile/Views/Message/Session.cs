using System;
using System.ComponentModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Models.Member;

namespace Worktile.Views.Message
{
    public class Session : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public BitmapImage ProfilePicture { get; set; }
        public SolidColorBrush Background { get; set; }
        public SessionType Type { get; set; }

        private bool _starred;
        public bool Starred
        {
            get => _starred;
            set
            {
                if (_starred != value)
                {
                    _starred = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Starred)));
                }
            }
        }

        public DateTime LatestMessageAt { get; set; }
        public int Show { get; set; }

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

        public string NamePinyin { get; set; }
        public string Name { get; set; }
        public bool IsBot { get; set; }
        public int? Component { get; set; }
        public bool IsAssistant => Component.HasValue;
        public FontFamily AvatarFont { get; set; }
        public string DefaultIcon { get; set; }
        public string Uid { get; set; }
        public int RefType => Type == SessionType.Channel ? 1 : 2;

        public Member CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public enum SessionType
    {
        Channel,
        Group,
        Session
    }
}
