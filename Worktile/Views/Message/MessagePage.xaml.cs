﻿using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.ApiModel.ApiTeamChats;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Common.WtRequestClient;

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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            Worktile.MainPage.OnMessageReceived -= OnMessageReceived;
        }

        private async void OnMessageReceived(Models.Message.Message apiMsg)
        {
            if (SelectedSession == null || apiMsg.To.Id != SelectedSession.Id)
            {
                var session = Sessions.Single(s => s.Id == apiMsg.To.Id);
                await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => session.UnRead += 1));
            }
        }
    }
}
