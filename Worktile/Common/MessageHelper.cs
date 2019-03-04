﻿using System.IO;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Worktile.Enums;
using Worktile.Models;
using Worktile.Models.Message;

namespace Worktile.Common
{
    public static class MessageHelper
    {
        public static void SetAvatar(Message message)
        {
            message.From.TethysAvatar = new TethysAvatar
            {
                DisplayName = message.From.DisplayName,
                Source = AvatarHelper.GetAvatarBitmap(message.From.Avatar, AvatarSize.X80, message.From.Type)
            };
            if (message.From.Avatar != "default.png" && Path.GetExtension(message.From.Avatar).ToLower() == ".png")
                message.From.TethysAvatar.Background = new SolidColorBrush(Colors.White);
            else
                message.From.TethysAvatar.Background = AvatarHelper.GetColorBrush(message.From.DisplayName);
        }
    }
}
