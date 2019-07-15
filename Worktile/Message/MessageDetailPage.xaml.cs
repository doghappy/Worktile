using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
                    case "Unread":
                        sourcePageType = typeof(UnreadListPage);
                        break;
                    case "File":
                        sourcePageType = typeof(FileListPage);
                        break;
                    case "Read":
                        sourcePageType = typeof(ReadListPage);
                        break;
                    case "Later":
                        sourcePageType = typeof(LaterListPage);
                        break;
                    case "Pin":
                        sourcePageType = typeof(PinListPage);
                        break;
                }
                if (sourcePageType != null)
                {
                    ContentFrame.Navigate(sourcePageType, ViewModel.Session);
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.ClearUnreadAsync();
        }
    }
}
