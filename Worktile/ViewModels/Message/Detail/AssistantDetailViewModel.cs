using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Worktile.Models.Message;
using Worktile.Models.Message.NavigationParam;
using Worktile.Models.Message.Session;
using Worktile.Views.Message.Detail.Content;

namespace Worktile.ViewModels.Message
{
    class AssistantDetailViewModel : DetailViewModel<MemberSession>, INotifyPropertyChanged
    {
        public AssistantDetailViewModel(MemberSession session) : base(session)
        {
            Navs = new ObservableCollection<TopNav>
            {
                new TopNav { Name = "未读", FilterType = 2 },
                new TopNav { Name = "已读", FilterType = 4 },
                new TopNav { Name = "待处理", FilterType = 3 }
            };
            SelectedNav = Navs.First();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override ObservableCollection<TopNav> Navs { get; protected set; }

        private TopNav _selectedNav;
        public override TopNav SelectedNav
        {
            get => _selectedNav;
            set
            {
                if (_selectedNav != value)
                {
                    _selectedNav = value;
                    //AssistantDetailOperator.ContentFrame.Navigate(typeof(AssistantMessagePage), new ToUnReadMsgPageParam
                    //{
                    //    Session = Session,
                    //    Nav = value
                    //});
                    OnPropertyChanged();
                }
            }
        }

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
