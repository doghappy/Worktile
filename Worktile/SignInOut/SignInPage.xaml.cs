using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;

namespace Worktile.SignInOut
{
    public sealed partial class SignInPage : Page
    {
        public SignInPage()
        {
            InitializeComponent();
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string url = args.Uri.ToString();
            if (url.Contains("?"))
            {
                url = url.Substring(0, url.IndexOf("?"));
            }
            string[] allowList = new[]
            {
                "https://worktile.com/signin",
                "https://worktile.com/signup"
            };
            if (!allowList.Contains(url))
            {
                var regex = new Regex(@"^https://(\w+)\.worktile\.com/$");
                if (regex.IsMatch(url))
                {
                    string domain = regex.Match(url).Groups[1].Value;
                    ApplicationData.Current.LocalSettings.Values["Domain"] = domain;
                    WtHttpClient.Domain = domain;
                }
                UtilityTool.ReloadMainPage();
            }
        }
    }
}
