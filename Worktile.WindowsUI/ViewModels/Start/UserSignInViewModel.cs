using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.Models.Results;
using Worktile.WindowsUI.Models.Start;

namespace Worktile.WindowsUI.ViewModels.Start
{
    public class UserSignInViewModel : ViewModel, INotifyPropertyChanged
    {
        public new event PropertyChangedEventHandler PropertyChanged;

        private string account;
        public string Account
        {
            get => account;
            set
            {
                if (value != account)
                {
                    account = value;
                    OnPropertyChanged();
                }
            }
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged();
                }
            }
        }

        private Config config;
        public Config Config
        {
            get => config;
            set
            {
                config = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Config)));
            }
        }

        private TeamLite teamLite;
        public TeamLite TeamLite
        {
            get => teamLite;
            set
            {
                teamLite = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TeamLite)));
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task GetTeamConfigAsync()
        {
            string url = $"{Configuration.BaseAddress}/api/user/me";
            var resMsg = await HttpClient.GetAsync(url);
            if (resMsg.IsSuccessStatusCode)
            {
                string json = await resMsg.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<DataResult<Config>>(json);
                if (data.Code == 200)
                {
                    Configuration.TeamConfig = data.Data;
                    Config = Configuration.TeamConfig;
                }
                else
                {
                    ShowNotification("获取信息失败", 5000);
                }
            }
            else
            {
                await HandleErrorStatusCodeAsync(resMsg);
            }
        }

        private async Task GetTeamInfoAsync()
        {
            string url = $"{Configuration.BaseAddress}/api/team/lite";
            var resMsg = await HttpClient.GetAsync(url);
            if (resMsg.IsSuccessStatusCode)
            {
                string json = await resMsg.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<DataResult<TeamLite>>(json);
                if (data.Code == 200)
                {
                    Configuration.TeamLite = data.Data;
                    TeamLite = Configuration.TeamLite;
                }
                else
                {
                    ShowNotification("获取信息失败", 5000);
                }
            }
            else
            {
                await HandleErrorStatusCodeAsync(resMsg);
            }
        }

        public async Task InitializeAsync()
        {
            await GetTeamConfigAsync();
            await GetTeamInfoAsync();
        }
    }
}
