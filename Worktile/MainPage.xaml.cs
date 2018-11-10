using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModel.ApiTeam;
using Worktile.ApiModel.ApiUserMe;
using Worktile.Models;
using Worktile.Services;
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

        private ImageSource _logo;
        public ImageSource Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Logo)));
            }
        }


        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            Logo = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.scale-200.png"));
            await RequestApiUserMeAsync();
            await RequestApiTeamAsync();
            IsActive = false;
        }

        private async Task RequestApiUserMeAsync()
        {
            string uri = CommonData.SubDomain + "/api/user/me";
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>(uri);
            CommonData.ApiUserMeConfig = me.Data.Config;
            CommonData.ApiUserMe = me.Data.Me;
            DisplayName = CommonData.ApiUserMe.DisplayName;
        }

        private async Task RequestApiTeamAsync()
        {
            string uri = CommonData.SubDomain + "/api/team";
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeam>(uri);
            CommonData.Team = data.Data;
            AppItems.Add(CommonData.Apps.SingleOrDefault(a => a.Name == "message"));
            CommonData.Team.Apps.ForEach(app =>
            {
                var item = CommonData.Apps.Single(a => a.Name == app.Name);
                AppItems.Add(item);
            });
            Logo = new BitmapImage(new Uri(CommonData.ApiUserMeConfig.Box.LogoUrl + CommonData.Team.Logo));
        }
    }
}
