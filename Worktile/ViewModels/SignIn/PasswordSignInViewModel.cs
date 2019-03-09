using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.ApiModels.SignIn.ApiSignInByPassword;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.PageModels;

namespace Worktile.ViewModels.SignIn
{
    class PasswordSignInViewModel : ViewModel, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

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
            string url = "https://worktile.com/api/account/security/exchange/pass-token/by-password";
            var req = new
            {
                signin_name = Account.Trim(),
                password = HashEncryptor.ComputeMd5(Password)
            };
            var client = new WtHttpClient();
            var res = await client.PostAsync<ApiSignInByPassword>(url, req);
            if (res.Code == 200)
            {

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
        }
    }
}
