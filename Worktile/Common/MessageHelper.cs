using System.Linq;
using Worktile.Main;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Common
{
    public static class MessageHelper
    {
        public static void CompleteMessageFrom(this WtMessage.Message msg)
        {
            if (msg.From.Type == WtMessage.FromType.Service)
            {
                var service = MainViewModel.Services.Single(u => u.Id == msg.From.Uid);
                msg.From.Avatar = service.Avatar;
                msg.From.DisplayName = service.DisplayName;
            }
            else
            {
                var member = MainViewModel.Members.Single(u => u.Id == msg.From.Uid);
                msg.From.Avatar = member.Avatar;
                msg.From.DisplayName = member.DisplayName;
            }
        }
    }
}
