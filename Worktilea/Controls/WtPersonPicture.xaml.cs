using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Worktile.Enums;

namespace Worktile.Controls
{
    public sealed partial class WtPersonPicture : UserControl, INotifyPropertyChanged
    {
        public WtPersonPicture()
        {
            InitializeComponent();
            _colors = new Color[]
            {
                Color.FromArgb(255, 44, 204, 218),
                Color.FromArgb(255, 45, 188, 255),
                Color.FromArgb(255, 78, 138, 249),
                Color.FromArgb(255, 112, 118, 250),
                Color.FromArgb(255, 148, 115, 253),
                Color.FromArgb(255, 239, 126, 222),
                Color.FromArgb(255, 153, 215, 90),
                Color.FromArgb(255, 102, 192, 96),
                Color.FromArgb(255, 57, 186, 93)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        readonly Color[] _colors;

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                if (_displayName != value)
                {
                    _displayName = value;
                    if (string.IsNullOrEmpty(Initials))
                    {
                        Initials = GetInitials(value);
                    }
                    EllipseColor = GetColor(value);
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
                }
            }
        }

        private ImageSource _profilePicture;
        public ImageSource ProfilePicture
        {
            get => _profilePicture;
            set
            {
                if (_profilePicture != value)
                {
                    _profilePicture = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProfilePicture)));
                }
            }
        }

        private bool _isGroup;
        public bool IsGroup
        {
            get => _isGroup;
            set
            {
                if (_isGroup != value)
                {
                    _isGroup = value;
                    if (value)
                    {
                        ProfilePicture = null;
                        //PersonPicture.FontFamily = new FontFamily("Segoe MDL2 Assets");
                        //Initials = "\uE902";
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsGroup)));
                }
            }
        }

        private WtVisibility _visibility;
        public WtVisibility WtVisibility
        {
            get => _visibility;
            set
            {
                if (_visibility != value)
                {
                    _visibility = value;
                    if (value == WtVisibility.Public)
                    {
                        Initials = "\uE64E";
                        PersonPicture.FontFamily = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
                    }
                    else if (value == WtVisibility.Private)
                    {
                        Initials = "\uE748";
                        PersonPicture.FontFamily = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WtVisibility)));
                }
            }
        }


        private string _initials;
        private string Initials
        {
            get => _initials;
            set
            {
                if (_initials != value)
                {
                    _initials = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Initials)));
                }
            }
        }

        private Color _ellipseColor;
        public Color EllipseColor
        {
            get => _ellipseColor;
            set
            {
                if (_ellipseColor != value)
                {
                    _ellipseColor = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EllipseColor)));
                }
            }
        }

        private string GetInitials(string displayName)
        {
            string initials = null;
            if (Regex.IsMatch(displayName, @"^[\u4e00-\u9fa5]+$") && displayName.Length > 2)
            {
                initials = displayName.Substring(displayName.Length - 2, 2);
            }
            else if (Regex.IsMatch(displayName, @"^[a-zA-Z\/]+$") && displayName.IndexOf(' ') > 0)
            {
                var arr = displayName.Split(' ');
                initials = (arr[0].First().ToString() + arr[1].First()).ToUpper();
            }
            else if (displayName.Length > 2)
            {
                initials = displayName.Substring(0, 2).ToUpper();
            }
            else
            {
                initials = displayName.ToUpper();
            }
            return initials;
        }

        private Color GetColor(string displayName)
        {
            int code = displayName.Sum(n => n);
            return _colors[code % _colors.Length];
        }
    }
}
