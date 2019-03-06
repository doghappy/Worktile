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
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Enums.Message;
using Worktile.Models.Message.Session;
using Worktile.Common.Extensions;
using Worktile.Views.Message.Detail;
using Windows.UI.Xaml;

namespace Worktile.ViewModels.Message
{
    class MasterViewModel : MessageViewModel, INotifyPropertyChanged, IDisposable
    {
        public MasterViewModel(MainViewModel mainViewModel, Frame contentFrame)
        {
            _contentFrame = contentFrame;
            _mainViewModel = mainViewModel;
            Sessions = new ObservableCollection<ISession>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private Frame _contentFrame;
        private MainViewModel _mainViewModel;

        public ObservableCollection<ISession> Sessions { get; }

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
                    ContentFrameNavigate(value);
                }
            }
        }

        private Visibility _starVisibility;
        public Visibility StarVisibility
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

        private Visibility _unStarVisibility;
        public Visibility UnStarVisibility
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

        public async Task InitializeAsync()
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeamChats>("/api/team/chats");
            var list = new List<ISession>();
            var channels = data.Data.Channels.Concat(data.Data.Groups);
            foreach (var item in channels)
            {
                item.TethysAvatar = AvatarHelper.GetAvatar(item);
                list.Add(item);
            }

            foreach (var item in data.Data.Sessions)
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
            IsActive = false;

            _mainViewModel.UnreadBadge += Sessions.Sum(s => s.UnRead);
            _mainViewModel.OnMessageReceived += OnMessageReceived;
            _mainViewModel.OnFeedReceived += OnFeedReceived;
        }

        private void ContentFrameNavigate(ISession session)
        {
            Type type = null;
            switch (session.PageType)
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
            _contentFrame.Navigate(type, new ToMessageDetailPageParam
            {
                Session = session,
                MainViewModel = _mainViewModel
            });
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
                        var data = await client.PostAsync<ApiDataResponse<MemberSession>>("/api/session", new { uid = apiMsg.From.Uid });
                        data.Data.ForShowAvatar(AvatarSize.X80);
                        data.Data.UnRead = 1;
                        Sessions.Insert(0, session);
                    }
                    else if (apiMsg.Type == MessageType.Activity)
                    {
                        var client = new WtHttpClient();
                        string url = $"/api/channels/{apiMsg.To.Id}";
                        var data = await client.GetAsync<ApiDataResponse<ChannelSession>>(url);
                        data.Data.ForShowAvatar();
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
                    var data = await client.GetAsync<ApiDataResponse<ChannelSession>>(url);
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

        public async Task DeleteSessionAsync(ISession session)
        {
            string url = $"/api/sessions/{session.Id}";
            var client = new WtHttpClient();
            var data = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (data.Code == 200 && data.Data)
            {
                Sessions.Remove(session);
            }
        }

       

        public void Dispose()
        {
            _mainViewModel.OnMessageReceived -= OnMessageReceived;
            _mainViewModel.OnFeedReceived -= OnFeedReceived;
        }
    }
}
