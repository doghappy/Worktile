using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.Message;
using Worktile.Models;
using Worktile.Models.Exceptions;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Main
{
    public sealed partial class LightMainPage : Page, INotifyPropertyChanged
    {
        public LightMainPage()
        {
            InitializeComponent();
            Apps = new ObservableCollection<WtApp>
            {
                new WtApp
                {
                    Name = "message",
                    DisplayName = UtilityTool.GetStringFromResources("WtAppMessageDisplayName"),
                    Icon = WtIconHelper.GetAppIcon("message")
                }
            };
            Members = new ObservableCollection<User>();
            Sessions = new ObservableCollection<Session>();
            Services = new ObservableCollection<WtService>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<WtApp> Apps { get; }
        public ObservableCollection<User> Members { get; }
        public ObservableCollection<Session> Sessions { get; }
        public ObservableCollection<WtService> Services { get; }

        private bool _canGoBack;
        public bool CanGoBack
        {
            get => _canGoBack;
            set
            {
                if (_canGoBack != value)
                {
                    _canGoBack = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CanGoBack)));
                }
            }
        }

        private string _wtBackgroundImage;
        public string WtBackgroundImage
        {
            get => _wtBackgroundImage;
            set
            {
                if (_wtBackgroundImage != value)
                {
                    _wtBackgroundImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WtBackgroundImage)));
                }
            }
        }

        private User _me;
        public User Me
        {
            get => _me;
            set
            {
                if (_me != value)
                {
                    _me = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Me)));
                }
            }
        }

        private Team _team;
        public Team Team
        {
            get => _team;
            set
            {
                if (_team != value)
                {
                    _team = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Team)));
                }
            }
        }

        private string _logo;
        public string Logo
        {
            get => _logo;
            set
            {
                if (_logo != value)
                {
                    _logo = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Logo)));
                }
            }
        }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get => _selectedApp;
            set
            {
                if (_selectedApp != value)
                {
                    _selectedApp = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedApp)));
                    switch (value.Name)
                    {
                        case "message":
                            ContentFrame.Navigate(typeof(MessagePage));
                            break;
                    }
                }
            }
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void GoBack_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string domain = ApplicationData.Current.LocalSettings.Values["Domain"]?.ToString();
            if (string.IsNullOrEmpty(domain))
            {
                Frame.Navigate(typeof(SignInPage));
            }
            else
            {
                WtHttpClient.Domain = domain;
                await RequestMeAsync();
                await RequestChatsAsync();
                SelectedApp = Apps.First();
                Parallel.Invoke(
                    async () => await RequestTeamAsync()
                );
                //try
                //{
                //    await ViewModel.RequestMeAsync();
                //    await ViewModel.RequestTeamAsync();
                //    await ViewModel.RequestChatsAsync();
                //}
                //catch (WtNotLoggedInException)
                //{
                //    ApplicationData.Current.LocalSettings.Values.Remove("Domain");
                //    UtilityTool.RootFrame.Navigate(typeof(SignInPage));
                //}
            }
        }

        #region Request API
        private async Task RequestMeAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/user/me");
            var data = obj["data"] as JObject;
            if (data.ContainsKey("me"))
            {
                //await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                //{
                SharedData.Box = data["config"]["box"].ToObject<StorageBox>();
                Me = data["me"].ToObject<User>();
                string imToken = data["me"].Value<string>("imToken");
                string imHost = data["config"]["feed"].Value<string>("newHost");
                string img = data["me"]["preferences"].Value<string>("background_image");
                if (img.StartsWith("desktop-") && img.EndsWith(".jpg"))
                {
                    WtBackgroundImage = "ms-appx:///Assets/Images/Background/" + img;
                }
                else
                {
                    WtBackgroundImage = SharedData.Box.BaseUrl + "background-image/" + img + "/from-s3";
                }
                //Window.Current.Activated += Window_Activated;
                //WtSocketClient.OnMessageReceived += WtSocketClient_OnMessageReceived;
                //await WtSocketClient.ConnectAsync(imHost, imToken, Me.Id);
                //}));
            }
            else
            {
                throw new WtNotLoggedInException();
            }
        }

        private async Task RequestTeamAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/team");
            Team = obj["data"].ToObject<Team>();
            Logo = SharedData.Box.LogoUrl + Team.Logo;
            var members = obj["data"]["members"].Children<JObject>();
            foreach (var item in members)
            {
                Members.Add(item.ToObject<User>());
            }
            var services = obj["data"]["services"].Children<JObject>();
            foreach (var item in services)
            {
                Services.Add(item.ToObject<WtService>());
            }
        }

        private async Task RequestChatsAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/team/chats");

            var channels = obj["data"]["channels"].Children<JObject>();
            var groups = obj["data"]["groups"].Children<JObject>();
            var allChannels = channels.Concat(groups);
            foreach (var item in allChannels)
            {
                var session = GetChannelSession(item);
                int index = GetNewIndex(session);
                Sessions.Insert(index, session);
            }

            var sessions = obj["data"]["sessions"].Children<JObject>();
            foreach (var item in sessions)
            {
                var session = GetMemberSession(item);
                int index = GetNewIndex(session);
                Sessions.Insert(index, session);
            }
            //UnreadMessageCount = Sessions.Sum(s => s.UnRead);
        }
        #endregion

        private async void WtSocketClient_OnMessageReceived(JObject obj)
        {
            //var msg = obj.ToObject<WtMessage.Message>();
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
            //else if (SharedData.SelectedSession != session)
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
            //SendToast(msg);
        }

        private Session GetChannelSession(JObject obj)
        {
            return new Session
            {
                Id = obj.Value<string>("_id"),
                DisplayName = obj.Value<string>("name"),
                UnRead = obj.Value<int>("unread"),
                IsStar = obj.Value<bool>("starred"),
                LatestMessageAt = DateTimeOffset.FromUnixTimeSeconds(obj.Value<long>("latest_message_at")),
                LatestMessageId = obj.Value<string>("latest_message_id"),
                Type = SessionType.Channel,
                RefType = 1,
                Visibility = (WtVisibility)obj.Value<int>("visibility"),
                Color = WtColorHelper.GetNewBrush(obj.Value<string>("color"))
            };
        }

        private Session GetMemberSession(JObject obj)
        {
            string displayName = obj["to"].Value<string>("display_name");
            return new Session
            {
                Id = obj.Value<string>("_id"),
                Avatar = obj["to"].Value<string>("avatar"),
                DisplayName = displayName,
                UnRead = obj.Value<int>("unread"),
                IsStar = obj.Value<bool>("starred"),
                LatestMessageAt = DateTimeOffset.FromUnixTimeSeconds(obj.Value<long>("latest_message_at")),
                IsAAssistant = obj["to"].Value<int>("role") == (int)UserRoleType.Bot,
                Type = SessionType.Session,
                RefType = 2,
                Color = WtColorHelper.GetAvatarBackground(displayName)
            };
        }

        private int GetNewIndex(Session session)
        {
            if (Sessions.Count == 0)
            {
                return 0;
            }
            for (int i = 0; i < Sessions.Count; i++)
            {
                if (session.IsStar)
                {
                    if (Sessions[i].IsStar)
                    {
                        if (session.LatestMessageAt >= Sessions[i].LatestMessageAt)
                        {
                            return i;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        return i;
                    }
                }
                else
                {
                    if (Sessions[i].IsStar)
                    {
                        continue;
                    }
                    else
                    {
                        if (session.LatestMessageAt >= Sessions[i].LatestMessageAt)
                        {
                            return i;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return Sessions.Count - 1;
        }
    }
}
