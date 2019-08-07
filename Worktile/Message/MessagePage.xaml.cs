using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Worktile.Main;
using Worktile.Message.Dialogs;
using Worktile.Models;
using Worktile.Common;
using System.Linq;

namespace Worktile.Message
{
    public sealed partial class MessagePage : Page, INotifyPropertyChanged
    {
        public MessagePage()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string RightTappedSessionType => RightTappedSession.Type.ToString().ToLower() + "s";

        private ObservableCollection<Session> _sessions;
        public ObservableCollection<Session> Sessions
        {
            get => _sessions;
            set
            {
                if (_sessions != value)
                {
                    _sessions = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sessions)));
                }
            }
        }

        private Session _selectedSession;
        public Session SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (_selectedSession != value)
                {
                    _selectedSession = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSession)));
                    //ContentFrame.Navigate(typeof(MessageDetailPage), value);
                }
            }
        }

        private Session _rightTappedSession;
        public Session RightTappedSession
        {
            get => _rightTappedSession;
            set
            {
                if (_rightTappedSession != value)
                {
                    _rightTappedSession = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RightTappedSession)));
                    ContentFrame.Navigate(typeof(MessageDetailPage), value);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var mainPage = SharedData.GetMainPage();
            Sessions = mainPage.Sessions;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (Sessions.Count > 1)
            {
                bool success;
                if (RightTappedSession.Type == SessionType.Channel)
                {
                    string url = $"api/channels/{RightTappedSession.Id}/disable";
                    var data = await WtHttpClient.PutAsync(url);
                    success = data.Value<int>("code") == 200;
                }
                else
                {
                    string url = $"api/sessions/{RightTappedSession.Id}";
                    var data = await WtHttpClient.DeleteAsync(url);
                    success = data.Value<int>("code") == 200 && data.Value<bool>("data");
                }
                if (success)
                {
                    Sessions.Remove(RightTappedSession);
                    SelectedSession = Sessions.First();
                }
            }
        }

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            ListViewItemMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            RightTappedSession = ((FrameworkElement)e.OriginalSource).DataContext as Session;
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            string url = $"api/{RightTappedSessionType}/{RightTappedSession.Id}/star";
            var data = await WtHttpClient.PutAsync(url);
            if (data.Value<int>("code") == 200 && data.Value<bool>("data"))
            {
                RightTappedSession.IsStar = true;
            }
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            string url = $"api/{RightTappedSessionType}/{RightTappedSession.Id}/unstar";
            var data = await WtHttpClient.PutAsync(url);
            if (data.Value<int>("code") == 200 && data.Value<bool>("data"))
            {
                RightTappedSession.IsStar = false;
            }
        }

        private async void CreateChannel_Click(object sender, RoutedEventArgs e)
        {
            //Type sourcePageType = typeof(CreateChannelPage);
            //EnableNavGoBack(sourcePageType);
            var dialog = new CreateGroupDialog();
            await dialog.ShowAsync();
        }

        private void JoinChannel_Click(object sender, RoutedEventArgs e)
        {
            //Type sourcePageType = typeof(JoinChannelPage);
            //EnableNavGoBack(sourcePageType);
        }

        //private async void AddMember_Click(object sender, RoutedEventArgs e)
        //{
        //    await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain + "/console/members?add=true"));
        //}

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            //Type sourcePageType = typeof(CreateChatPage);
            //EnableNavGoBack(sourcePageType);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ContentFrame.Navigate(typeof(MessageDetailPage), e.AddedItems[0]);
        }
    }
}
