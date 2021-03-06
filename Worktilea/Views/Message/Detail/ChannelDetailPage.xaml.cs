﻿using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Worktile.Models.Message.Session;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using Worktile.Models.Member;
using Windows.System;
using System;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Services;
using System.Collections.ObjectModel;
using Worktile.Models.Message;
using Worktile.Enums.Privileges;
using Worktile.Views.Message.Detail.Content;
using System.Linq;
using Worktile.Common.Extensions;
using Worktile.NavigateModels.Message;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class ChannelDetailPage : Page, INotifyPropertyChanged
    {
        public ChannelDetailPage()
        {
            InitializeComponent();
            _userService = new UserService();
            _messageService = new ChannelMessageService();
            Navs = new ObservableCollection<TopNav>
            {
                new TopNav { Name = "消息" },
                new TopNav { Name = "文件" },
                new TopNav { Name = "固定消息", IsPin = true }
            };
            _ignorePageNames = new[]
            {
                nameof(MessageListPage),
                nameof(AssistantMessagePage),
                nameof(FilePage),
                nameof(PinnedPage)
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly UserService _userService;
        readonly ChannelMessageService _messageService;
        private MasterPage _masterPage;

        public string PaneTitle => "群组成员";
        public ObservableCollection<TopNav> Navs { get; }

        private string[] _ignorePageNames;

        private bool _canAddService;
        public bool CanAddService
        {
            get => _canAddService;
            set
            {
                if (_canAddService != value)
                {
                    _canAddService = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanAddService)));
                }
            }
        }

        private bool _canAddMember;
        public bool CanAddMember
        {
            get => _canAddMember;
            set
            {
                if (_canAddMember != value)
                {
                    _canAddMember = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanAddMember)));
                }
            }
        }

        private bool _canAdminChannel;
        public bool CanAdminChannel
        {
            get => _canAdminChannel;
            set
            {
                if (_canAdminChannel != value)
                {
                    _canAdminChannel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanAdminChannel)));
                }
            }
        }


        private TopNav _selectedNav;
        public TopNav SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    int index = Navs.IndexOf(value);
                    switch (index)
                    {
                        case 0:
                            ContentFrame.Navigate(typeof(MessageListPage), new ToMessageListParam
                            {
                                TopNav = value,
                                Session = Session
                            });
                            break;
                        case 1:
                            ContentFrame.Navigate(typeof(FilePage));
                            break;
                        case 2:
                            ContentFrame.Navigate(typeof(PinnedPage));
                            break;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                }
            }
        }

        private ChannelSession _session;
        public ChannelSession Session
        {
            get => _session;
            set
            {
                if (_session != value)
                {
                    _session = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Session)));
                }
            }
        }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set
            {
                if (_isPaneOpen != value)
                {
                    _isPaneOpen = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsPaneOpen)));
                }
            }
        }

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

        private void SetPermission()
        {
            int adminPermission = Convert.ToInt32(DataSource.Team.Privileges.Admin.Value, 2);
            CanAddService = (adminPermission & (int)AdminPrivilege.Service) != 0;

            int messagePermission = Convert.ToInt32(DataSource.Team.Privileges.Message.Value, 2);
            CanAddMember = Session.Visibility == WtVisibility.Private || (((int)MessagePrivilege.AdminPublicChannel & messagePermission) != 0 && !Session.IsSystem);
            CanAdminChannel = Session.Visibility == WtVisibility.Private || (!Session.IsSystem && ((int)MessagePrivilege.AdminPublicChannel & messagePermission) != 0);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _masterPage = this.GetParent<MasterPage>();
            Session = _masterPage.SelectedSession as ChannelSession;
            SetPermission();
            SelectedNav = Navs.First();
        }

        private void ContactInfo_Click(object sender, RoutedEventArgs e)
        {
            IsPaneOpen = true;
            // 为了显示头像，当 Avatar 未初始化时，需要进行初始化。
            var member = Session.Members.FirstOrDefault();
            if (member != null && member.TethysAvatar == null)
            {
                foreach (var item in Session.Members)
                {
                    item.ForShowAvatar(AvatarSize.X80);
                }
            }
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.StarSessionAsync(Session);
            if (result)
            {
                Session.Starred = true;
            }
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.UnStarSessionAsync(Session);
            if (result)
            {
                Session.Starred = false;
            }
        }

        private async void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(DataSource.SubDomain + "/console/services");
            await Launcher.LaunchUriAsync(uri);
        }

        private void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(AddMemberPage));
        }

        private void Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (ContentFrame.CanGoBack)
            {
                ContentFrame.GoBack();
            }
        }

        private async void ExitChannel_Click(object sender, RoutedEventArgs e)
        {
            string content = Session.Visibility == WtVisibility.Public
                ? "该群组为公开群组，退出群组后，你将收不到该群组中的消息，但是仍然可以搜索到群组消息。稍后你可以从群组列表中重新加入该群组。确定要退出群组吗？"
                : "该群组为私有群组，退出群组后，你将收不到该群组中的消息，并且无法重新加入，除非该群组中的成员邀请你加入。确定要退出群组吗？";
            var dialog = new ContentDialog
            {
                Title = "退出群组",
                Content = content,
                PrimaryButtonText = "确认退出",
                SecondaryButtonText = "取消",
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                await _messageService.ExitChannelAsync(Session.Id);
            }
        }

        private void ChannelSettingButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(ChannelSettingPage));
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            //for (int i = 0; i < ContentFrame.BackStack.Count; i++)
            //{
            //    if (_ignorePageNames.Contains(ContentFrame.BackStack[i].SourcePageType.Name))
            //    {
            //        ContentFrame.BackStack.RemoveAt(i);
            //        if (i != 0)
            //        {
            //            i--;
            //        }
            //    }
            //}

            Nav.IsBackEnabled = ContentFrame.CanGoBack;
            Nav.IsBackButtonVisible = Nav.IsBackEnabled
                ? NavigationViewBackButtonVisible.Visible
                : NavigationViewBackButtonVisible.Collapsed;
            if (e.NavigationMode == NavigationMode.Back)
            {
                var items = Nav.GetChildren<NavigationViewItem>()
                    .Where(n => n.Content != null && n.Content is string)
                    .ToList();
                int index = -1;
                switch (e.SourcePageType)
                {
                    case Type t when e.SourcePageType == typeof(MessageListPage):
                    case Type a when e.SourcePageType == typeof(AssistantMessagePage):
                        index = 0;
                        break;
                    case Type t when e.SourcePageType == typeof(FilePage):
                        index = 1;
                        break;
                    case Type t when e.SourcePageType == typeof(PinnedPage):
                        index = 2;
                        break;
                }
                if (index != -1)
                {
                    items[index].IsSelected = true;
                }
            }
        }

        public void ContentFrameGoBack(int step)
        {
            for (int i = 0; i < step; i++)
            {
                if (ContentFrame.CanGoBack)
                {
                    ContentFrame.GoBack();
                }
            }
        }

        private void MemberInfo_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            var transform = item.TransformToVisual(MembersListView);
            var point = transform.TransformPoint(new Point(0, 0));
            MemberCard.Member = ((FrameworkElement)e.OriginalSource).DataContext as Member;
            MemberCardFlyout.ShowAt(MembersListView, new FlyoutShowOptions
            {
                Position = point,
                Placement = FlyoutPlacementMode.LeftEdgeAlignedTop
            });
        }

        private async void RemoveMember_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            var member = item.DataContext as Member;
            if (member.Uid != DataSource.ApiUserMeData.Me.Uid)
            {
                var result = await _messageService.DeleteMemberFromChannelAsync(Session.Id, member.Uid);
                if (result)
                {
                    Session.Members.Remove(member);
                }
            }
        }
    }
}
