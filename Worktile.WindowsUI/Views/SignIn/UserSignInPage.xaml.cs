using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.ViewModels.Start;

namespace Worktile.WindowsUI.Views.SignIn
{
    public sealed partial class UserSignInPage : Page,INotifyPropertyChanged
    {
        public UserSignInPage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private UserSignInViewModel viewModel;
        public UserSignInViewModel ViewModel
        {
            get => viewModel;
            set
            {
                viewModel = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewModel)));
            }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = new UserSignInViewModel();
            await ViewModel.InitializeAsync();
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.SignInAsync(false);
        }

        private async void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string url = Configuration.BaseAddress + "/forgot";
            await Launcher.LaunchUriAsync(new Uri(url));
        }
    }
}
