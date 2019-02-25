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
                    ContentFrame.Navigate(typeof(MessageDetailPage), value);
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
            foreach (var item in data.Data.Channels)
            {
                var session = new Session
                {
                    Id = item.Id,
                    DisplayName = item.Name,
                    Background = WtColorHelper.GetSolidColorBrush(WtColorHelper.GetNewColor(item.Color)),
                    Starred = item.Starred,
                    LatestMessageAt = item.LatestMessageAt,
                    Show = item.Show,
                    UnRead = item.UnRead,
                    NamePinyin = item.NamePinyin,
                    Type = SessionType.Channel
                };
                if (item.Visibility == Enums.Visibility.Public)
                {
                    session.Initials = "\uE64E";
                    session.DefaultIcon = "\uE64E";
                    session.AvatarFont = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
                }
                else
                {
                    session.Initials = "\uE748";
                    session.DefaultIcon = "\uE748";
                    session.AvatarFont = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
                }
                list.Add(session);
            }
            foreach (var item in data.Data.Groups)
            {
                var session = new Session
                {
                    Id = item.Id,
                    DisplayName = item.Name,
                    Background = WtColorHelper.GetSolidColorBrush(WtColorHelper.GetNewColor(item.Color)),
                    Starred = item.Starred,
                    LatestMessageAt = item.LatestMessageAt,
                    Show = item.Show,
                    UnRead = item.UnRead,
                    NamePinyin = item.NamePinyin,
                    Type = SessionType.Channel
                };
                if (item.Visibility == Enums.Visibility.Public)
                {
                    session.Initials = "\uE64E";
                    session.DefaultIcon = "\uE64E";
                    session.AvatarFont = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
                }
                else
                {
                    session.Initials = "\uE748";
                    session.DefaultIcon = "\uE748";
                    session.AvatarFont = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
                }
                list.Add(session);
            }
            foreach (var item in data.Data.Sessions)
            {
                list.Add(new Session
                {
                    Id = item.Id,
                    DisplayName = item.To.DisplayName,
                    Initials = AvatarHelper.GetInitials(item.To.DisplayName),
                    ProfilePicture = AvatarHelper.GetAvatarBitmap(item.To.Avatar, AvatarSize.X80, FromType.User),
                    Background = AvatarHelper.GetColorBrush(item.To.DisplayName),
                    Starred = item.Starred,
                    LatestMessageAt = item.LatestMessageAt,
                    Show = item.Show,
                    UnRead = item.UnRead,
                    //NamePinyin = item.To.DisplayName,
                    Component = item.Component,
                    Name = item.To.Name,
                    Type = SessionType.Session,
                    IsBot = item.IsBot
                });
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

            Worktile.MainPage.UnreadBadge += Sessions.Sum(s => s.UnRead);
            Worktile.MainPage.OnMessageReceived += OnMessageReceived;
            Worktile.MainPage.OnFeedReceived += OnFeedReceived;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Worktile.MainPage.OnMessageReceived -= OnMessageReceived;
            Worktile.MainPage.OnFeedReceived -= OnFeedReceived;
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
                        //api/channels/5c726867ccb4bd72fc022399
                        var data = await client.PostAsync<ApiDataResponse<ApiModels.ApiTeamChats.Session>>("/api/session", new { uid = apiMsg.From.Uid });

                        session = new Session
                        {
                            Id = data.Data.Id,
                            DisplayName = data.Data.To.DisplayName,
                            Initials = AvatarHelper.GetInitials(data.Data.To.DisplayName),
                            ProfilePicture = AvatarHelper.GetAvatarBitmap(data.Data.To.Avatar, AvatarSize.X80, FromType.User),
                            Background = AvatarHelper.GetColorBrush(data.Data.To.DisplayName),
                            Starred = data.Data.Starred,
                            LatestMessageAt = data.Data.LatestMessageAt,
                            Show = data.Data.Show,
                            UnRead = 1,
                            NamePinyin = data.Data.To.DisplayName,
                            Component = data.Data.Component,
                            Name = data.Data.To.Name,
                            Type = SessionType.Session,
                            IsBot = data.Data.IsBot
                        };
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (SelectedSession == null || apiMsg.To.Id != SelectedSession.Id)
                    {
                        session.UnRead += 1;
                        Sessions.Remove(session);
                    }
                }
                Sessions.Insert(0, session);
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
                    var session = new Session
                    {
                        Id = data.Data.Id,
                        DisplayName = data.Data.Name,
                        Background = WtColorHelper.GetSolidColorBrush(WtColorHelper.GetNewColor(data.Data.Color)),
                        Starred = data.Data.Starred,
                        LatestMessageAt = data.Data.LatestMessageAt,
                        Show = data.Data.Show,
                        UnRead = data.Data.UnRead,
                        NamePinyin = data.Data.NamePinyin,
                        Type = SessionType.Channel
                    };
                    if (data.Data.Visibility == Enums.Visibility.Public)
                    {
                        session.Initials = "\uE64E";
                        session.DefaultIcon = "\uE64E";
                        session.AvatarFont = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
                    }
                    else
                    {
                        session.Initials = "\uE748";
                        session.DefaultIcon = "\uE748";
                        session.AvatarFont = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
                    }
                    Sessions.Insert(0, session);
                }
                else if (feed.Type == FeedType.RemoveChannel)
                {
                    var session = Sessions.Single(s => s.Id == feed.ChannelId);
                    Sessions.Remove(session);
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
