using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModel.ApiTeam;
using Worktile.ApiModel.ApiUserMe;
using Worktile.Models;
using Worktile.Services;
using Worktile.Views;
using Worktile.Views.Mission;
using Worktile.WtRequestClient;

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
            WtHttpClient.SetBaseAddress(CommonData.SubDomain);
            Logo = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.scale-200.png"));
            await RequestApiUserMeAsync();
            await RequestApiTeamAsync();
            IsActive = false;
        }

        private async Task RequestApiUserMeAsync()
        {
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>("/api/user/me");
            CommonData.ApiUserMeConfig = me.Data.Config;
            CommonData.ApiUserMe = me.Data.Me;
            DisplayName = CommonData.ApiUserMe.DisplayName;

            string bgImg = CommonData.ApiUserMe.Preferences.BackgroundImage;
            if (bgImg.StartsWith("desktop-") && bgImg.EndsWith(".jpg"))
            {
                BgImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/" + bgImg));
            }
            else
            {
                string imgUriString = CommonData.ApiUserMeConfig.Box.BaseUrl + "background-image/" + bgImg + "/from-s3";
                byte[] buffer = await client.GetByteArrayAsync(imgUriString);
                BgImage = await UtilityTool.GetImageFromBytesAsync(buffer);
            }
        }

        private async Task RequestApiTeamAsync()
        {
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeam>("/api/team");
            CommonData.Team = data.Data;
            AppItems.Add(CommonData.Apps.SingleOrDefault(a => a.Name == "message"));
            CommonData.Team.Apps.ForEach(app =>
            {
                var item = CommonData.Apps.Single(a => a.Name == app.Name);
                AppItems.Add(item);
            });
            Logo = new BitmapImage(new Uri(CommonData.ApiUserMeConfig.Box.LogoUrl + CommonData.Team.Logo));
        }

        private void Nav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {

            }
            else
            {
                var app = args.SelectedItem as WtApp;
                switch (app.Name)
                {
                    case "mission":
                        ContentFrame.Navigate(typeof(MissionPage));
                        break;
                }
            }
        }

        private void ContentFrame_Navigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            Nav.IsBackEnabled = ContentFrame.CanGoBack;
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
