using System;
using System.Linq;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Common
{
    public static class MessageHelper
    {
        public static void CompleteMessageFrom(this WtMessage.Message msg)
        {
            try
            {
                var mainPage = SharedData.GetMainPage();
                if (msg.From.Type == WtMessage.FromType.Service)
                {
                    var service = mainPage.Services.Single(u => u.Id == msg.From.Uid);
                    msg.From.Avatar = service.Avatar;
                    msg.From.DisplayName = service.DisplayName;
                }
                else
                {
                    var member = mainPage.Members.Single(u => u.Id == msg.From.Uid);
                    msg.From.Avatar = member.Avatar;
                    msg.From.DisplayName = member.DisplayName;
                }
            }
            catch (InvalidOperationException)
            {
                msg.From.Avatar = string.Empty;
                msg.From.DisplayName = "-";
            }
        }
    }
}
