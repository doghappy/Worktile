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

        public SignInViewModel ViewModel { get; }

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
