using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Worktile.ApiModels;
using Worktile.ApiModels.SignIn.ApiSignInByPassword;
using Worktile.Common.Communication;
using Worktile.NavigateModels.SignIn;
using Worktile.PageModels;
using Worktile.Views.SignIn;

namespace Worktile.ViewModels.SignIn
{
    class VerifyCodeSignInViewModel : SignInViewModel, INotifyPropertyChanged
    {
        public VerifyCodeSignInViewModel()
        {
            Areas = new List<WtKeyValue>
            {
                new WtKeyValue("中国 +86", "0086"),
                new WtKeyValue("台湾 +886", "00886"),
                new WtKeyValue("United States +1", "001"),
                new WtKeyValue("日本 +81", "0081")
            };
            SelectedArea = Areas.First();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<WtKeyValue> Areas { get; }

        private WtKeyValue _selectedArea;
        public WtKeyValue SelectedArea
        {
            get => _selectedArea;
            set
            {
                if (_selectedArea != value)
                {
                    _selectedArea = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set
            {
                if (_mobile != value)
                {
                    _mobile = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                if (_code != value)
                {
                    _code = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _imageCode;
        public string ImageCode
        {
            get => _imageCode;
            set
            {
                if (_imageCode != value)
                {
                    _imageCode = value;
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
            string url = "https://worktile.com/api/account/security/exchange/pass-token/by-verification-code";
            var req = new
            {
                scope = "signin",
                verification_code = Code,
                mobile = new
                {
                    area = SelectedArea.Value,
                    mobile = Mobile
                }
            };
            var res = await WtHttpClient.RawPostAsync(url, req);
            int code = res.Value<int>("code");
            if (code == 200)
            {
                var obj = res.ToObject<ApiSignInByPassword>();
                if (obj.Data.Teams.Count == 1)
                {
                    var team = obj.Data.Teams[0];
                    await SignInAsync(team.Id, team.Domain, obj.Data.PassToken);
                }
                else if (obj.Data.Teams.Count > 1)
                {
                    var param = new ToChooseTeamParam
                    {
                        PassToken = obj.Data.PassToken,
                        Teams = obj.Data.Teams
                    };
                    Frame.Navigate(typeof(ChooseTeamPage), param);
                }
            }
            else if (code == 3000004 || code == 3000005)
            {
                ErrorText = "验证码错误";
                ShowError = true;
            }
            else if (code == 3000008)
            {
                ErrorText = "您的验证信息已过期，请点击验证码刷新。";
                ShowError = true;
            }
            IsActive = false;
        }

        public async Task SendCodeAsync()
        {
            IsActive = true;
            string url = "https://worktile.com/api/account/security/verification-code/sms";
            var req = new
            {
                scope = "signin",
                captcha_code = ImageCode,
                mobile = new
                {
                    area = SelectedArea.Value,
                    mobile = Mobile
                }
            };
            var res = await WtHttpClient.PostAsync<ApiResponse>(url, req);
            switch (res.Code)
            {
                case 3000001:
                    {
                        ErrorText = "图形验证码错误";
                        ShowError = true;
                        ImageCode = string.Empty;
                    }
                    break;
                case 3000003:
                case 5000009:
                    {
                        ErrorText = "验证码类短信1小时内同一手机号发送次数不能超过3次";
                        ShowError = true;
                    }
                    break;
            }
            IsActive = false;
        }

        public async Task SendVoiceAsync()
        {
            IsActive = true;
            string url = "https://worktile.com/api/account/security/verification-code/voice";
            var req = new
            {
                scope = "signin",
                mobile = new
                {
                    area = SelectedArea.Value,
                    mobile = Mobile
                }
            };
            var res = await WtHttpClient.PostAsync<ApiResponse>(url, req);
            if (res.Code == 3000001)
            {
                ErrorText = "图形验证码错误";
                ShowError = true;
                ImageCode = string.Empty;
            }
            IsActive = false;
        }
    }
}
