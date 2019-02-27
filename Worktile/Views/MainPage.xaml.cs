using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.ApiModels.ApiTeam;
using Worktile.ApiModels.ApiUserMe;
using Worktile.Models;
using Worktile.Common;
using Worktile.Views;
using Worktile.Common.WtRequestClient;
using Windows.UI.Xaml.Navigation;
using Worktile.Views.Message;
using Windows.Networking.Sockets;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Storage.Streams;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Domain.SocketMessageConverter.Converters;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Worktile.Enums;
using Windows.System.Threading;
using Windows.UI.Core;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Media;
using Worktile.ViewModels;

namespace Worktile.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainViewModel(Dispatcher, ContentFrame, InAppNotification);
        }

        public MainViewModel ViewModel { get; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.IsActive = true;
            string cookie = ApplicationData.Current.LocalSettings.Values[SignInPage.AuthCookie]?.ToString();
            if (string.IsNullOrEmpty(cookie))
            {
                Frame.Navigate(typeof(SignInPage));
            }
            else
            {
                WtHttpClient.SetBaseAddress(DataSource.SubDomain);
                WtHttpClient.AddDefaultRequestHeaders("Cookie", cookie);
                ViewModel.Logo = new BitmapImage(new Uri("ms-appx:///Assets/StoreLogo.scale-200.png"));
                await ViewModel.RequestApiUserMeAsync();
                await ViewModel.RequestApiTeamAsync();
            }
            ViewModel.IsActive = false;
            Window.Current.Activated += Window_Activated;
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
            ContentFrame.Navigate(typeof(WaitForDevelopmentPage));
        }

        private void Setting_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ViewModel.SelectedApp = null;
            ContentFrame.Navigate(typeof(TestPage));
        }
    }
}
