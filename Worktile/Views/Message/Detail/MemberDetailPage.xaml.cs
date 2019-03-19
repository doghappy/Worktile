using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Common;
using Worktile.Models.Message.Session;
using Worktile.Services;
using Worktile.Models.Member;
using Worktile.Models.Message;
using System.Collections.ObjectModel;
using System.Linq;
using Worktile.Views.Message.Detail.Content;
using Worktile.Views.Message.Detail.Content.Pin;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class MemberDetailPage : Page, INotifyPropertyChanged
    {
        public MemberDetailPage()
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly UserService _userService;
        readonly MessageService _messageService;

        public string PaneTitle => "成员";
        public ObservableCollection<TopNav> Navs { get; }

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

        private MemberSession _session;
        public MemberSession Session
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

        private Member _member;
        public Member Member
        {
            get => _member;
            set
            {
                if (_member != value)
                {
                    _member = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Member)));
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
                            ContentFrame.Navigate(typeof(MemberMessagePage));
                            break;
                        case 1:
                            ContentFrame.Navigate(typeof(FilePage));
                            break;
                        case 2:
                            ContentFrame.Navigate(typeof(MemberPinnedPage));
                            break;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var masterPage = this.GetParent<MasterPage>();
            Session = masterPage.SelectedSession as MemberSession;
            SelectedNav = Navs.First();
        }

        private async void ContactInfo_Click(object sender, RoutedEventArgs e)
        {
            IsPaneOpen = true;
            IsActive = true;
            Member = await _userService.GetMemberInfoAsync(_session.To.Uid);
            IsActive = false;
        }

        private async void StarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.StarSessionAsync(_session);
            if (result)
            {
                _session.Starred = true;
            }
        }

        private async void UnStarButton_Click(object sender, RoutedEventArgs e)
        {
            bool result = await _messageService.UnStarSessionAsync(_session);
            if (result)
            {
                _session.Starred = false;
            }
        }
    }
}
