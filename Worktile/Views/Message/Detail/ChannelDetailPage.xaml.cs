using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.ViewModels.Message;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using Worktile.Models.Member;
using Windows.System;
using System;
using Worktile.Common;
using Worktile.Enums;
using Worktile.Common.Communication;
using Worktile.Operators.Message.Detail;
using Worktile.Services;
using System.Collections.ObjectModel;
using Worktile.Models.Message;
using Worktile.Enums.Privileges;
using Worktile.Views.Message.Detail.Content;
using Worktile.Views.Message.Detail.Content.Pin;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class ChannelDetailPage : Page, INotifyPropertyChanged
    {
        public ChannelDetailPage()
        {
            InitializeComponent();
            _userService = new UserService();
            _messageService = new MessageService();
            Navs = new ObservableCollection<TopNav>
            {
                new TopNav { Name = "消息" },
                new TopNav { Name = "文件" },
                new TopNav { Name = "固定消息", IsPin = true }
            };
            SetPermission();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly UserService _userService;
        readonly MessageService _messageService;
        private MasterPage _masterPage;

        public string PaneTitle => "群组成员";
        public bool CanAddService { get; private set; }
        public bool CanAddMember { get; private set; }
        public bool CanAdminChannel { get; private set; }
        public ObservableCollection<TopNav> Navs { get; }

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
                            ContentFrame.Navigate(typeof(ChannelMessagePage));
                            break;
                        case 1:
                            ContentFrame.Navigate(typeof(FilePage));
                            break;
                        case 2:
                            ContentFrame.Navigate(typeof(ChannelPinnedPage), Session);
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
        }

        private void ContactInfo_Click(object sender, RoutedEventArgs e)
        {
            IsPaneOpen = true;
        }

        private void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var listView = sender as ListView;
            var position = e.GetPosition(listView);
            position.X = -2;
            MemberCard.Member = ((FrameworkElement)e.OriginalSource).DataContext as Member;
            MemberCardFlyout.ShowAt(listView, new FlyoutShowOptions
            {
                Position = position,
                Placement = FlyoutPlacementMode.Left
            });
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
            Nav.IsBackButtonVisible = NavigationViewBackButtonVisible.Visible;
            Nav.BackRequested += Nav_BackRequested;
            Nav.IsBackEnabled = true;
        }

        private void Nav_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            ContentFrame.GoBack();
            Nav.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed;
            Nav.BackRequested -= Nav_BackRequested;
            Nav.IsBackEnabled = false;
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
                PrimaryButtonText = "取消",
                SecondaryButtonText = "确认退出",
                DefaultButton = ContentDialogButton.Primary
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Secondary)
            {
                var success = await _messageService.ExitChannelAsync(Session.Id);
                if (success)
                {
                    _masterPage.Sessions.Remove(Session);
                }
            }
        }
    }
}
