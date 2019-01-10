using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Worktile.ApiModel.ApiTeamChats;
using Worktile.Models;
using Worktile.Models.IM;
using Worktile.WtRequestClient;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Enums.IM;

namespace Worktile.Views.IM
{
    public sealed partial class IMPage : Page, INotifyPropertyChanged
    {
        public IMPage()
        {
            InitializeComponent();
            ChatNavs = new ObservableCollection<ChatNav>();
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

        public ObservableCollection<ChatNav> ChatNavs { get; }

        private ChatNav _selectedChatNav;
        public ChatNav SelectedChatNav
        {
            get => _selectedChatNav;
            set
            {
                if (_selectedChatNav != value)
                {
                    _selectedChatNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedChatNav)));
                    if (value.IsBot)
                        ContentFrame.Navigate(typeof(ChannelMessagePage), value);
                    else
                        ContentFrame.Navigate(typeof(GeneralMessagePage), value);
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            IsActive = true;
            var client = new WtHttpClient();
            var data = await client.GetAsync<ApiTeamChats>("/api/team/chats");
            var list = new List<ChatNav>();
            foreach (var item in data.Data.Channels)
            {
                list.Add(new ChatNav
                {
                    Id = item.Id,
                    DisplayName = item.Name,
                    Initials = item.Visibility == Enums.Visibility.Public ? "\uE64E" : "\uE652",
                    Background = WtColorHelper.GetColor(WtColorHelper.GetNewColor(item.Color)),
                    Starred = item.Starred,
                    LatestMessageAt = item.LatestMessageAt,
                    Show = item.Show,
                    UnRead = item.UnRead,
                    NamePinyin = item.NamePinyin,
                    Type = ChatType.Channel
                });
            }
            foreach (var item in data.Data.Groups)
            {
                list.Add(new ChatNav
                {
                    Id = item.Id,
                    DisplayName = item.Name,
                    Initials = item.Visibility == Enums.Visibility.Public ? "\uE64E" : "\uE652",
                    Background = WtColorHelper.GetColor(WtColorHelper.GetNewColor(item.Color)),
                    Starred = item.Starred,
                    LatestMessageAt = item.LatestMessageAt,
                    Show = item.Show,
                    UnRead = item.UnRead,
                    NamePinyin = item.NamePinyin,
                    Type = ChatType.Channel
                });
            }
            foreach (var item in data.Data.Sessions)
            {
                list.Add(new ChatNav
                {
                    Id = item.Id,
                    DisplayName = item.To.DisplayName,
                    Initials = AvatarHelper.GetInitials(item.To.DisplayName),
                    ProfilePicture = AvatarHelper.GetAvatarUrl(item.To.Avatar, 80),
                    Background = AvatarHelper.GetColor(item.To.DisplayName),
                    Starred = item.Starred,
                    LatestMessageAt = item.LatestMessageAt,
                    Show = item.Show,
                    UnRead = item.UnRead,
                    //NamePinyin = item.To.DisplayName,
                    Name = item.To.Name,
                    Type = ChatType.Session,
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
                ChatNavs.Add(item);
            }
            IsActive = false;
        }
    }
}
