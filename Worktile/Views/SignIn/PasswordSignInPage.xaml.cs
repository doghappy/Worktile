﻿using System;
using System.Collections.Generic;
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
using Worktile.ViewModels.SignIn;

namespace Worktile.Views.SignIn
{
    public sealed partial class PasswordSignInPage : Page
    {
        public PasswordSignInPage()
        {
            InitializeComponent();
            ViewModel = new PasswordSignInViewModel();
        }

        PasswordSignInViewModel ViewModel { get; }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel.Frame = Frame;
            AccountTextBox.Focus(FocusState.Programmatic);
        }

        private void VerifyCodeSignIn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(VerifyCodeSignInPage));
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Account.Trim() == string.Empty)
            {
                ViewModel.ErrorText = "请输入手机号或者邮箱";
                ViewModel.ShowError = true;
                AccountTextBox.Focus(FocusState.Programmatic);
            }
            else if (string.IsNullOrEmpty(ViewModel.Password))
            {
                ViewModel.ErrorText = "请输入密码";
                ViewModel.ShowError = true;
                PasswordControl.Focus(FocusState.Programmatic);
            }
            else
            {
                ViewModel.ShowError = false;
                await ViewModel.SignInAsync();
            }
        }

        private void PasswordControl_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SignInButton.Focus(FocusState.Programmatic);
                PasswordControl.Focus(FocusState.Programmatic);
                SignInButton_Click(sender, e);
            }
        }
    }
}
