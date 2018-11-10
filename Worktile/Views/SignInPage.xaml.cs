using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModel.ApiTeamDomainCheck;
using Worktile.ApiModel.ApiTeamLite;
using Worktile.ApiModel.ApiUserMe;
using Worktile.ApiModel.ApiUserSignIn;
using Worktile.Services;
using Worktile.WtRequestClient;

namespace Worktile.Views
{
    public sealed partial class SignInPage : Page, INotifyPropertyChanged
    {
        public const string AuthCookie = "AuthCookie";

        public SignInPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private GridState _gridState;
        public GridState GridState
        {
            get => _gridState;
            set
            {
                _gridState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GridState)));
            }
        }

        const string DOMAIN_SUFFIX = ".worktile.com";

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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Domain)));
                }
            }
        }

        private bool _domainEnable;
        public bool DomainDisable
        {
            get => _domainEnable;
            set
            {
                _domainEnable = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DomainDisable)));
            }
        }
        #endregion

        #region Member Grid Properties
        private string _teamName;
        public string TeamName
        {
            get => _teamName;
            set
            {
                _teamName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TeamName)));
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
            }
        }

        private bool _passwordIsError;
        public bool PasswordIsError
        {
            get => _passwordIsError;
            set
            {
                _passwordIsError = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordIsError)));
            }
        }
        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GridState = GridState.Enterprise;
            Logo = new BitmapImage(new Uri("ms-appx:///Assets/Images/Logo/worktile logo-横版.png"));
        }

        private async void NextStep_Click(object sender, RoutedEventArgs e)
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
                    CommonData.TeamId = lite.Data.Id;
                    TeamName = lite.Data.Name;
                    CommonData.TeamName = TeamName;
                    Logo = new BitmapImage(new Uri(CommonData.ApiUserMeConfig.Box.LogoUrl + lite.Data.OutsideLogo));
                    CommonData.TeamLogo = CommonData.ApiUserMeConfig.Box.LogoUrl + lite.Data.Logo;
                    GridState = GridState.Member;
                }
            }
            IsActive = false;
            TextBoxUserName.Focus(FocusState.Programmatic);
        }

        private async Task<ApiTeamLite> GetTeamLiteAsync()
        {
            string uri = CommonData.SubDomain + "/api/team/lite";
            var client = new WtHttpClient();
            return await client.GetAsync<ApiTeamLite>(uri);
        }

        private async Task RequestApiUserMeAsync()
        {
            string uri = CommonData.SubDomain + "/api/user/me";
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>(uri);
            CommonData.ApiUserMeConfig = me.Data.Config;
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            PasswordIsError = false;
            string uri = CommonData.SubDomain + "/api/user/signin";
            var body = new
            {
                locale = "zh-cn",
                name = UserName,
                password = HashEncryptor.ComputeMd5(Password),
                team_id = CommonData.TeamId
            };
            var client = new WtHttpClient();
            client.OnSuccessStatusCode += resMsg =>
            {
                string cookieString = resMsg.Headers.GetValues("Set-Cookie").First();
                var cookie = WtHttpClient.GetCookieByString(cookieString);
                cookie.Expires = cookie.Expires.AddYears(20);
                cookieString = WtHttpClient.GetStringByCookie(cookie);
                ApplicationData.Current.LocalSettings.Values[AuthCookie] = cookieString;
                WtHttpClient.AddDefaultRequestHeaders("Cookie", cookieString);
            };
            var data = await client.PostAsync<ApiUserSignIn>(uri, body);
            if (client.IsSuccessStatusCode)
            {
                if (data.Code == 200)
                {
                    Frame.Navigate(typeof(MainPage));
                }
                else
                {
                    PasswordIsError = true;
                }
            }
            IsActive = false;
        }

        private async void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string url = CommonData.SubDomain + "/forgot";
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }

    public enum GridState
    {
        Enterprise,
        Member
    }
}
