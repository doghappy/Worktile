using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.WebUI.Common;
using Worktile.WebUI.Models;
using Worktile.WebUI.Models.Message;
using Worktile.WebUI.Modles;
using WtMessage = Worktile.WebUI.Models.Message;

namespace Worktile.WebUI
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public static string Domain { get; private set; }

        public static string IMToken { get; private set; }

        public static string IMHost { get; private set; }

        public static StorageBox Box { get; private set; }

        public static Member Me { get; private set; }

        private List<Member> Members { get; set; }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string domain = ApplicationData.Current.LocalSettings.Values["Domain"]?.ToString();
            if (string.IsNullOrEmpty(domain))
            {
                RootWebView.Navigate(new Uri("https://worktile.com/signin"));
            }
            else
            {
                Domain = domain;
                RootWebView.Navigate(new Uri($"https://{domain}.worktile.com"));
                await ConnectSocketAsync();
            }
        }

        private async void RootWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string url = args.Uri.ToString();
            if (url.Contains("?"))
            {
                url = url.Substring(0, url.IndexOf("?"));
            }
            url = url.TrimEnd('/');

            if (Domain == null)
            {
                var regex = new Regex(@"^https://([a-zA-Z]+\w+)\.worktile\.com$");
                if (regex.IsMatch(url))
                {
                    string domain = regex.Match(url).Groups[1].Value;
                    ApplicationData.Current.LocalSettings.Values["Domain"] = domain;
                    Domain = domain;
                    await ConnectSocketAsync();
                }
            }
            else
            {
                if (url == $"https://{Domain}.worktile.com/signout")
                {
                    ApplicationData.Current.LocalSettings.Values.Remove("Domain");
                    Domain = null;
                    RootWebView.Navigate(new Uri("https://worktile.com/signin"));
                }
            }
        }

        private async Task ConnectSocketAsync()
        {
            await RequestMeAsync();
            await RequestTeamAsync();

            WtSocketClient.OnMessageReceived += WtSocketClient_OnMessageReceived;
            WtSocketClient.Connect(IMHost, IMToken);
        }

        private async Task RequestMeAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/user/me");
            var data = obj["data"] as JObject;
            if (data.ContainsKey("me"))
            {
                Me = data["me"].ToObject<Member>();
                Box = data["config"]["box"].ToObject<StorageBox>();
                IMToken = data["me"].Value<string>("imToken");
                IMHost = data["config"]["feed"].Value<string>("newHost");

                //Window.Current.Activated += Window_Activated;
            }
            else
            {
                throw new Exception();
            }
        }

        private async Task RequestTeamAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/team");
            Members = new List<Member>();
            var members = obj["data"]["members"].Children<JObject>();
            foreach (var item in members)
            {
                Members.Add(item.ToObject<Member>());
            }
        }

        private void WtSocketClient_OnMessageReceived(JObject obj)
        {
            var msg = obj.ToObject<WtMessage.Message>();
            //var session = Sessions.FirstOrDefault(s => s.Id == msg.To.Id);
            //if (session == null)
            //{
            //    string url = $"api/{msg.To.Type.ToString().ToLower()}s/{msg.To.Id}";
            //    var sessionObj = await WtHttpClient.GetAsync(url);
            //    Session newSession = null;
            //    if (msg.To.Type == WtMessage.ToType.Channel)
            //        newSession = GetChannelSession(sessionObj);
            //    else
            //        newSession = GetMemberSession(sessionObj);
            //    newSession.UnRead++;
            //    newSession.LatestMessageAt = msg.CreatedAt;
            //    int index = GetNewIndex(newSession);
            //    await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => Sessions.Insert(index, newSession)));
            //}
            //else if (MessageViewModel.Session != session)
            //{
            //    int index = Sessions.IndexOf(session);
            //    await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            //    {
            //        session.LatestMessageAt = msg.CreatedAt;
            //        session.UnRead++;
            //        UnreadMessageCount++;
            //        int newindex = GetNewIndex(session);
            //        if (index != newindex)
            //        {
            //            Sessions.Move(index, newindex);
            //            MessageViewModel.Highlight(session);
            //        }
            //    }));
            //}
            SendToast(msg);
        }

        private void SendToast(WtMessage.Message msg)
        {
            if (msg.From.Uid == Me.Id)
            {
                return;
            }
            //else if (MessageViewModel.Session != null && MessageViewModel.Session.Id == msg.To.Id && WindowActivationState == CoreWindowActivationState.Deactivated)
            //{
            //    return;
            //}
            var member = Members.Single(m => m.Id == msg.From.Uid);
            string avatar = "ms-appx:///Assets/StoreLogo.scale-400.png";
            if (member.Avatar != string.Empty)
            {
                avatar = AvatarHelper.GetAvatarUrl(member.Avatar, AvatarSize.X80, FromType.User);
            }

            var toastContent = new ToastContent()
            {
                DisplayTimestamp = msg.CreatedAt,
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = member.DisplayName,
                                HintMaxLines = 1
                            },
                            new AdaptiveText()
                            {
                                //Text = MessageContentReader.ReadSummary(apiMsg)
                                Text = msg.Body.Content
                            }
                        },
                        AppLogoOverride = new ToastGenericAppLogo()
                        {
                            Source = avatar,
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                }
            };

            if (msg.Body.At != null && msg.Body.At.Count == 1)
            {
                string quicklyReply = UtilityTool.GetStringFromResources("PleaseEnterMessageToReplyQuickly");
                toastContent.Actions = new ToastActionsCustom()
                {
                    Inputs =
                    {
                        new ToastTextBox("msg")
                        {
                            PlaceholderContent = quicklyReply
                        }
                    },
                    Buttons =
                    {
                        new ToastButton("Send", $"action=reply&toType={msg.To.Type}&to={msg.To.Id}")
                        {
                            ActivationType = ToastActivationType.Foreground,
                            ImageUri = "Assets/Images/Icons/send.png",
                            TextBoxId = "msg"
                        }
                    }
                };
                toastContent.Launch = "action=launch";
            }
            var toastNotif = new ToastNotification(toastContent.GetXml());
            ToastNotificationManager.CreateToastNotifier().Show(toastNotif);
        }
    }
}
