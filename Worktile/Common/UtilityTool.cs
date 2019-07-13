using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Worktile.Main;

namespace Worktile.Common
{
    public static class UtilityTool
    {
        public static string GetStringFromResources(string key)
        {
            var srcLoader = ResourceLoader.GetForViewIndependentUse();
            return srcLoader.GetString(key);
        }

        public static void ReloadPageTheme(ElementTheme startTheme, MainPage mainPage)
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
                rootFrame.Navigate(typeof(MainPage));
            }
        }

        public static Frame RootFrame => Window.Current.Content as Frame;

        public static MainPage MainPage => RootFrame.Content as MainPage;

        public static string GetS3FileUrl(string id)
        {
            return $"{MainViewModel.Box.BaseUrl}/entities/{id}/from-s3?team_id={MainViewModel.TeamId}";
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
    }
}
