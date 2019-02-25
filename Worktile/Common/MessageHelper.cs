using Windows.UI.Xaml.Media;
using Worktile.ApiModels.ApiTeamChats;
using Worktile.Views.Message;

namespace Worktile.Common
{
    public static class MessageHelper
    {
        public static Views.Message.Session GetSession(Channel channel)
        {
            var session = new Views.Message.Session
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

        public static string GetContent(Models.Message.Message msg)
        {
            switch (msg.Type)
            {
                case MessageType.Attachment:
                case MessageType.Calendar:
                    return msg.Body.InlineAttachment.Text;
                default: return msg.Body.Content;
            }
        }
    }
}
