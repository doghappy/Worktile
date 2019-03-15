using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Member;
using Worktile.Models.Message.Session;

namespace Worktile.Common
{
    public static class AvatarHelper
    {
        readonly static SolidColorBrush[] _brushes = new[]
        {
            //"#2cccda", "#2dbcff", "#4e8af9", "#7076fa", "#9473fd", "#ef7ede", "#99d75a", "#66c060", "#39ba5d"
            new SolidColorBrush(Color.FromArgb(255, 44, 204, 218)),
            new SolidColorBrush(Color.FromArgb(255, 45, 188, 255)),
            new SolidColorBrush(Color.FromArgb(255, 78, 138, 249)),
            new SolidColorBrush(Color.FromArgb(255, 112, 118, 250)),
            new SolidColorBrush(Color.FromArgb(255, 148, 115, 253)),
            new SolidColorBrush(Color.FromArgb(255, 239, 126, 222)),
            new SolidColorBrush(Color.FromArgb(255, 153, 215, 90)),
            new SolidColorBrush(Color.FromArgb(255, 102, 192, 96)),
            new SolidColorBrush(Color.FromArgb(255, 57, 186, 93))
        };

        public static string GetInitials(string displayName)
        {
            if (Regex.IsMatch(displayName, @"^[\u4e00-\u9fa5]+$") && displayName.Length > 2)
            {
                return displayName.Substring(displayName.Length - 2, 2);
            }
            else if (Regex.IsMatch(displayName, @"^[a-zA-Z\/]+$") && displayName.IndexOf(' ') > 0)
            {
                var arr = displayName.Split(' ');
                return (arr[0].First().ToString() + arr[1].First()).ToUpper();
            }
            else if (displayName.Length > 2)
            {
                return displayName.Substring(0, 2).ToUpper();
            }
            else
            {
                return displayName.ToUpper();
            }
        }

        //public static Color GetColor(string displayName)
        //{
        //    int code = displayName.Sum(n => n);
        //    string hex = _colors[code % 9];
        //    return WtColorHelper.GetColor(WtColorHelper.GetNewColor(hex));
        //}
        public static SolidColorBrush GetColorBrush(string displayName)
        {
            int code = displayName.Sum(n => n);
            return _brushes[code % _brushes.Length];
        }

        public static BitmapImage GetAvatarBitmap(string avatar, AvatarSize size, FromType fromType)
        {
            string url = GetAvatarUrl(avatar, size, fromType);
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }
            else
            {
                return new BitmapImage(new Uri(url));
            }
        }

        public static string GetAvatarUrl(string avatar, AvatarSize size, FromType fromType)
        {
            if (string.IsNullOrWhiteSpace(avatar) || avatar == "default.png")
            {
                return null;
            }
            else
            {
                switch (fromType)
                {
                    case FromType.User:
                        {
                            string ext = Path.GetExtension(avatar);
                            string name = Path.GetFileNameWithoutExtension(avatar);
                            return DataSource.ApiUserMeData.Config.Box.AvatarUrl + name + "_" + (int)size + "x" + (int)size + ext;
                        }
                    case FromType.Service:
                    case FromType.Addition:
                        return DataSource.ApiUserMeData.Config.Box.ServiceUrl + avatar;
                    default: throw new NotImplementedException();
                }
            }
        }

        public static Avatar GetAvatar(string uid, AvatarSize size)
        {
            var member = DataSource.Team.Members.FirstOrDefault(m => m.Uid == uid);
            if (member == null)
            {
                return null;
            }
            return new Avatar
            {
                ProfilePicture = GetAvatarBitmap(member.Avatar, size, FromType.User),
                DisplayName = member.DisplayName,
                Initials = GetInitials(member.DisplayName)
            };
        }

        public static TethysAvatar GetAvatar(Member member, AvatarSize size)
        {
            return new TethysAvatar
            {
                Id = member.Uid,
                DisplayName = member.DisplayName,
                Background = GetColorBrush(member.DisplayName),
                Source = GetAvatarBitmap(member.Avatar, size, FromType.User),
                Name = member.Name,
                DisplayNamePinyin = member.DisplayNamePinyin
            };
        }

        public static TethysAvatar GetAvatar(ChannelSession session)
        {
            var avatar = new TethysAvatar
            {
                Id = session.Id,
                DisplayName = session.Name,
                Background = WtColorHelper.GetNewBrush(session.Color),
                DisplayNamePinyin = session.NamePinyin
            };
            if (session.Visibility == WtVisibility.Public)
            {
                avatar.Icon = "\uE64E";
                avatar.AvatarFont = new FontFamily("ms-appx:///Worktile,,,/Assets/Fonts/lc-iconfont.ttf#lcfont");
            }
            else
            {
                avatar.Icon = "\uE748";
                avatar.AvatarFont = new FontFamily("ms-appx:///Worktile.Tethys/Assets/Fonts/iconfont.ttf#wtf");
            }
            return avatar;
        }

        public static TethysAvatar GetAvatar(MemberSession session, AvatarSize size)
        {
            return new TethysAvatar
            {
                Id = session.Id,
                DisplayName = session.To.DisplayName,
                Background = AvatarHelper.GetColorBrush(session.To.DisplayName),
                DisplayNamePinyin = session.To.DisplayNamePinyin,
                Name = session.To.Name,
                Source = GetAvatarBitmap(session.To.Avatar, size, FromType.User)
            };
        }
    }
}
