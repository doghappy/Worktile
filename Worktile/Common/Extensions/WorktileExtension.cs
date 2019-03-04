using Worktile.Enums;
using Worktile.Models.Message.Session;

namespace Worktile.Common.Extensions
{
    public static class WorktileExtension
    {
        public static void ForShowAvatar(this MemberSession session, AvatarSize size)
        {
            session.NamePinyin = session.To.DisplayNamePinyin;
            session.TethysAvatar = AvatarHelper.GetAvatar(session, size);
        }

        public static void ForShowAvatar(this ChannelSession session)
        {
            session.TethysAvatar = AvatarHelper.GetAvatar(session);
        }
    }
}
