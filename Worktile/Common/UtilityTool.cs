using System;
using System.IO;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Main;
using Worktile.Message.Models;
using Worktile.Modles;

namespace Worktile.Common
{
    public static class UtilityTool
    {
        public static string GetStringFromResources(string key)
        {
            var srcLoader = ResourceLoader.GetForViewIndependentUse();
            return srcLoader.GetString(key);
        }

        public static void ReloadPageTheme(ElementTheme startTheme, LightMainPage mainPage)
        {
            if (mainPage.RequestedTheme == ElementTheme.Dark)
                mainPage.RequestedTheme = ElementTheme.Light;
            else if (mainPage.RequestedTheme == ElementTheme.Light)
                mainPage.RequestedTheme = ElementTheme.Default;
            else if (mainPage.RequestedTheme == ElementTheme.Default)
                mainPage.RequestedTheme = ElementTheme.Dark;

            if (mainPage.RequestedTheme != startTheme)
                ReloadPageTheme(startTheme, mainPage);
        }

        public static void ReloadMainPage()
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                rootFrame.Navigate(typeof(LightMainPage));
            }
        }

        public static Frame RootFrame => Window.Current.Content as Frame;

        public static string GetS3FileUrl(string id)
        {
            return $"{SharedData.Box.BaseUrl}/entities/{id}/from-s3?team_id={MainViewModel.TeamId}";
        }

        public static string GetIdFromS3FileUrl(string uri)
        {
            return GetIdFromS3FileUrl(new Uri(uri));
        }

        public static string GetIdFromS3FileUrl(Uri uri)
        {
            string segment = uri.Segments[2];
            return segment.Substring(0, segment.Length - 1);
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
                            string sizeStr = ((int)size).ToString();
                            return SharedData.Box.AvatarUrl + name + "_" + sizeStr + "x" + sizeStr + ext;
                        }
                    case FromType.Service:
                    case FromType.Addition:
                        return SharedData.Box.ServiceUrl + avatar;
                    default: throw new NotImplementedException();
                }
            }
        }
    }
}
