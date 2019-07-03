using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Models
{
    public class Avatar
    {
        public string DisplayName { get; set; }
        public BitmapImage ProfilePicture { get; set; }
        public string Initials { get; set; }
    }
}
