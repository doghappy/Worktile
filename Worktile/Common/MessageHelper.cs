using System.Linq;
using Windows.UI.Xaml.Media;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models.Message;
using Worktile.Views.Message;

namespace Worktile.Common
{
    public static class MessageHelper
    {
        public static Models.Message.Session GetSession(Channel channel)
        {
            var session = new Models.Message.Session
            {
                Id = channel.Id,
                DisplayName = channel.Name,
                Background = WtColorHelper.GetSolidColorBrush(WtColorHelper.GetNewColor(channel.Color)),
                Starred = channel.Starred,
                LatestMessageAt = channel.LatestMessageAt,
                Show = channel.Show,
                UnRead = channel.UnRead,
                NamePinyin = channel.NamePinyin,
                Type = SessionType.Channel,
                Joined = channel.Joined,
                CreatedBy = new Models.Member.Member
                {
                    DisplayName = channel.CreatedBy.DisplayName,
                    DisplayNamePinyin = channel.CreatedBy.DisplayNamePinyin
                },
                CreatedAt = channel.CreatedAt
            };
            if (channel.Visibility == Enums.Visibility.Public)
            {
                session.Initials = "\uE64E";
                session.DefaultIcon = "\uE64E";
                session.AvatarFont = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
            }
            else
            {
                session.Initials = "\uE748";
                session.DefaultIcon = "\uE748";
                session.AvatarFont = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
            }
            return session;
        }

        public static Models.Message.Session GetSession(ApiModels.ApiTeamChats.Session session, AvatarSize size)
        {
            return new Models.Message.Session
            {
                Id = session.Id,
                DisplayName = session.To.DisplayName,
                Initials = AvatarHelper.GetInitials(session.To.DisplayName),
                ProfilePicture = AvatarHelper.GetAvatarBitmap(session.To.Avatar, size, FromType.User),
                Background = AvatarHelper.GetColorBrush(session.To.DisplayName),
                Starred = session.Starred,
                LatestMessageAt = session.LatestMessageAt,
                Show = session.Show,
                UnRead = session.UnRead,
                //NamePinyin = item.To.DisplayName,
                Component = session.Component,
                Name = session.To.Name,
                Type = SessionType.Session,
                IsBot = session.IsBot
            };
        }
    }
}
