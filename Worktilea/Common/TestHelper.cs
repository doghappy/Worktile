﻿using System;
using Windows.Storage;
using Worktile.Common.Communication;

namespace Worktile.Common
{
    public static class TestHelper
    {
        public static void SignIn()
        {
            string cookie = ApplicationData.Current.LocalSettings.Values["AuthCookie"]?.ToString();
            if (string.IsNullOrEmpty(cookie))
            {
                throw new ArgumentNullException("cookie");
            }
            else
            {
                //WtHttpClient.SetBaseAddress(DataSource.SubDomain);
                //WtHttpClient.AddDefaultRequestHeaders("Cookie", cookie);
            }
        }
    }
}
