using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ViewModels.SignIn;

namespace Worktile.Views.SignIn
{
    public sealed partial class VerifyCodeSignInPage : Page
    {
        public VerifyCodeSignInPage()
        {
            InitializeComponent();
            ViewModel = new VerifyCodeSignInViewModel();
        }

        VerifyCodeSignInViewModel ViewModel { get; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MobileTextBox.Focus(FocusState.Programmatic);
        }

        private void PasswordSignIn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PasswordSignInPage));
        }
    }
}
