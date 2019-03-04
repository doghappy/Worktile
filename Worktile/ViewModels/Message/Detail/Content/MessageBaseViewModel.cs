using Worktile.Enums.Message;
using Worktile.Models.Message.Session;

namespace Worktile.ViewModels.Message.Detail.Content
{
    abstract class MessageBaseViewModel<S> : ViewModel where S:ISession
    {
        public MessageBaseViewModel(S session)
        {
            Session = session;
        }

        protected S Session { get; }

        protected int RefType => Session.PageType == PageType.Channel ? 1 : 2;
        protected abstract string IdType { get; }
    }
}
