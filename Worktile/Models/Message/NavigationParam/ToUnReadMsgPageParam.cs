using Worktile.Models.Message.Session;
using Worktile.ViewModels;

namespace Worktile.Models.Message.NavigationParam
{
    public class ToUnReadMsgPageParam
    {
        public ISession Session { get; set; }
        public TopNav Nav { get; set; }
    }
}
