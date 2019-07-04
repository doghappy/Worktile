using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.Models;

namespace Worktile.Main
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            Apps = new ObservableCollection<WtApp>
            {
                new WtApp
                {
                    Name = "message",
                    DisplayName = UtilityTool.GetStringFromResources("WtAppMessageDisplayName"),
                    Icon = WtIconHelper.GetAppIcon("message")
                }
            };
            SelectedApp = Apps.First();
            Members = new ObservableCollection<User>();
        }

        public string IMToken { get; private set; }

        public StorageBox Box { get; private set; }

        public ObservableCollection<WtApp> Apps { get; }

        public ObservableCollection<User> Members { get; }

        private Team _team;
        public Team Team
        {
            get => _team;
            set => SetProperty(ref _team, value);
        }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get => _selectedApp;
            set => SetProperty(ref _selectedApp, value);
        }

        private User _user;
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private string _wtBackgroundImage;
        public string WtBackgroundImage
        {
            get => _wtBackgroundImage;
            set => SetProperty(ref _wtBackgroundImage, value);
        }

        private string _logo;
        public string Logo
        {
            get => _logo;
            set => SetProperty(ref _logo, value);
        }

        public async Task RequestMeAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/user/me");
            User = obj["data"]["me"].ToObject<User>();
            Box = obj["data"]["config"]["box"].ToObject<StorageBox>();
            string img = obj["data"]["me"]["preferences"].Value<string>("background_image");
            if (img.StartsWith("desktop-") && img.EndsWith(".jpg"))
            {
                WtBackgroundImage = "ms-appx:///Assets/Images/Background/" + img;
            }
            else
            {
                WtBackgroundImage = Box.BaseUrl + "background-image/" + img + "/from-s3";
            }
        }

        public async Task RequestTeamAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/team");
            Team = obj["data"].ToObject<Team>();
            Logo = Box.LogoUrl + Team.Logo;
            var members = obj["data"]["members"].Children<JObject>();
            foreach (var item in members)
            {
                Members.Add(item.ToObject<User>());
            }
        }
    }
}
