using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.Common.Communication;
using Worktile.Views;

namespace Worktile.ViewModels.SignIn
{
    abstract class SignInViewModel : ViewModel
    {
        public Frame Frame { get; set; }

        private bool _showError;
        public bool ShowError
        {
            get => _showError;
            set
            {
                if (_showError != value)
                {
                    _showError = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set
            {
                if (_errorText != value)
                {
                    _errorText = value;
                    OnPropertyChanged();
                }
            }
        }

        public async Task SignUpTeamAsync()
        {
            Uri uri = new Uri("https://worktile.com/signup");
            await Launcher.LaunchUriAsync(uri);
        }

        public async Task SignInAsync(string teamId, string domain, string passToken)
        {
            IsActive = true;
            string url = "https://worktile.com/api/account/user/signin";
            var req = new
            {
                pass_token = passToken,
                team_id = teamId
            };
            var res = await WtHttpClient.PostAsync<ApiResponse>(url, req);
            if (res.Code == 200)
            {
                IsActive = false;
                Frame.Navigate(typeof(MainPage));
                WtHttpClient.Domain = domain;
                ApplicationData.Current.LocalSettings.Values["Domain"] = domain;
            }
        }
    }
}
