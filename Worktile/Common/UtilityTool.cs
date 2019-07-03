﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var srcLoader = ResourceLoader.GetForCurrentView();
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

        public static MainPage MainPage
        {
            get
            {
                if (Window.Current.Content is Frame rootFrame)
                {
                    return rootFrame.Content as MainPage;
                }
                return null;
            }
        }
    }
}
