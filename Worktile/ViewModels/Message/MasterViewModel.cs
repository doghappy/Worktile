using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Worktile.ApiModels;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Common;
using Worktile.Common.WtRequestClient;
using Worktile.Enums;
using Worktile.Views.Message;
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Enums.Message;

namespace Worktile.ViewModels.Message
{
    class MasterViewModel : ViewModel, INotifyPropertyChanged, IDisposable
    {
        public MasterViewModel(MainViewModel mainViewModel, Frame contentFrame)
        {
            _contentFrame = contentFrame;
            _mainViewModel = mainViewModel;
            Sessions = new ObservableCollection<Models.Message.Session>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Frame _contentFrame;
        private MainViewModel _mainViewModel;

        public ObservableCollection<Models.Message.Session> Sessions { get; }

        private Models.Message.Session _selectedSession;
        public Models.Message.Session SelectedSession
        {
            get => _selectedSession;
            set
            {
                if (_selectedSession != value)
                {
                    _selectedSession = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSession)));
                    _contentFrame.Navigate(typeof(DetailPage), new ToMessageDetailPageParam
                    {
                        Session = value,
                        MainViewModel = _mainViewModel
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
                    OnPropertyChanged();
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
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public async Task LoadSesionsAsync()
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeamChats>("/api/team/chats");
            var list = new List<Models.Message.Session>();
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

            _mainViewModel.UnreadBadge += Sessions.Sum(s => s.UnRead);
            _mainViewModel.OnMessageReceived += OnMessageReceived;
            _mainViewModel.OnFeedReceived += OnFeedReceived;
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

        public async Task DeleteSessionAsync(Models.Message.Session session)
        {
            string url = $"/api/sessions/{session.Id}";
            var client = new WtHttpClient();
            var data = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                Sessions.Remove(session);
            }
        }

        public async Task StarSessionAsync(Models.Message.Session session)
        {
            string sessionType = session.Type.ToString().ToLower();
            string url = $"/api/{sessionType}s/{session.Id}/star";
            var client = new WtHttpClient();
            var data = await client.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                session.Starred = true;
            }
        }

        public async Task UnStarSessionAsync(Models.Message.Session session)
        {
            string sessionType = session.Type.ToString().ToLower();
            string url = $"/api/{sessionType}s/{session.Id}/unstar";
            var client = new WtHttpClient();
            var data = await client.PutAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                session.Starred = false;
            }
        }

        public void Dispose()
        {
            _mainViewModel.OnMessageReceived -= OnMessageReceived;
            _mainViewModel.OnFeedReceived -= OnFeedReceived;
        }
    }
}
