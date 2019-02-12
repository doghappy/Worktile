using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Main;
using Worktile.ViewModels;
using Windows.UI.Xaml;
using Worktile.ViewModels.Infrastructure;
using Windows.UI.Xaml.Media.Imaging;
using System;
using Worktile.Views.Message;

namespace Worktile.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            NavBackground = "#6f76fa";

            var navbg = WtColorHelper.GetColor("#575df9");
            NavList.Resources["ListViewItemBackgroundSelected"] = navbg;
            NavList.Resources["ListViewItemBackgroundSelectedPressed"] = navbg;
            NavList.Resources["ListViewItemBackgroundSelectedPointerOver"] = navbg;
            NavList.Resources["ListViewItemBackgroundPointerOver"] = navbg;
            NavList.Resources["ListViewItemBackgroundPressed"] = navbg;
            LogoUrl = "https://s3.cn-north-1.amazonaws.com.cn/lclogo/8dbc28b9-ec0e-4d2e-b66d-ea235525942d_180x180.png";
            AvatarUrl = "https://s3.cn-north-1.amazonaws.com.cn/lcavatar/7f79f351-408d-459f-b880-620eef734b65_80x80.png";
            NavApps = new ObservableCollection<NavApp>
            {
                new NavApp
                {
                    Id = "message",
                    Label = "消息",
                    Glyph = "\ue618",
                    SelectedGlyph = "\ue61e"
                },
                new NavApp
                {
                    Label = "项目",
                    Glyph = "\ue70d",
                    SelectedGlyph = "\ue70c"
                },
                new NavApp
                {
                    Label = "应用",
                    Glyph = "\ue61b",
                    SelectedGlyph = "\ue61d"
                },
                new NavApp
                {
                    Label = "通讯录",
                    Glyph = "\ue617",
                    SelectedGlyph = "\ue604"
                }
            };
            NavApp = NavApps.First();
            BackgroundImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/desktop-4.jpg"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string NavBackground { get; }
        public string LogoUrl { get; }
        public string AvatarUrl { get; }

        public ObservableCollection<NavApp> NavApps { get; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        private NavApp _navApp;
        public NavApp NavApp
        {
            get => _navApp;
            set
            {
                if (_navApp != value)
                {
                    if (_navApp != null)
                    {
                        _navApp.State1Visibility = Visibility.Visible;
                        _navApp.State2Visibility = Visibility.Collapsed;
                    }
                    value.State1Visibility = Visibility.Collapsed;
                    value.State2Visibility = Visibility.Visible;
                    _navApp = value;
                    switch (value.Id)
                    {
                        case "message":
                            ContentFrame.Navigate(typeof(MessagePage));
                            break;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NavApp)));
                }
            }
        }

        private BitmapImage _backgroundImage;
        public BitmapImage BackgroundImage
        {
            get => _backgroundImage;
            set
            {
                _backgroundImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundImage)));
            }
        }
    }
}
