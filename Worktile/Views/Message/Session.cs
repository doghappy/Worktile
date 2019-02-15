using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Views.Message
{
    public class Session
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public BitmapImage ProfilePicture { get; set; }
        public SolidColorBrush Background { get; set; }
        public SessionType Type { get; set; }
        public bool Starred { get; set; }
        public DateTime LatestMessageAt { get; set; }
        public int Show { get; set; }
        public int UnRead { get; set; }
        public string NamePinyin { get; set; }
        public string Name { get; set; }
        public bool IsBot { get; set; }
        public int? Component { get; set; }
        public bool IsAssistant => Component.HasValue;
        public FontFamily AvatarFont { get; set; }
        public string DefaultIcon { get; set; }
        public string Uid { get; set; }
    }

    public enum SessionType
    {
        Channel,
        Group,
        Session
    }
}
