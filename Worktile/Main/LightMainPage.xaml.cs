using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.Main.Profile;
using Worktile.Message;
using Worktile.Message.Models;
using Worktile.Models;
using Worktile.Models.Exceptions;
using Worktile.Modles;
using WtMessage = Worktile.Message.Models;
using Windows.UI.Xaml.Navigation;

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

        #region Fields
        private string _imHost;
        private string _imToken;
        #endregion

        #region Properties
        public CoreWindowActivationState WindowActivationState { get; private set; }

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
                }
            }
        }

        private int _unreadMessageCount;
        public int UnreadMessageCount
        {
            get => _unreadMessageCount;
            set
            {
                if (_unreadMessageCount != value || value == 0)
                {
                    _unreadMessageCount = value;
                    Apps[0].UnreadCount = value;
                    UpdateBadgeNumber(value);
                }
            }
        }
        #endregion

        #region Event Methods
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

                Window.Current.Activated += Window_Activated;
                WtSocketClient.OnMessageReceived += WtSocketClient_OnMessageReceived;
                await WtSocketClient.ConnectAsync(_imHost, _imToken, Me.Id);
                Parallel.Invoke(
                    async () => await RequestTeamAsync()//,
                    //async () => await WtSocketClient.ConnectAsync(_imHost, _imToken, Me.Id)
                );
            }
        }
        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            WindowActivationState = e.WindowActivationState;
        }

        private void AppsNav_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (SelectedApp.Name)
            {
                case "message":
                    ContentFrame.Navigate(typeof(MessagePage));
                    break;
            }
        }

        private void GoBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentFrame.GoBack();
        }

        private void SettingsMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            AppsNav.SelectionChanged -= AppsNav_SelectionChanged;
            SelectedApp = null;
            ContentFrame.Navigate(typeof(SettingPage));
            AppsNav.SelectionChanged += AppsNav_SelectionChanged;
        }

        private void ProfileMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            AppsNav.SelectionChanged -= AppsNav_SelectionChanged;
            SelectedApp = null;
            ContentFrame.Navigate(typeof(AccountInfoPage));
            AppsNav.SelectionChanged += AppsNav_SelectionChanged;
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            var frame = sender as Frame;
            CanGoBack = frame.CanGoBack;
        }
        #endregion

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
                SharedData.Me = Me;
                _imToken = data["me"].Value<string>("imToken");
                _imHost = data["config"]["feed"].Value<string>("newHost");
                string img = data["me"]["preferences"].Value<string>("background_image");
                if (img.StartsWith("desktop-") && img.EndsWith(".jpg"))
                {
                    WtBackgroundImage = "ms-appx:///Assets/Images/Background/" + img;
                }
                else
                {
                    WtBackgroundImage = SharedData.Box.BaseUrl + "background-image/" + img + "/from-s3";
                }
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
            UnreadMessageCount = Sessions.Sum(s => s.UnRead);
        }
        #endregion

        #region Socket Messages
        private async void WtSocketClient_OnMessageReceived(JObject obj)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                var msg = obj.ToObject<WtMessage.Message>();
                var session = Sessions.FirstOrDefault(s => s.Id == msg.To.Id);
                if (session == null)
                {
                    string url = $"api/{msg.To.Type.ToString().ToLower()}s/{msg.To.Id}";
                    var sessionObj = await WtHttpClient.GetAsync(url);
                    Session newSession = null;
                    if (msg.To.Type == WtMessage.ToType.Channel)
                        newSession = GetChannelSession(sessionObj);
                    else
                        newSession = GetMemberSession(sessionObj);
                    newSession.UnRead++;
                    newSession.LatestMessageAt = msg.CreatedAt;
                    int index = GetNewIndex(newSession);
                    //await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => Sessions.Insert(index, newSession)));
                    Sessions.Insert(index, newSession);
                }
                else// if (SharedData.SelectedSession != session)
                {
                    var messagePage = ContentFrame.Content as MessagePage;
                    if (messagePage != null && messagePage.SelectedSession != session)
                    {
                        int index = Sessions.IndexOf(session);
                        session.LatestMessageAt = msg.CreatedAt;
                        session.UnRead++;
                        UnreadMessageCount++;
                        int newindex = GetNewIndex(session);
                        if (index != newindex)
                        {
                            Sessions.Move(index, newindex);
                            messagePage.Highlight(session);
                        }
                    }
                }
                SendToast(msg);
            });
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

        private void SendToast(WtMessage.Message msg)
        {
            if (msg.From.Uid == Me.Id)
            {
                return;
            }
            else
            {
                if (ContentFrame.Content is MessagePage messagePage
                    && messagePage.SelectedSession != null
                    && messagePage.SelectedSession.Id == msg.To.Id
                    && WindowActivationState == CoreWindowActivationState.Deactivated)
                {
                    return;
                }
            }
            var member = Members.Single(m => m.Id == msg.From.Uid);
            string avatar = "ms-appx:///Assets/StoreLogo.scale-400.png";
            if (member.Avatar != string.Empty)
            {
                avatar = UtilityTool.GetAvatarUrl(member.Avatar, AvatarSize.X80, FromType.User);
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

        private void UpdateBadgeNumber(int count)
        {
            if (count > 0)
            {
                XmlDocument badgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
                XmlElement badgeElement = badgeXml.SelectSingleNode("/badge") as XmlElement;
                badgeElement.SetAttribute("value", count.ToString());
                BadgeNotification badge = new BadgeNotification(badgeXml);
                BadgeUpdater badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
                badgeUpdater.Update(badge);
            }
            else
            {
                BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
            }
        }
        #endregion
    }
}
