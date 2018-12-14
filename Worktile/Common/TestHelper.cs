using System;
using Windows.Storage;
using Worktile.Views;
using Worktile.WtRequestClient;

namespace Worktile.Common
{
    public static class TestHelper
    {
        public static void SignIn()
        {
            string cookie = ApplicationData.Current.LocalSettings.Values[SignInPage.AuthCookie]?.ToString();
            if (string.IsNullOrEmpty(cookie))
            {
                throw new ArgumentNullException("cookie");
            }
            else
            {
                WtHttpClient.SetBaseAddress(CommonData.SubDomain);
                WtHttpClient.AddDefaultRequestHeaders("Cookie", cookie);
            }
        }
    }
}
