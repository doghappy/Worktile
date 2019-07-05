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
using Worktile.Message.Details;
using Worktile.Message.Models;
using Worktile.Models;

namespace Worktile.Message
{
    public sealed partial class MessageDetailPage : Page
    {
        public MessageDetailPage()
        {
            InitializeComponent();
            ViewModel = new MessageDetailViewModel();
        }

        public MessageDetailViewModel ViewModel { get; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Session = e.Parameter as Session;
            ViewModel.LoadNavs();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (!args.IsSettingsSelected)
            {
                var nav = args.SelectedItem as MessageNav;
                Type sourcePageType = null;
                switch (nav.Tag)
                {
                    case "Message":
                        sourcePageType = typeof(MessageListPage);
                        break;
                }
                if (sourcePageType != null)
                {
                    ContentFrame.Navigate(sourcePageType, ViewModel.Session);
                }
            }
        }
    }
}
