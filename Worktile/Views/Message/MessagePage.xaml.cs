using System;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Common.WtRequestClient;
using Windows.UI.Xaml.Input;
using Worktile.Views.Message.Dialog;
using Worktile.ApiModels;
using Worktile.Views.Message.NavigationParam;
using Windows.System;

namespace Worktile.Views.Message
{
    public sealed partial class MessagePage : Page, INotifyPropertyChanged
    {
        public MessagePage()
        {
            InitializeComponent();
            Sessions = new ObservableCollection<Session>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        Worktile.MainPage _mainPage;

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsActive)));
                }
            }
        }

        public ObservableCollection<Session> Sessions { get; }

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
                    ContentFrame.Navigate(typeof(MessageDetailPage), new ToMessageDetailPageParam
                    {
                        Session = value,
                        MainPage = _mainPage
                    });
                }
            }
        }

        private Windows.UI.Xaml.Visibility _starVisibility;
        public Windows.UI.Xaml.Visibility StarVisibility
        {
            get => _starVisibility;
            set
            {
                if (_starVisibility != value)
                {
                    _starVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StarVisibility)));
                }
            }
        }

        private Windows.UI.Xaml.Visibility _unStarVisibility;
        public Windows.UI.Xaml.Visibility UnStarVisibility
        {
            get => _unStarVisibility;
            set
            {
                if (_unStarVisibility != value)
                {
                    _unStarVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UnStarVisibility)));
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeamChats>("/api/team/chats");
            var list = new List<Session>();
            var channels = data.Data.Channels.Concat(data.Data.Groups);
            foreach (var item in channels)
            {
                var session = MessageHelper.GetSession(item);
                list.Add(session);
            }

            foreach (var item in data.Data.Sessions)
            {
                list.Add(MessageHelper.GetSession(item, AvatarSize.X80));
            }
            list.Sort((a, b) =>
            {
                if (a.Starred && !b.Starred)
                    return -1;
                if (!a.Starred && b.Starred)
                    return 1;
                if (a.LatestMessageAt > b.LatestMessageAt)
                    return -1;
                if (a.LatestMessageAt < b.LatestMessageAt)
                    return 1;
                if (a.Show > 0 && b.Show <= 0)
                    return -1;
                if (a.Show <= 0 && b.Show > 0)
                    return 1;
                if (a.UnRead > 0 && b.UnRead <= 0)
                    return -1;
                if (a.UnRead <= 0 && b.UnRead > 0)
                    return 1;
                return a.NamePinyin.CompareTo(b.NamePinyin);
            });
            foreach (var item in list)
            {
                Sessions.Add(item);
            }
            IsActive = false;

            _mainPage.UnreadBadge += Sessions.Sum(s => s.UnRead);
            _mainPage.OnMessageReceived += OnMessageReceived;
            _mainPage.OnFeedReceived += OnFeedReceived;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _mainPage = e.Parameter as Worktile.MainPage;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _mainPage.OnMessageReceived -= OnMessageReceived;
            _mainPage.OnFeedReceived -= OnFeedReceived;
        }

        private async void OnMessageReceived(Models.Message.Message apiMsg)
        {
            await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                var session = Sessions.SingleOrDefault(s => s.Id == apiMsg.To.Id);
                if (session == null)
                {
                    if (apiMsg.Body.At != null && apiMsg.Body.At.Count == 1)
                    {
                        var client = new WtHttpClient();
                        var data = await client.PostAsync<ApiDataResponse<ApiModels.ApiTeamChats.Session>>("/api/session", new { uid = apiMsg.From.Uid });
                        session = MessageHelper.GetSession(data.Data, AvatarSize.X80);
                        session.UnRead = 1;
                        Sessions.Insert(0, session);
                    }
                    else if (apiMsg.Type == MessageType.Activity)
                    {
                        var client = new WtHttpClient();
                        string url = $"/api/channels/{apiMsg.To.Id}";
                        var data = await client.GetAsync<ApiDataResponse<Channel>>(url);
                        session = MessageHelper.GetSession(data.Data);
                        Sessions.Insert(0, session);
                    }
                }
                else
                {
                    if (SelectedSession == null || apiMsg.To.Id != SelectedSession.Id)
                    {
                        session.UnRead += 1;
                        Sessions.Remove(session);
                        Sessions.Insert(0, session);
                    }
                }
            }));
        }

        private async void OnFeedReceived(Feed feed)
        {
            await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                if (feed.Type == FeedType.NewChannel)
                {
                    string url = $"/api/channels/{feed.ChannelId}";
                    var client = new WtHttpClient();
                    var data = await client.GetAsync<ApiDataResponse<Channel>>(url);
                    var session = MessageHelper.GetSession(data.Data);
                    Sessions.Insert(0, session);
                }
                else if (feed.Type == FeedType.RemoveChannel)
                {
                    var session = Sessions.SingleOrDefault(s => s.Id == feed.ChannelId);
                    if (session != null)
                    {
                        Sessions.Remove(session);
                    }
                }
                else if (feed.Type == FeedType.RemoveChannelMember)
                {
                    var session = Sessions.Single(s => s.Id == feed.ChannelId);
                    if (feed.Uid == DataSource.ApiUserMeData.Me.Uid)
                    {
                        Sessions.Remove(session);
                    }
                }
            }));
        }

        private Session _rightTappedSession;

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            ListViewItemMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            _rightTappedSession = ((FrameworkElement)e.OriginalSource).DataContext as Session;
            if (_rightTappedSession.Starred)
            {
                StarVisibility = Windows.UI.Xaml.Visibility.Collapsed;
                UnStarVisibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StarVisibility = Windows.UI.Xaml.Visibility.Visible;
                UnStarVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private async void CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateGroupDialog();
            dialog.OnCreateSuccess += channel =>
            {
                var session = MessageHelper.GetSession(channel);
                Sessions.Insert(0, session);
            };
            await dialog.ShowAsync();
        }

        private void CreateNewSession(Session session)
        {
            var ss = Sessions.SingleOrDefault(s => s.Id == session.Id);
            if (ss == null)
            {
                Sessions.Insert(0, session);
                SelectedSession = session;
            }
            else if (SelectedSession != ss)
            {
                Sessions.Remove(ss);
                Sessions.Insert(0, ss);
                SelectedSession = ss;
            }
        }

        private async void JoinGroup_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new JoinGroupDialog();
            dialog.OnActived += session => CreateNewSession(session);
            dialog.OnJoined += session =>
            {
                Sessions.Insert(0, session);
                SelectedSession = session;
            };
            await dialog.ShowAsync();
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain+ "/console/members?add=true"));
        }

        private async void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateChatDialog();
            dialog.OnSessionCreated += session => CreateNewSession(session);
            await dialog.ShowAsync();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string url = $"/api/sessions/{_rightTappedSession.Id}";
            var client = new WtHttpClient();
            var data = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                Sessions.Remove(_rightTappedSession);
            }
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            string sessionType = _rightTappedSession.Type.ToString().ToLower();
            string url = $"/api/{sessionType}s/{_rightTappedSession.Id}/star";
            var client = new WtHttpClient();
            var data = await client.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                _rightTappedSession.Starred = true;
            }
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            string sessionType = _rightTappedSession.Type.ToString().ToLower();
            string url = $"/api/{sessionType}s/{_rightTappedSession.Id}/unstar";
            var client = new WtHttpClient();
            var data = await client.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                _rightTappedSession.Starred = false;
            }
        }
    }
}
