using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Enums;

namespace Worktile.Models
{
    public class TethysAvatar
    {
        public TethysAvatar()
        {
            AvatarFont = Application.Current.Resources["ContentControlThemeFontFamily"] as FontFamily;
        }

        public string Id { get; set; }
        public BitmapImage Source { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public SolidColorBrush Background { get; set; }
        public FontFamily AvatarFont { get; set; }
        public string DisplayNamePinyin { get; set; }
        public string Name { get; set; }
    }
}
