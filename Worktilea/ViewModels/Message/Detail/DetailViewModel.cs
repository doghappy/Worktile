using System.Collections.ObjectModel;
using Worktile.Models.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message
{
    abstract class DetailViewModel<S> : MessageViewModel where S : ISession
    {
        public DetailViewModel(S session)
        {
            Session = session;
        }

        public S Session { get; }

        public abstract ObservableCollection<TopNav> Navs { get; protected set; }
        public abstract TopNav SelectedNav { get; set; }
    }
}
