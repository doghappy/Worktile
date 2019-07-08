using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.Message;
using Worktile.Models;
using Worktile.Models.Exceptions;
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

        //private bool _imageViewerIsActive;
        //public bool ImageViewerIsActive
        //{
        //    get => _imageViewerIsActive;
        //    set => SetProperty(ref _imageViewerIsActive, value);
        //}

        //private List<string> _imageViewerImages;
        //public List<string> ImageViewerImages
        //{
        //    get => _imageViewerImages;
        //    set => SetProperty(ref _imageViewerImages, value);
        //}

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

                WtSocketClient.OnMessageReceived += WtSocketClient_OnMessageReceived;
                WtSocketClient.Connect(imHost, imToken, User.Id);
            }
            else
            {
                throw new WtNotLoggedInException();
            }
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
            else
            {
                //if (session != MessageViewModel.SelectedSession)
                //{
                int index = Sessions.IndexOf(session);
                await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    session.LatestMessageAt = msg.CreatedAt;
                    session.UnRead++;
                    int newindex = GetNewIndex(session);
                    if (index != newindex)
                    {
                        Sessions.Move(index, newindex);
                        MessageViewModel.Highlight(session);
                    }
                }));
                //}
            }
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
    }
}