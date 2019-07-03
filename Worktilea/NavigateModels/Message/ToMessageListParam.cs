using Worktile.Models.Message;
using Worktile.Models.Message.Session;

namespace Worktile.NavigateModels.Message
{
    class ToMessageListParam
    {
        public TopNav TopNav { get; set; }
        public ISession Session { get; set; }
    }
}
