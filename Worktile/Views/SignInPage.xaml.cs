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
using Worktile.ApiModels.ApiTeamDomainCheck;
using Worktile.ApiModels.ApiTeamLite;
using Worktile.ApiModels.ApiUserMe;
using Worktile.ApiModels.ApiUserSignIn;
using Worktile.Common;
using Worktile.Infrastructure;
using Worktile.ViewModels.Infrastructure;
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
                if (_gridState != value)
                {
                    _gridState = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(GridState)));
                }
            }
        }

        const string DOMAIN_SUFFIX = ".worktile.com";

        private BitmapImage _logo;
        public BitmapImage Logo
        {
            get => _logo;
            set
            {
                if (_logo != value)
                {
                    _logo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Logo)));
                }
            }
        }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
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
                if (_domainEnable != value)
                {
                    _domainEnable = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DomainDisable)));
                }
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
                if (_userName != value)
                {
                    _userName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserName)));
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Password)));
                }
            }
        }

        private bool _passwordIsError;
        public bool PasswordIsError
        {
            get => _passwordIsError;
            set
            {
                if (_passwordIsError != value)
                {
                    _passwordIsError = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordIsError)));
                }
            }
        }
        #endregion

        string _teamId;

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
                    DataSource.SubDomain = "https://" + Domain;
                    await RequestApiUserMeAsync();

                    var lite = await GetTeamLiteAsync();
                    TeamName = lite.Data.Name;
                    _teamId = lite.Data.Id;
                    Logo = new BitmapImage(new Uri(DataSource.ApiUserMeConfig.Box.LogoUrl + lite.Data.OutsideLogo));
                    GridState = GridState.Member;
                }
            }
            IsActive = false;
            TextBoxUserName.Focus(FocusState.Programmatic);
        }

        private async Task<ApiTeamLite> GetTeamLiteAsync()
        {
            string uri = DataSource.SubDomain + "/api/team/lite";
            var client = new WtHttpClient();
            return await client.GetAsync<ApiTeamLite>(uri);
        }

        private async Task RequestApiUserMeAsync()
        {
            string uri = DataSource.SubDomain + "/api/user/me";
            var client = new WtHttpClient();
            var me = await client.GetAsync<ApiUserMe>(uri);
            DataSource.ApiUserMeConfig = me.Data.Config;
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            PasswordIsError = false;
            string uri = DataSource.SubDomain + "/api/user/signin";
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
            string url = DataSource.SubDomain + "/forgot";
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }

    public enum GridState
    {
        Enterprise,
        Member
    }
}
