using System.ComponentModel;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message.Session;
using Windows.UI.Xaml;
using Worktile.Models.Message;
using System.Collections.ObjectModel;
using Worktile.Views.Message.Detail.Content;
using System.Linq;
using Worktile.Common;

namespace Worktile.Views.Message.Detail
{
    public sealed partial class AssistantDetailPage : Page, INotifyPropertyChanged
    {
        public AssistantDetailPage()
        {
            InitializeComponent();
            Navs = new ObservableCollection<TopNav>
            {
                new TopNav { Name = "未读", FilterType = 2 },
                new TopNav { Name = "已读", FilterType = 4 },
                new TopNav { Name = "待处理", FilterType = 3 }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedNav = Navs.First();
            var masterPage = this.GetParent<MasterPage>();
            Session = masterPage.SelectedSession as MemberSession;
        }
    }
}
