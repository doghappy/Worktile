﻿using System;
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
using Worktile.Main.Models;
using Worktile.Setting;
using Worktile.SignInOut;

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
                ViewModel.SelectedApp = args.SelectedItem as WtApp;
            }
        }

        private void ProfileNavigationViewItem_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.User==null)
            {
                ContentFrame.Navigate(typeof(VerifyCodeSignInPage));
            }
            else
            {

            }
        }
    }
}
