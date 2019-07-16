using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.Message;
using Worktile.Message.Models;
using Worktile.Models;
using Worktile.Models.Exceptions;
using Worktile.Modles;
using WtMessage = Worktile.Message.Models;

namespace Worktile.Main
{
    public class MainViewModel : BindableBase
    {
        public MainViewModel()
        {
            Apps = new ObservableCollection<WtApp>
            {
                new WtApp
                {
                    Name = "message",
                    DisplayName = UtilityTool.GetStringFromResources("WtAppMessageDisplayName"),
                    Icon = WtIconHelper.GetAppIcon("message")
                }
            };
            SelectedApp = Apps.First();
            Members = new ObservableCollection<User>();
            Sessions = new ObservableCollection<Session>();
            Services = new ObservableCollection<WtService>();
        }

        public ObservableCollection<WtApp> Apps { get; }

        public static StorageBox Box { get; private set; }

        public static string TeamId { get; private set; }

        public static User Me { get; private set; }

        public static ObservableCollection<Session> Sessions { get; private set; }

        public static ObservableCollection<User> Members { get; private set; }

        public static ObservableCollection<WtService> Services { get; private set; }

        public static MessageViewModel MessageViewModel { get; set; }

        private static int _unreadMessageCount;
        public static int UnreadMessageCount
        {
            get => _unreadMessageCount;
            set
            {
                if (_unreadMessageCount != value || value == 0)
                {
                    _unreadMessageCount = value;
                    UpdateBadgeNumber(value);
                }
            }
        }

        public CoreWindowActivationState WindowActivationState { get; private set; }

        private Team _team;
        public Team Team
        {
            get => _team;
            set => SetProperty(ref _team, value);
        }

        private WtApp _selectedApp;
        public WtApp SelectedApp
        {
            get => _selectedApp;
            set => SetProperty(ref _selectedApp, value);
        }

        private User _user;
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private string _wtBackgroundImage;
        public string WtBackgroundImage
        {
            get => _wtBackgroundImage;
            set => SetProperty(ref _wtBackgroundImage, value);
        }

        private string _logo;
        public string Logo
        {
            get => _logo;
            set => SetProperty(ref _logo, value);
        }

        public async Task RequestMeAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/user/me");
            var data = obj["data"] as JObject;
            if (data.ContainsKey("me"))
            {
                User = data["me"].ToObject<User>();
                Me = User;
                Box = data["config"]["box"].ToObject<StorageBox>();
                string imToken = data["me"].Value<string>("imToken");
                string imHost = data["config"]["feed"].Value<string>("newHost");
                string img = data["me"]["preferences"].Value<string>("background_image");
                if (img.StartsWith("desktop-") && img.EndsWith(".jpg"))
                {
                    WtBackgroundImage = "ms-appx:///Assets/Images/Background/" + img;
                }
                else
                {
                    WtBackgroundImage = Box.BaseUrl + "background-image/" + img + "/from-s3";
                }

                Window.Current.Activated += Window_Activated;
                WtSocketClient.OnMessageReceived += WtSocketClient_OnMessageReceived;
                WtSocketClient.Connect(imHost, imToken, User.Id);
            }
            else
            {
                throw new WtNotLoggedInException();
            }
        }

        private void Window_Activated(object sender, WindowActivatedEventArgs e)
        {
            WindowActivationState = e.WindowActivationState;
        }

        private async void WtSocketClient_OnMessageReceived(JObject obj)
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
                await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => Sessions.Insert(index, newSession)));
            }
            else if (MessageViewModel.Session != session)
            {
                int index = Sessions.IndexOf(session);
                await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    session.LatestMessageAt = msg.CreatedAt;
                    session.UnRead++;
                    UnreadMessageCount++;
                    int newindex = GetNewIndex(session);
                    if (index != newindex)
                    {
                        Sessions.Move(index, newindex);
                        MessageViewModel.Highlight(session);
                    }
                }));
            }
            SendToast(msg);
        }

        private void SendToast(WtMessage.Message msg)
        {
            if (msg.From.Uid == User.Id)
            {
                return;
            }
            else if (MessageViewModel.Session != null && MessageViewModel.Session.Id == msg.To.Id && WindowActivationState == CoreWindowActivationState.Deactivated)
            {
                return;
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

        public async Task RequestTeamAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/team");
            Team = obj["data"].ToObject<Team>();
            TeamId = Team.Id;
            Logo = Box.LogoUrl + Team.Logo;
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
                RefType = 1
            };
        }

        private Session GetMemberSession(JObject obj)
        {
            return new Session
            {
                Id = obj.Value<string>("_id"),
                Avatar = obj["to"].Value<string>("avatar"),
                DisplayName = obj["to"].Value<string>("display_name"),
                UnRead = obj.Value<int>("unread"),
                IsStar = obj.Value<bool>("starred"),
                LatestMessageAt = DateTimeOffset.FromUnixTimeSeconds(obj.Value<long>("latest_message_at")),
                IsAAssistant = obj["to"].Value<int>("role") == (int)UserRoleType.Bot,
                Type = SessionType.Session,
                RefType = 2
            };
        }

        public async Task RequestChatsAsync()
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

        public int GetNewIndex(Session session)
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

        private static void UpdateBadgeNumber(int count)
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
    }
}