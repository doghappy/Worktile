using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Message.Details;
using Worktile.Message.Models;
using Worktile.Models;

namespace Worktile.Message
{
    public sealed partial class MessageDetailPage : Page, INotifyPropertyChanged
    {
        public MessageDetailPage()
        {
            InitializeComponent();
            Navs = new ObservableCollection<MessageNav>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Session Session { get; private set; }

        public ObservableCollection<MessageNav> Navs { get; }

        private MessageNav _selectedNav;
        public MessageNav SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Session = e.Parameter as Session;
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
                    ContentFrame.Navigate(sourcePageType, Session);
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Session.IsAAssistant)
            {
                string unread = UtilityTool.GetStringFromResources("MessageDetailPageNavUnread");
                string read = UtilityTool.GetStringFromResources("MessageDetailPageNavRead");
                string later = UtilityTool.GetStringFromResources("MessageDetailPageNavLater");
                Navs.Add(new MessageNav { Tag = "Unread", Content = unread });
                Navs.Add(new MessageNav { Tag = "Read", Content = read });
                Navs.Add(new MessageNav { Tag = "Later", Content = later });
            }
            else
            {
                string message = UtilityTool.GetStringFromResources("MessageDetailPageNavMessage");
                string file = UtilityTool.GetStringFromResources("MessageDetailPageNavFiles");
                string pin = UtilityTool.GetStringFromResources("MessageDetailPageNavPinnedMessages");
                Navs.Add(new MessageNav { Tag = "Message", Content = message });
                Navs.Add(new MessageNav { Tag = "File", Content = file });
                Navs.Add(new MessageNav { Tag = "Pin", Content = pin });
            }
            SelectedNav = Navs.First();

            string url = $"api/messages/unread/clear?ref_id={Session.Id}";
            var obj = await WtHttpClient.PutAsync(url);
            if (obj.Value<int>("code") == 200)
            {
                //MainViewModel.UnreadMessageCount -= Session.UnRead;
                Session.UnRead = 0;
            }
        }
    }
}
