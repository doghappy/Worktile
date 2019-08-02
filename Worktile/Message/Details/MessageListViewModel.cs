using System;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WtMessage = Worktile.Message.Models;
using Worktile.Common;
using Worktile.Models;
using Newtonsoft.Json;
using Microsoft.Toolkit.Uwp.Helpers;
using System.Collections.Generic;
using Windows.Storage;
using Worktile.Main;
using Windows.Storage.AccessCache;
using Windows.Web.Http;

namespace Worktile.Message.Details
{
    public class MessageListViewModel : BindableBase
    {
        public MessageListViewModel()
        {
            Messages = new ObservableCollection<WtMessage.Message>();
            HasMore = true;
        }

        public ObservableCollection<WtMessage.Message> Messages { get; }

        public bool HasMore { get; private set; }

        public Session Session { get; set; }

        private bool _isActive;
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        string _latestId = null;

        public async Task LoadMessagesAsync()
        {
            if (HasMore)
            {
                IsActive = true;
                string url = $"api/messages?ref_id={Session.Id}&ref_type={Session.RefType}&latest_id={_latestId}&size=20";
                var obj = await WtHttpClient.GetAsync(url);
                if (_latestId != null)
                {
                    HasMore = obj["data"].Value<bool>("more");
                }
                _latestId = obj["data"].Value<string>("latest_id");
                var msgs = obj["data"]["messages"].Children<JObject>().OrderByDescending(g => g.Value<double>("created_at"));
                foreach (var item in msgs)
                {
                    Messages.Insert(0, item.ToObject<WtMessage.Message>());
                }
                IsActive = false;
            }
        }

        public async Task PinAsync(WtMessage.Message msg)
        {
            string idType = Session.Type == SessionType.Session ? "session_id" : "channel_id";
            var req = new
            {
                type = 1,
                message_id = msg.Id,
                worktile = Session.Id
            };
            string json = JsonConvert.SerializeObject(req);
            json = json.Replace("worktile", idType);
            var obj = await WtHttpClient.PostAsync("api/pinneds", json);
            if (obj.Value<int>("code") == 200)
            {
                msg.IsPinned = true;
            }
        }

        public async Task UnPinAsync(WtMessage.Message msg)
        {
            string idType = Session.Type == SessionType.Session ? "session_id" : "channel_id";
            string url = $"api/messages/{msg.Id}/unpinned?{idType}={Session.Id}";
            var obj = await WtHttpClient.DeleteAsync(url);
            if (obj.Value<int>("code") == 200 && obj.Value<bool>("data"))
            {
                msg.IsPinned = false;
            }
        }

        public async void OnMessageReceived(JObject obj)
        {
            var msg = obj.ToObject<WtMessage.Message>();
            if (msg.To.Id == Session.Id)
            {
                msg.CompleteMessageFrom();
                await Task.Run(async () => await DispatcherHelper.ExecuteOnUIThreadAsync(() => Messages.Add(msg)));
            }
        }

        public async Task SendMessageAsync(string msg)
        {
            string to = Session.Id;
            WtMessage.ToType toType = WtMessage.ToType.Channel;
            if (Session.Type == SessionType.Session)
            {
                toType = WtMessage.ToType.Session;
            }
            string message = msg.Trim();
            if (message != string.Empty)
            {
                await WtSocketClient.SendMessageAsync(to, toType, message, WtMessage.MessageType.Text);
            }
        }

        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            if (files.Any())
            {
                string refType = Session.Type == SessionType.Session ? "2" : "1";
                string url = $"{MainViewModel.Box.BaseUrl}/entities/upload?team_id={MainViewModel.TeamId}&ref_id={Session.Id}&ref_type={refType}";
                foreach (var file in files)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", file);
                    using (var stream = await file.OpenReadAsync())
                    {
                        string fileName = file.DisplayName + file.FileType;
                        var content = new HttpMultipartFormDataContent
                        {
                            { new HttpStringContent(fileName), "name" },
                            { new HttpStreamContent(stream), "file", fileName }
                        };
                        await WtHttpClient.PostAsync(url, content);
                    }
                }
            }
        }
    }
}
