using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Views;
using Worktile.Views.Message;
using Worktile.ApiModels;
using Worktile.Models.Team;
using Worktile.Common.Extensions;
using Worktile.Operators;

namespace Worktile.ViewModels
{
    public class MainViewModel : ViewModel, INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Apps = new ObservableCollection<WtApp>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<WtApp> Apps { get; }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _logo;
        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                _logo = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _bgImage;
        public BitmapImage BgImage
        {
            get => _bgImage;
            set
            {
                _bgImage = value;
                OnPropertyChanged();
            }
        }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get => _selectedApp;
            set
            {
                if (_selectedApp != value)
                {
                    _selectedApp = value;
                    MainOperator.SelectedApp = value;
                    OnPropertyChanged();
                    if (value != null)
                    {
                        ContentFrameNavigate(value.Name);
                    }
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task RequestApiUserMeAsync()
        {
            var me = await WtHttpClient.GetAsync<ApiUserMe>("/api/user/me");
            DataSource.ApiUserMeData = me.Data;
            DisplayName = DataSource.ApiUserMeData.Me.DisplayName;

            string bgImg = DataSource.ApiUserMeData.Me.Preferences.BackgroundImage;
            if (bgImg.StartsWith("desktop-") && bgImg.EndsWith(".jpg"))
            {
                BgImage = new BitmapImage(new Uri("ms-appx:///Assets/Images/Background/" + bgImg));
            }
            else
            {
                string imgUriString = DataSource.ApiUserMeData.Config.Box.BaseUrl + "background-image/" + bgImg + "/from-s3";
                var buffer = await WtHttpClient.GetByteBufferAsync(imgUriString);

                BgImage = await ImageHelper.GetImageAsync(buffer);
            }

            await WtSocket.ConnectSocketAsync();
        }

        public async Task RequestApiTeamAsync()
        {
            var data = await WtHttpClient.GetAsync<ApiDataResponse<Team>>("/api/team");
            DataSource.Team = data.Data;
            Apps.Add(new WtApp
            {
                Name = "message",
                Icon = WtIconHelper.GetAppIcon("message"),
                DisplayName = "消息"
            });
            DataSource.Team.Apps.ForEach(app =>
            {
                app.Icon = WtIconHelper.GetAppIcon(app.Name);
                Apps.Add(app);
            });
            SelectedApp = Apps.First();
            Logo = new BitmapImage(new Uri(DataSource.ApiUserMeData.Config.Box.LogoUrl + DataSource.Team.Logo));

            DataSource.Team.Members.ForEach(m => m.ForShowAvatar(AvatarSize.X80));
        }

        private void ContentFrameNavigate(string app)
        {
            switch (app)
            {
                case "message":
                    MainOperator.ContentFrame.Navigate(typeof(MasterPage), this);
                    break;
                //case "mission":
                //    ContentFrame.Navigate(typeof(MissionPage));
                //    break;
                default:
                    MainOperator.ContentFrame.Navigate(typeof(WaitForDevelopmentPage));
                    break;
            }
        }
    }
}
