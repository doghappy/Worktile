using System;
using Worktile.WebUI.Modles;
using Worktile.WebUI.Models.Message;
using System.IO;

namespace Worktile.WebUI.Common
{
    public static class AvatarHelper
    {
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
                            string sizeStr = ((int)size).ToString();
                            return MainPage.Box.AvatarUrl + name + "_" + sizeStr + "x" + sizeStr + ext;
                        }
                    case FromType.Service:
                    case FromType.Addition:
                        return MainPage.Box.ServiceUrl + avatar;
                    default: throw new NotImplementedException();
                }
            }
        }
    }
}
