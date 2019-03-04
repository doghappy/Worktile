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
        
        // To do:
        // 1. 将 ViewModel 和 Views 放置到 Read 和 UnRead 文件夹
        // 2. 写一个中间类 FileMessageViewModel，派生自此 MessageBaseViewModel
        // MessageViewModel 和 FileViewModel 类继承 FileMessageViewModel
    }
}
