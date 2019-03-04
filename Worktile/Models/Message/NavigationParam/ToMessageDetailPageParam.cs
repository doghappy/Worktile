using Worktile.Models.Message.Session;
using Worktile.ViewModels;

namespace Worktile.Models.Message.NavigationParam
{
    class ToMessageDetailPageParam
    {
        public ISession Session { get; set; }
        public MainViewModel MainViewModel { get; set; }
    }
}
