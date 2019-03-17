using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Common.Communication;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Worktile.ViewModels;
using Worktile.Views.Profile;
using Worktile.Views.SignIn;
using Microsoft.Toolkit.Uwp.Connectivity;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Worktile.Enums;
using Worktile.Operators;

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
            NetworkHelper.Instance.NetworkChanged += NetworkChanged;
            ViewModel.IsActive = true;
            MainOperator.ContentFrame = MainContentFrame;
            MainOperator.InAppNotification = InAppNotification;
            //MainOperator.NavView = MainNavView;
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
            MainOperator.WindowActivationState = e.WindowActivationState;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            WtSocket.Dispose();
            Window.Current.Activated -= Window_Activated;
        }

        private void Me_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.SelectedApp = null;
            MainContentFrame.Navigate(typeof(ProfilePage));
        }

        private void Setting_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.SelectedApp = null;
            MainContentFrame.Navigate(typeof(TestPage));
        }

        private async void NetworkChanged(object sender, EventArgs e)
        {
            await Task.Run(async () =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    var helper = sender as NetworkHelper;
                    if (helper.ConnectionInformation.IsInternetAvailable)
                    {
                        MainOperator.ShowNotification("网络已恢复，正在重新连接……", NotificationLevel.Warning, 4000);
                    }
                    else
                    {
                        MainOperator.ShowNotification("网络已断开，正在重新连接……", NotificationLevel.Danger, 4000);
                        WtSocket.ReConnect();
                    }
                });
            });
        }
    }
}
