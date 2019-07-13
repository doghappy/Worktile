using System;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.Main.Profile;
using Worktile.Message;
using Worktile.Models.Exceptions;
using Worktile.Tool;

namespace Worktile.Main
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
        }

        public MainViewModel ViewModel { get; set; }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingPage));
            }
            else
            {
                if (args.SelectedItem != null)
                {
                    ViewModel.SelectedApp = args.SelectedItem as WtApp;
                    switch (ViewModel.SelectedApp.Name)
                    {
                        case "message":
                            ContentFrame.Navigate(typeof(MessagePage));
                            break;
                    }
                }
            }
        }

        private void ProfileNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SelectedApp = null;
            ContentFrame.Navigate(typeof(AccountInfoPage), ViewModel.User);
        }

        private async void DownloadsNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ViewModel.SelectedApp = null;
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(DownloadPage));
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string domain = ApplicationData.Current.LocalSettings.Values["Domain"]?.ToString();
            if (string.IsNullOrEmpty(domain))
            {
                Frame.Navigate(typeof(SignInPage));
            }
            else
            {
                WtHttpClient.Domain = domain;
                try
                {
                    await ViewModel.RequestMeAsync();
                    await ViewModel.RequestTeamAsync();
                    await ViewModel.RequestChatsAsync();
                }
                catch (WtNotLoggedInException)
                {
                    ApplicationData.Current.LocalSettings.Values.Remove("Domain");
                    UtilityTool.RootFrame.Navigate(typeof(SignInPage));
                }
            }
        }
    }
}
