using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Worktile.WindowsUI.Common;
using Worktile.WindowsUI.Views.Mission;

namespace Worktile.WindowsUI.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            SetBackgroundImage();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string BackgroundImage { get; private set; }


        private void SetBackgroundImage()
        {
            var preference = Configuration.TeamConfig.Me.Preference;
            if (preference.BackgroundImage.StartsWith("desktop-"))
            {
                BackgroundImage = "/Assets/Images/Background/" + preference.BackgroundImage;
            }
            else
            {
                var box = Configuration.TeamConfig.Config.Box;
                int start = box.ServiceUrl.IndexOf("//") + 2;
                int end = box.ServiceUrl.IndexOf('.');
                string from = box.ServiceUrl.Substring(start, end - start);
                BackgroundImage = $"{box.BaseUrl}/background-image/{preference.BackgroundImage}/from-{from}";
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(Mission.HomePage));
            var item = NavView.MenuItems[1] as NavigationViewItem;
            item.IsSelected = true;
            NavView.SelectionChanged += NavView_SelectionChanged;
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {

            }
            else
            {
                var item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag)
                {
                    case "mission":
                        ContentFrame.Navigate(typeof(Mission.HomePage));
                        break;
                }
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {

        }
    }
}
