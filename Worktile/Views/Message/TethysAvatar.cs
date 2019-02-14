using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Views.Message
{
    public class TethysAvatar
    {
        public BitmapImage Source { get; set; }
        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public SolidColorBrush Background { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public FontFamily AvatarFont { get; set; }
    }
}
