﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktile.Common;
using Worktile.Main.Models;
using Worktile.Models;

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
        }

        public string IMToken { get; private set; }

        public ObservableCollection<WtApp> Apps { get; }

        public static StorageBox Box { get; private set; }

        public static ObservableCollection<Session> Sessions { get; private set; }

        public ObservableCollection<User> Members { get; }

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
            User = obj["data"]["me"].ToObject<User>();
            Box = obj["data"]["config"]["box"].ToObject<StorageBox>();
            string img = obj["data"]["me"]["preferences"].Value<string>("background_image");
            if (img.StartsWith("desktop-") && img.EndsWith(".jpg"))
            {
                WtBackgroundImage = "ms-appx:///Assets/Images/Background/" + img;
            }
            else
            {
                WtBackgroundImage = Box.BaseUrl + "background-image/" + img + "/from-s3";
            }
        }

        public async Task RequestTeamAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/team");
            Team = obj["data"].ToObject<Team>();
            Logo = Box.LogoUrl + Team.Logo;
            var members = obj["data"]["members"].Children<JObject>();
            foreach (var item in members)
            {
                Members.Add(item.ToObject<User>());
            }
        }

        public async Task RequestChatsAsync()
        {
            var obj = await WtHttpClient.GetAsync("api/team/chats");
            var list = new List<Session>();

            var channels = obj["data"]["channels"].Children<JObject>();
            foreach (var item in channels)
            {
                list.Add(new Session
                {
                    Id = item.Value<string>("_id"),
                    DisplayName = item.Value<string>("name"),
                    UnRead = item.Value<int>("unread"),
                    IsStar = item.Value<bool>("starred"),
                    LatestMessageAt = DateTimeOffset.FromUnixTimeSeconds(item.Value<long>("latest_message_at")),
                    Type = SessionType.Channel
                });
            }

            var groups = obj["data"]["groups"].Children<JObject>();
            foreach (var item in groups)
            {
                list.Add(new Session
                {
                    Id = item.Value<string>("_id"),
                    DisplayName = item.Value<string>("name"),
                    UnRead = item.Value<int>("unread"),
                    IsStar = item.Value<bool>("starred"),
                    LatestMessageAt = DateTimeOffset.FromUnixTimeSeconds(item.Value<long>("latest_message_at")),
                    Type = SessionType.Group
                });
            }

            var sessions = obj["data"]["sessions"].Children<JObject>();
            foreach (var item in sessions)
            {
                list.Add(new Session
                {
                    Id = item.Value<string>("_id"),
                    Avatar = item["to"].Value<string>("avatar"),
                    DisplayName = item["to"].Value<string>("display_name"),
                    UnRead = item.Value<int>("unread"),
                    IsStar = item.Value<bool>("starred"),
                    LatestMessageAt = DateTimeOffset.FromUnixTimeSeconds(item.Value<long>("latest_message_at")),
                    Type = SessionType.Session
                });
            }

            list.Sort((a, b) =>
            {
                if (a.IsStar && !b.IsStar)
                    return -1;
                if (!a.IsStar && b.IsStar)
                    return 1;
                if (a.LatestMessageAt > b.LatestMessageAt)
                    return -1;
                if (a.LatestMessageAt < b.LatestMessageAt)
                    return 1;
                if (a.UnRead > 0 && b.UnRead <= 0)
                    return -1;
                if (a.UnRead <= 0 && b.UnRead > 0)
                    return 1;
                return a.DisplayName.CompareTo(b.DisplayName);
            });
            foreach (var item in list)
            {
                Sessions.Add(item);
            }
        }
    }
}
