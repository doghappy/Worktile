using Worktile.ViewModels;

namespace Worktile.Views.Message.NavigationParam
{
    public class ToUnReadMsgPageParam
    {
        public Session Session { get; set; }
        public TopNav Nav { get; set; }
        public MainViewModel MainViewModel { get; set; }
    }
}
