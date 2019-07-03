using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message.Session;
using Windows.UI.Xaml;
using Worktile.Models.Message;
using System.Collections.ObjectModel;
using Worktile.Views.Message.Detail.Content;
using System.Linq;
using Worktile.Common;
using Worktile.Models.Member;
using Worktile.Services;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class AssistantDetailPage : Page, INotifyPropertyChanged
    {
        public AssistantDetailPage()
        {
            InitializeComponent();
            _userService = new UserService();
            Navs = new ObservableCollection<TopNav>
            {
                new TopNav { Name = "未读", FilterType = 2 },
                new TopNav { Name = "已读", FilterType = 4 },
                new TopNav { Name = "待处理", FilterType = 3 }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        readonly UserService _userService;

        public ObservableCollection<TopNav> Navs { get; }
        public string PaneTitle => "成员";

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

        private TopNav _selectedNav;
        public TopNav SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedNav)));
                    ContentFrame.Navigate(typeof(AssistantMessagePage));
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedNav = Navs.First();
            var masterPage = this.GetParent<MasterPage>();
            Session = masterPage.SelectedSession as MemberSession;
        }

        private async void ContactInfo_Click(object sender, RoutedEventArgs e)
        {
            IsPaneOpen = true;
            if (Member == null)
            {
                IsActive = true;
                Member = await _userService.GetMemberInfoAsync(_session.To.Uid);
                IsActive = false;
            }
        }
    }
}
