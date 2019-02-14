using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModels.ApiTeam;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Models;
using Worktile.Common;
using Worktile.Views;
using Worktile.Views.Mission;
using Worktile.WtRequestClient;
using Worktile.Views.IM;
using Windows.UI.Xaml.Navigation;
using Worktile.Infrastructure;
using Worktile.Views.Message;

namespace Worktile
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            AppItems = new ObservableCollection<WtApp>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<WtApp> AppItems { get; }

        //private WtApp _selectedApp;
        //public WtApp SelectedApp
        //{
        //    get => _selectedApp;
        //    set
        //    {
        //        if (_selectedApp != value)
        //        {
        //            _selectedApp = value;
        //            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedApp)));
        //            ContentFrameNavigate(value.Name);
        //        }
        //    }
        //}

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            }
        }

        private BitmapImage _logo;
        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Logo)));
            }
        }

        private BitmapImage _bgImage;
        public BitmapImage BgImage
        {
            get => _bgImage;
            set
            {
                _bgImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BgImage)));
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
            DisplayName = DataSource.ApiUserMe.DisplayName;

            string bgImg = DataSource.ApiUserMe.Preferences.BackgroundImage;
            if (bgImg.StartsWith("desktop-") && bgImg.EndsWith(".jpg"))
            {
                BgImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/" + bgImg));
            }
            else
            {
                string imgUriString = DataSource.ApiUserMeConfig.Box.BaseUrl + "background-image/" + bgImg + "/from-s3";
                byte[] buffer = await client.GetByteArrayAsync(imgUriString);
                BgImage = await ImageHelper.GetImageFromBytesAsync(buffer);
            }
        }

        private async Task RequestApiTeamAsync()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeam>("/api/team");
            DataSource.Team = data.Data;
            AppItems.Add(DataSource.Apps.SingleOrDefault(a => a.Name == "message"));
            DataSource.Team.Apps.ForEach(app =>
            {
                var item = DataSource.Apps.Single(a => a.Name == app.Name);
                AppItems.Add(item);
            });
            //SelectedApp = AppItems.First();
            Logo = new BitmapImage(new Uri(DataSource.ApiUserMeConfig.Box.LogoUrl + DataSource.Team.Logo));
        }

        private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {

            }
            else
            {
                var app = args.SelectedItem as WtApp;
                ContentFrameNavigate(app.Name);
            }
        }

        private void ContentFrameNavigate(string app)
        {
            switch (app)
            {
                case "message":
                    ContentFrame.Navigate(typeof(MessagePage));
                    break;
                case "mission":
                    ContentFrame.Navigate(typeof(MissionPage));
                    break;
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            Nav.IsBackEnabled = ContentFrame.CanGoBack;
            if (e.NavigationMode == NavigationMode.Back)
            {
                int index = -1;
                switch (e.SourcePageType)
                {
                    case Type t when e.SourcePageType == typeof(IMPage):
                        index = 0;
                        break;
                    case Type t when e.SourcePageType == typeof(MissionPage):
                        index = 2;
                        break;
                }
                if (index != -1)
                {
                    var item = Nav.MenuItems[index] as NavigationViewItem;
                    item.IsSelected = true;
                }
            }
        }

        private void Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }
    }
}
