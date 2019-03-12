using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Common.WtRequestClient;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Worktile.ViewModels;
using Worktile.Views.Profile;
using Worktile.Views.SignIn;

namespace Worktile.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
        }

        public MainViewModel ViewModel { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsActive = true;
            ViewModel.Dispatcher = Dispatcher;
            ViewModel.ContentFrame = ContentFrame;
            ViewModel.InAppNotification = InAppNotification;
            ViewModel.NavigationView = Nav;
            string domain = ApplicationData.Current.LocalSettings.Values["Domain"]?.ToString();
            if (string.IsNullOrEmpty(domain))
            {
                Frame.Navigate(typeof(PasswordSignInPage));
            }
            else
            {
                WtHttpClient.Domain = domain;
                ViewModel.Logo = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.scale-200.png"));
                await ViewModel.RequestApiUserMeAsync();
                await ViewModel.RequestApiTeamAsync();
            }
            Window.Current.Activated += Window_Activated;
            ViewModel.IsActive = false;
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            ViewModel.WindowActivationState = e.WindowActivationState;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            ViewModel.Dispose();
            Window.Current.Activated -= Window_Activated;
        }

        private void Me_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.SelectedApp = null;
            ContentFrame.Navigate(typeof(ProfilePage));
        }

        private void Setting_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.SelectedApp = null;
            ContentFrame.Navigate(typeof(TestPage));
        }
    }
}
