using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.SignInOut.Models;

namespace Worktile.SignInOut
{
    public class VerifyCodeSignInViewModel : BindableBase
    {
        public VerifyCodeSignInViewModel()
        {
            LoadAreaNumbers();
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        private bool _showError;
        public bool ShowError
        {
            get => _showError;
            set => SetProperty(ref _showError, value);
        }

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        public List<AreaNumber> Areas { get; private set; }

        private AreaNumber _selectedArea;
        public AreaNumber SelectedArea
        {
            get => _selectedArea;
            set => SetProperty(ref _selectedArea, value);
        }

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set => SetProperty(ref _mobile, value);
        }

        private string _code;
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }

        private string _imageCode;
        public string ImageCode
        {
            get => _imageCode;
            set => SetProperty(ref _imageCode, value);
        }

        private void LoadAreaNumbers()
        {
            string china = UtilityTool.GetStringFromResources("AreaNumberChinaText");
            string taiwan = UtilityTool.GetStringFromResources("AreaNumberTaiwanText");
            string us = UtilityTool.GetStringFromResources("AreaNumberUSText");
            string japan = UtilityTool.GetStringFromResources("AreaNumberJapanText");
            Areas = new List<AreaNumber>
            {
                new AreaNumber { Text = china, Value = "0086" },
                new AreaNumber { Text = taiwan, Value = "00886" },
                new AreaNumber { Text = us, Value = "001" },
                new AreaNumber { Text = japan, Value = "0081" }
            };
            SelectedArea = Areas.First();
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
                //var obj = res.ToObject<ApiSignInByPassword>();
                //if (obj.Data.Teams.Count == 1)
                //{
                //    var team = obj.Data.Teams[0];
                //    await SignInAsync(team.Id, team.Domain, obj.Data.PassToken);
                //}
                //else if (obj.Data.Teams.Count > 1)
                //{
                //    var param = new ToChooseTeamParam
                //    {
                //        PassToken = obj.Data.PassToken,
                //        Teams = obj.Data.Teams
                //    };
                //    Frame.Navigate(typeof(ChooseTeamPage), param);
                //}
            }
            else if (code == 3000004 || code == 3000005)
            {
                //ErrorText = "验证码错误";
                //ShowError = true;
            }
            else if (code == 3000008)
            {
                //ErrorText = "您的验证信息已过期，请点击验证码刷新。";
                //ShowError = true;
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
            var res = await WtHttpClient.RawPostAsync(url, req);
            //switch (res.Code)
            //{
            //    case 3000001:
            //        {
            //            ErrorText = "图形验证码错误";
            //            ShowError = true;
            //            ImageCode = string.Empty;
            //        }
            //        break;
            //    case 3000003:
            //    case 5000009:
            //        {
            //            ErrorText = "验证码类短信1小时内同一手机号发送次数不能超过3次";
            //            ShowError = true;
            //        }
            //        break;
            //}
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
            //var res = await WtHttpClient.PostAsync<ApiResponse>(url, req);
            //if (res.Code == 3000001)
            //{
            //    ErrorText = "图形验证码错误";
            //    ShowError = true;
            //    ImageCode = string.Empty;
            //}
            IsActive = false;
        }
    }
}
