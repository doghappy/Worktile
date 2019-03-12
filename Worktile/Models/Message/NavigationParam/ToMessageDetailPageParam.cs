using Worktile.Models.Message.Session;
using Worktile.ViewModels;
using Worktile.ViewModels.Message;

namespace Worktile.Models.Message.NavigationParam
{
    class ToMessageDetailPageParam
    {
        public ISession Session { get; set; }
        public MainViewModel MainViewModel { get; set; }
        public MasterViewModel MasterViewModel { get; set; }
    }
}
