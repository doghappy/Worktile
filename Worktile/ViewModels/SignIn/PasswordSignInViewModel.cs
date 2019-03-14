using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels.SignIn.ApiSignInByPassword;
using Worktile.Common;
using Worktile.Common.Communication;
using Worktile.NavigateModels.SignIn;
using Worktile.Views.SignIn;

namespace Worktile.ViewModels.SignIn
{
    class PasswordSignInViewModel : SignInViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _account;
        public string Account
        {
            get => _account;
            set
            {
                if (_account != value)
                {
                    _account = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                }
            }
        }


        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task SignInAsync()
        {
            IsActive = true;
            string url = "https://worktile.com/api/account/security/exchange/pass-token/by-password";
            var req = new
            {
                signin_name = Account.Trim(),
                password = HashEncryptor.ComputeMd5(Password)
            };
            var res = await WtHttpClient.PostAsync<ApiSignInByPassword>(url, req);
            if (res.Code == 200)
            {
                if (res.Data.Teams.Count == 1)
                {
                    var team = res.Data.Teams[0];
                    await SignInAsync(team.Id, team.Domain, res.Data.PassToken);
                }
                else if (res.Data.Teams.Count > 1)
                {
                    var param = new ToChooseTeamParam
                    {
                        PassToken = res.Data.PassToken,
                        Teams = res.Data.Teams
                    };
                    Frame.Navigate(typeof(ChooseTeamPage), param);
                }
            }
            else if (res.Code == 4000005)
            {
                ErrorText = "帐号或密码输入有误，请重新输入。";
                ShowError = true;
            }
            else if (res.Code == 4000008)
            {
                ErrorText = "请输入正确的手机号或邮箱";
                ShowError = true;
            }
            IsActive = false;
        }
    }
}
