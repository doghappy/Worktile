using System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Worktile.Enums.IM;

namespace Worktile.Models.IM
{
    public class ChatNav
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Initials { get; set; }
        public ImageSource ProfilePicture { get; set; }
        public Color Background { get; set; }
        public ChatType Type { get; set; }
        public bool Starred { get; set; }
        public DateTime LatestMessageAt { get; set; }
        public int Show { get; set; }
        public int UnRead { get; set; }
        public string NamePinyin { get; set; }
        public string Name { get; set; }
    }
}
