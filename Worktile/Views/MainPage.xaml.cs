using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Main;
using Windows.UI.Xaml;
using Worktile.ViewModels.Infrastructure;
using Windows.UI.Xaml.Media.Imaging;
using System;
using Worktile.Views.Message;
using Windows.Storage;
using Worktile.WtRequestClient;
using System.Threading.Tasks;
using Worktile.ApiModels.ApiTeam;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Infrastructure;
using Worktile.Common;

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
            AvatarUrl = "https://s3.cn-north-1.amazonaws.com.cn/lcavatar/7f79f351-408d-459f-b880-620eef734b65_80x80.png";
            NavApps = new ObservableCollection<NavApp>
            {
                new NavApp
                {
                    Id = "message",
                    Label = "消息",
                    Glyph = "\ue618",
                    SelectedGlyph = "\ue61e"
                }
            };
            NavApp = NavApps.First();
            BackgroundImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/desktop-4.jpg"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string NavBackground { get; }

        private BitmapImage _logo;
        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                if (_logo != value)
                {
                    _logo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Logo)));
                }
            }
        }

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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            string cookie = ApplicationData.Current.LocalSettings.Values[SignInPage.AuthCookie]?.ToString();
            if (string.IsNullOrEmpty(cookie))
            {
                Frame.Navigate(typeof(SignInPage));
            }
            else
            {
                WtHttpClient.SetBaseAddress(DataSource.SubDomain);
                WtHttpClient.AddDefaultRequestHeaders("Cookie", cookie);
                Logo = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.scale-200.png"));
                await RequestApiUserMeAsync();
                await RequestApiTeamAsync();
            }
            IsActive = false;
        }

        private async Task RequestApiUserMeAsync()
        {
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>("/api/user/me");
            DataSource.ApiUserMeConfig = me.Data.Config;
            DataSource.ApiUserMe = me.Data.Me;

            string bgImg = DataSource.ApiUserMe.Preferences.BackgroundImage;
            if (bgImg.StartsWith("desktop-") && bgImg.EndsWith(".jpg"))
            {
                BackgroundImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/" + bgImg));
            }
            else
            {
                string imgUriString = DataSource.ApiUserMeConfig.Box.BaseUrl + "background-image/" + bgImg + "/from-s3";
                byte[] buffer = await client.GetByteArrayAsync(imgUriString);
                BackgroundImage = await ImageHelper.GetImageFromBytesAsync(buffer);
            }
        }

        private async Task RequestApiTeamAsync()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeam>("/api/team");
            DataSource.Team = data.Data;

            for (int i = 0; i < 3; i++)
            {
                var item = DataSource.Apps.Single(a => a.Name == DataSource.Team.Apps[i].Name);
                NavApps.Add(new NavApp
                {
                    Label = item.DisplayName,
                    Glyph = item.Glyph,
                    SelectedGlyph = item.GlyphO
                });
            }
            NavApps.Add(new NavApp
            {
                Label = "应用",
                Glyph = "\ue61b",
                SelectedGlyph = "\ue61d"
            });
            NavApps.Add(new NavApp
            {
                Label = "通讯录",
                Glyph = "\ue617",
                SelectedGlyph = "\ue604"
            });

            Logo = new BitmapImage(new Uri(DataSource.ApiUserMeConfig.Box.LogoUrl + DataSource.Team.Logo));
        }
    }
}
