using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Windows.UI.Xaml.Input;
using Windows.System;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail;
using System.Threading.Tasks;
using Worktile.Services;
using System.Collections.Generic;
using Worktile.Enums;
using System.Collections.ObjectModel;
using Worktile.Enums.Privileges;
using Worktile.Common.Communication;
using Microsoft.Toolkit.Uwp.Helpers;
using Worktile.Models.Message;
using Worktile.ApiModels;
using Worktile.Enums.Message;
using Worktile.Common.Extensions;

namespace Worktile.Views.Message
{
    public sealed partial class MasterPage : Page, INotifyPropertyChanged
    {
        public MasterPage()
        {
            InitializeComponent();
            _teamService = new TeamService();
            _messageService = new MessageService();
            Sessions = new ObservableCollection<ISession>();
            SetPermission();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly TeamService _teamService;
        readonly MessageService _messageService;
        LightMainPage _mainPage;

        public ObservableCollection<ISession> Sessions { get; }
        public bool CanAddMember { get; private set; }

        private Visibility _starVisibility;
        public Visibility StarVisibility
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

        private Visibility _unStarVisibility;
        public Visibility UnStarVisibility
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

        private ISession _selectedSession;
        public ISession SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (_selectedSession != value)
                {
                    _selectedSession = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSession)));
                    ContentFrameNavigate();
                }
            }
        }

        private void SetPermission()
        {
            int messagePermission = Convert.ToInt32(DataSource.Team.Privileges.Message.Value, 2);
            CanAddMember = ((int)AdminPrivilege.Member & messagePermission) != 0;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _mainPage = this.GetParent<LightMainPage>();
            await LoadChatAsync();

            await _mainPage.UpdateMessageBadgeAsync(Sessions.Sum(s => s.UnRead));
            WtSocket.OnMessageReceived += OnMessageReceived;
            WtSocket.OnFeedReceived += OnFeedReceived;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            WtSocket.OnMessageReceived -= OnMessageReceived;
            WtSocket.OnFeedReceived -= OnFeedReceived;
        }

        private async Task LoadChatAsync()
        {
            _mainPage.IsActive = true;
            var data = await _teamService.GetChatAsync();
            var list = new List<ISession>();
            var channels = data.Channels.Concat(data.Groups);
            foreach (var item in channels)
            {
                item.TethysAvatar = AvatarHelper.GetAvatar(item);
                list.Add(item);
            }

            foreach (var item in data.Sessions)
            {
                item.NamePinyin = item.To.DisplayNamePinyin;
                item.TethysAvatar = AvatarHelper.GetAvatar(item, AvatarSize.X80);
                list.Add(item);
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
            _mainPage.IsActive = false;
        }

        private ISession _rightTappedSession;

        private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            ListViewItemMenuFlyout.ShowAt(listView, e.GetPosition(listView));
            _rightTappedSession = ((FrameworkElement)e.OriginalSource).DataContext as ISession;
            if (_rightTappedSession.Starred)
            {
                StarVisibility = Visibility.Collapsed;
                UnStarVisibility = Visibility.Visible;
            }
            else
            {
                StarVisibility = Visibility.Visible;
                UnStarVisibility = Visibility.Collapsed;
            }
        }

        private void CreateChannel_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(CreateChannelPage);
            EnableNavGoBack(sourcePageType);
        }

        private void JoinChannel_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(JoinChannelPage);
            EnableNavGoBack(sourcePageType);
        }

        private async void AddMember_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(DataSource.SubDomain + "/console/members?add=true"));
        }

        private void CreateChat_Click(object sender, RoutedEventArgs e)
        {
            Type sourcePageType = typeof(CreateChatPage);
            EnableNavGoBack(sourcePageType);
        }

        private void EnableNavGoBack(Type sourcePageType)
        {
            if (MasterContentFrame.CurrentSourcePageType != sourcePageType)
            {
                MasterContentFrame.Navigate(sourcePageType);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.DeleteSessionAsync(_rightTappedSession.Id);
            if (result)
            {
                Sessions.Remove(_rightTappedSession);
            }
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.StarSessionAsync(_rightTappedSession);
            if (result)
            {
                _rightTappedSession.Starred = true;
            }
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.UnStarSessionAsync(_rightTappedSession);
            if (result)
            {
                _rightTappedSession.Starred = false;
            }
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
                        var data = await WtHttpClient.PostAsync<ApiDataResponse<MemberSession>>("/api/session", new { uid = apiMsg.From.Uid });
                        data.Data.ForShowAvatar(AvatarSize.X80);
                        data.Data.UnRead = 1;
                        Sessions.Insert(0, session);
                    }
                    else if (apiMsg.Type == MessageType.Activity)
                    {
                        string url = $"/api/channels/{apiMsg.To.Id}";
                        var data = await WtHttpClient.GetAsync<ApiDataResponse<ChannelSession>>(url);
                        if (!Sessions.Any(s => s.Id == data.Data.Id))
                        {
                            data.Data.ForShowAvatar();
                            Sessions.Insert(0, data.Data);
                        }
                    }
                }
                else
                {
                    if (SelectedSession == null || apiMsg.To.Id != SelectedSession.Id)
                    {
                        session.UnRead += 1;
                        if (Sessions.IndexOf(session) != 0)
                        {
                            Sessions.Remove(session);
                            Sessions.Insert(0, session);
                        }
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
                    var data = await WtHttpClient.GetAsync<ApiDataResponse<ChannelSession>>(url);
                    data.Data.ForShowAvatar();
                    Sessions.Insert(0, data.Data);
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

        private void ContentFrameNavigate()
        {
            if (SelectedSession == null)
            {
                MasterContentFrame.Navigate(typeof(TransparentPage));
                MasterContentFrame.BackStack.Clear();
            }
            else
            {
                Type type = null;
                switch (SelectedSession.PageType)
                {
                    case PageType.Assistant:
                        type = typeof(AssistantDetailPage);
                        break;
                    case PageType.Member:
                        type = typeof(MemberDetailPage);
                        break;
                    case PageType.Channel:
                        type = typeof(ChannelDetailPage);
                        break;
                }
                MasterContentFrame.Navigate(type);
            }
        }

        public void InserSession(ISession session, bool navigate)
        {
            var sourceSession = Sessions.SingleOrDefault(s => s.Id == session.Id);
            if (sourceSession == null)
            {
                Sessions.Insert(0, session);
                if (navigate)
                    SelectedSession = session;
            }
            else
            {
                Sessions.Remove(sourceSession);
                Sessions.Insert(0, sourceSession);
                if (navigate)
                    SelectedSession = sourceSession;
            }
        }
    }
}
