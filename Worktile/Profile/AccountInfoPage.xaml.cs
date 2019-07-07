using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Models;

namespace Worktile.Profile
{
    public sealed partial class AccountInfoPage : Page
    {
        public AccountInfoPage()
        {
            InitializeComponent();
            ViewModel = new AccountInfoViewModel();
        }

        public AccountInfoViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.User = e.Parameter as User;
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values.Remove("Domain");
            UtilityTool.ReloadMainPage();
        }
    }
}
