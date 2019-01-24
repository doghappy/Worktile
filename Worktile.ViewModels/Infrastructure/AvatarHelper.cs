using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Worktile.Models;

namespace Worktile.ViewModels.Infrastructure
{
    public static class AvatarHelper
    {
        readonly static string[] _colors = new[]
        {
            "#2cccda", "#2dbcff", "#4e8af9", "#7076fa", "#9473fd", "#ef7ede", "#99d75a", "#66c060", "#39ba5d"
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

        public static Color GetColor(string displayName)
        {
            int code = displayName.Sum(n => n);
            string hex = _colors[code % 9];
            return WtColorHelper.GetColor(WtColorHelper.GetNewColor(hex));
        }

        public static BitmapImage GetAvatarBitmap(string avatar, int size)
        {
            string url = GetAvatarUrl(avatar, size);
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }
            else
            {
                return new BitmapImage(new Uri(url));
            }
        }

        public static string GetAvatarUrl(string avatar, int size)
        {
            if (string.IsNullOrWhiteSpace(avatar) || avatar == "default.png")
            {
                return null;
            }
            else
            {
                string ext = Path.GetExtension(avatar);
                string name = Path.GetFileNameWithoutExtension(avatar);
                return DataSource.ApiUserMeConfig.Box.AvatarUrl + name + "_" + size + "x" + size + ext;
            }
        }

        public static Avatar GetAvatar(string uid, int avatarSize)
        {
            var member = DataSource.Team.Members.FirstOrDefault(m => m.Uid == uid);
            if (member == null)
            {
                return null;
            }
            return new Avatar
            {
                ProfilePicture = GetAvatarBitmap(member.Avatar, avatarSize),
                DisplayName = member.DisplayName,
                Initials = GetInitials(member.DisplayName)
            };
        }
    }
}
