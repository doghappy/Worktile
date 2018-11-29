using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModels.ApiTeamDomainCheck;
using Worktile.ApiModels.ApiTeamLite;
using Worktile.ApiModels.ApiUserMe;
using Worktile.ApiModels.ApiUserSignIn;
using Worktile.Common;
using Worktile.WtRequestClient;

namespace Worktile.ViewModels
{
    public class SignInViewModel : ViewModel
    {
        const string DOMAIN_SUFFIX = ".worktile.com";
        string _teamId;

        #region Enterprise Grid Properties
        private string _domain;
        public string Domain
        {
            get => _domain + DOMAIN_SUFFIX;
            set
            {
                value = value.Replace(DOMAIN_SUFFIX, string.Empty);
                if (_domain != value)
                {
                    _domain = value;
                    DomainDisable = false;
                    OnPropertyChanged();
                }
            }
        }

        private bool _domainEnable;
        public bool DomainDisable
        {
            get => _domainEnable;
            set => SetProperty(ref _domainEnable, value);
        }
        #endregion

        #region Member Grid Properties
        private string _teamName;
        public string TeamName
        {
            get => _teamName;
            set => SetProperty(ref _teamName, value);
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private bool _passwordIsError;
        public bool PasswordIsError
        {
            get => _passwordIsError;
            set => SetProperty(ref _passwordIsError, value);
        }
        #endregion

        private GridState _gridState;
        public GridState GridState
        {
            get => _gridState;
            set => SetProperty(ref _gridState, value);
        }

        private BitmapImage _logo;
        public BitmapImage Logo
        {
            get => _logo;
            set => SetProperty(ref _logo, value);
        }

        public async Task CheckTeamAsync()
        {
            IsActive = true;
            DomainDisable = false;
            var client = new WtHttpClient();
            string uri = $"https://worktile.com/api/team/domain/check?domain={_domain}&absolute=true";
            var data = await client.GetAsync<ApiTeamDomainCheck>(uri);
            if (client.IsSuccessStatusCode)
            {
                DomainDisable = !data.Data;
                if (data.Data)
                {
                    CommonData.SubDomain = "https://" + Domain;
                    await RequestApiUserMeAsync();

                    var lite = await GetTeamLiteAsync();
                    TeamName = lite.Data.Name;
                    _teamId = lite.Data.Id;
                    Logo = new BitmapImage(new Uri(CommonData.ApiUserMeConfig.Box.LogoUrl + lite.Data.OutsideLogo));
                    GridState = GridState.Member;
                }
            }
            IsActive = false;
        }

        private async Task RequestApiUserMeAsync()
        {
            string uri = CommonData.SubDomain + "/api/user/me";
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>(uri);
            CommonData.ApiUserMeConfig = me.Data.Config;
        }

        private async Task<ApiTeamLite> GetTeamLiteAsync()
        {
            string uri = CommonData.SubDomain + "/api/team/lite";
            var client = new WtHttpClient();
            return await client.GetAsync<ApiTeamLite>(uri);
        }

        public async Task<bool> SignInAsync(Action<string> onGetAuthCookie)
        {
            IsActive = true;
            PasswordIsError = false;
            string uri = CommonData.SubDomain + "/api/user/signin";
            var body = new
            {
                locale = "zh-cn",
                name = UserName,
                password = HashEncryptor.ComputeMd5(Password),
                team_id = _teamId
            };
            var client = new WtHttpClient();
            client.OnSuccessStatusCode += resMsg =>
            {
                string cookieString = resMsg.Headers.GetValues("Set-Cookie").First();
                var cookie = WtHttpClient.GetCookieByString(cookieString);
                cookie.Expires = cookie.Expires.AddYears(20);
                cookieString = WtHttpClient.GetStringByCookie(cookie);
                onGetAuthCookie?.Invoke(cookieString);
                WtHttpClient.AddDefaultRequestHeaders("Cookie", cookieString);
            };
            var data = await client.PostAsync<ApiUserSignIn>(uri, body);
            if (client.IsSuccessStatusCode)
            {
                if (data.Code == 200)
                {
                    return true;
                }
                else
                {
                    PasswordIsError = true;
                }
            }
            IsActive = false;
            return false;
        }
    }

    public enum GridState
    {
        Enterprise,
        Member
    }
}
