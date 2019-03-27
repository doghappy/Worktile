using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Worktile.Models
{
    public class TethysAvatar : INotifyPropertyChanged
    {
        public TethysAvatar()
        {
            AvatarFont = Application.Current.Resources["ContentControlThemeFontFamily"] as FontFamily;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; set; }

        private BitmapImage _source;
        public BitmapImage Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    _source = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Source)));
                }
            }
        }

        public string DisplayName { get; set; }
        public string Icon { get; set; }
        public SolidColorBrush Background { get; set; }
        public FontFamily AvatarFont { get; set; }
        public string DisplayNamePinyin { get; set; }
        public string Name { get; set; }
    }
}
