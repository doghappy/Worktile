using System.Collections.ObjectModel;
using System.Linq;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message
{
    abstract class SessionDetailViewModel<S> : DetailViewModel<S> where S : ISession
    {
        public SessionDetailViewModel(S session) : base(session)
        {
            Navs = new ObservableCollection<TopNav>
            {
                new TopNav { Name = "消息" },
                new TopNav { Name = "文件" },
                new TopNav { Name = "固定消息", IsPin = true }
            };
            SelectedNav = Navs.First();
        }

        public abstract string PaneTitle { get; }

        public override ObservableCollection<TopNav> Navs { get; protected set; }

        private bool _isPaneOpen;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set
            {
                if (_isPaneOpen != value)
                {
                    _isPaneOpen = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
