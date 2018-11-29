using System;
using System.ComponentModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Common;
using Worktile.ViewModels;

namespace Worktile.Views
{
    public sealed partial class SignInPage : Page, INotifyPropertyChanged
    {
        public const string AuthCookie = "AuthCookie";

        public SignInPage()
        {
            InitializeComponent();
            ViewModel = new SignInViewModel();
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
            ViewModel.GridState = GridState.Enterprise;
            ViewModel.Logo = new BitmapImage(new Uri("ms-appx:///Assets/Images/Logo/worktile logo-横版.png"));
        }

        private async void NextStep_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.CheckTeamAsync();
            TextBoxUserName.Focus(FocusState.Programmatic);
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            void onGetAuthCookie(string cookie) =>
                ApplicationData.Current.LocalSettings.Values[AuthCookie] = cookie;
            if (await ViewModel.SignInAsync(onGetAuthCookie))
            {
                Frame.Navigate(typeof(MainPage));
            }
        }

        private async void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string url = CommonData.SubDomain + "/forgot";
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
