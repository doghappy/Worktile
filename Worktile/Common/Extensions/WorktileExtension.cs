using System.Text.RegularExpressions;
using Worktile.Enums;
using Worktile.Enums.Message;
using Worktile.Models;
using Worktile.Models.Department;
using Worktile.Models.Entity;
using Worktile.Models.Member;
using Worktile.Models.Message.Session;
using EnumsNET;
using Windows.UI.Xaml.Media.Imaging;
using System;

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

        public static void ForShowAvatar(this Member member, AvatarSize size)
        {
            member.TethysAvatar = AvatarHelper.GetAvatar(member, size);
        }

        public static void ForShowAvatar(this DepartmentNode node)
        {
            if (node.Type == DepartmentNodeType.Member)
            {
                node.Avatar = new TethysAvatar
                {
                    DisplayName = node.Addition.DisplayName,
                    Background = AvatarHelper.GetColorBrush(node.Addition.DisplayName),
                    Source = AvatarHelper.GetAvatarBitmap(node.Addition.Avatar, AvatarSize.X40, FromType.User)
                };
            }
            else if (node.Type == DepartmentNodeType.Department)
            {
                foreach (var item in node.Children)
                {
                    ForShowAvatar(item);
                }
            }
        }

        public static void Resize(this TethysAvatar avatar, AvatarSize size)
        {
            if (avatar.Source != null)
            {
                string uri = avatar.Source.UriSource.ToString();
                string newUri = Regex.Replace(uri, @".+_(\d{2,3})x\d{2,3}\.(png|jpg)$", match => uri.Replace(match.Groups[1].Value, ((int)size).ToString()));
                avatar.Source = new BitmapImage(new Uri(newUri));
            }
        }

        public static bool IsTrueMember(this Member member) => member.Role != RoleType.Bot && !string.IsNullOrEmpty(member.Team);

        public static void ForShowEntity(this Entity entity)
        {
            entity.Avatar = new TethysAvatar
            {
                DisplayName = entity.CreatedBy.DisplayName,
                Background = AvatarHelper.GetColorBrush(entity.CreatedBy.DisplayName),
                Source = AvatarHelper.GetAvatarBitmap(entity.CreatedBy.Avatar, AvatarSize.X40, FromType.User)
            };
            entity.Icon = WtFileHelper.GetFileIcon(entity.Addition.Ext);
            entity.IsEnableDelete = entity.CreatedBy.Uid == DataSource.ApiUserMeData.Me.Uid;
            entity.IsEnableDownload = entity.Addition.Path != null && !(entity.Addition.Path.StartsWith("http://") || entity.Addition.Path.StartsWith("https://")) && (entity.Type == MessageType.File || entity.Type == MessageType.Image);
        }
    }
}
