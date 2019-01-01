﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Worktile
{
    public sealed partial class TestPage : Page
    {
        public TestPage()
        {
            InitializeComponent();
            Persons = new List<Person>
            {
                new Person { Id = 1, Name = "Bob" },
                new Person { Id = 2, Name = "Mary" },
                new Person { Id = 2, Name = "Mary" },
                new Person { Id = 2, Name = "HeroWong" }
            };
        }

        List<Person> Persons { get; }

        private async void SystemDialog_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Title",
                PrimaryButtonText = "OK"
            };
            await dialog.ShowAsync();
        }

        private async void NewView_Click(object sender, RoutedEventArgs e)
        {
            var newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(Views.Mission.Project.Detail.DetailPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();
                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Tb.Text = "\uE64E \uE652";
            Tb.FontFamily = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
            //Ta.Initials = "\uE64E";
            Ta.FontFamily = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
        }
    }
    class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
