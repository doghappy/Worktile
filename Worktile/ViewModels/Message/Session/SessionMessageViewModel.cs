using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Worktile.Views.Message;
using Newtonsoft.Json.Linq;
using Worktile.ApiModels.Message.ApiMessages;
using Worktile.Common;
using Worktile.Enums;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.IO;
using Windows.Storage;
using System.Collections.Generic;
using Worktile.Common.WtRequestClient;
using Newtonsoft.Json;
using Worktile.ApiModels;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Worktile.ViewModels.Message.Session
{
    class SessionMessageViewModel : MessageViewModel, INotifyPropertyChanged
    {
        public SessionMessageViewModel(Views.Message.Session session) : base(session) { }

        public event PropertyChangedEventHandler PropertyChanged;

        string _latestId;

        protected override string Url => $"/api/messages?ref_id={Session.Id}&ref_type={Session.RefType}&latest_id={_latestId}&size=20";

        protected override void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected override void ReadMessage(JToken jToken)
        {
            var apiData = jToken.ToObject<ApiMessages>();
            bool flag = false;
            if (Messages.Any())
            {
                apiData.Data.Messages.Reverse();
                flag = true;
            }
            foreach (var item in apiData.Data.Messages)
            {
                var msg = new Views.Message.Message
                {
                    Id = item.Id,
                    Avatar = new TethysAvatar
                    {
                        DisplayName = item.From.DisplayName,
                        Source = AvatarHelper.GetAvatarBitmap(item.From.Avatar, AvatarSize.X80, item.From.Type),
                        Foreground = new SolidColorBrush(Colors.White)
                    },
                    Content = MessageHelper.GetContent(item),
                    Time = item.CreatedAt,
                    Type = item.Type,
                    IsPinned = item.IsPinned
                };
                if (Path.GetExtension(item.From.Avatar).ToLower() == ".png")
                    msg.Avatar.Background = new SolidColorBrush(Colors.White);
                else
                    msg.Avatar.Background = AvatarHelper.GetColorBrush(item.From.DisplayName);
                if (flag)
                    Messages.Insert(0, msg);
                else
                    Messages.Add(msg);
            }
            _latestId = apiData.Data.LatestId;
            HasMore = apiData.Data.More;
        }

        public async override Task PinMessageAsync(Views.Message.Message msg)
        {
            string url = "/api/pinneds";
            var client = new WtHttpClient();
            var req = new
            {
                type = 1,
                message_id = msg.Id,
                session_id = Session.Id
            };
            string json = JsonConvert.SerializeObject(req);
            if (Session.Type == SessionType.Channel)
                json = json.Replace("session_id", "channel_id");
            var response = await client.PostAsync<ApiResponse>(url, json);
            if (response.Code == 200)
            {
                msg.IsPinned = true;
            }
        }

        public async override Task UnPinMessageAsync(Views.Message.Message msg)
        {
            string idType = Session.Type == SessionType.Channel ? "channel_id" : "session_id";
            string url = $"/api/messages/{msg.Id}/unpinned?{idType}={Session.Id}";
            var client = new WtHttpClient();
            var response = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                msg.IsPinned = false;
            }
        }

        public async Task SendMessageAsync(string msg)
        {
            int toType = 1;
            if (Session.Type == SessionType.Session)
                toType = 2;
            var data = new
            {
                fromType = 1,
                from = DataSource.ApiUserMeData.Me.Uid,
                to = Session.Id,
                toType,
                messageType = 1,
                client = 1,
                markdown = 1,
                content = msg
            };
            //await _navParam.MainPage.SendMessageAsync(SocketMessageType.Message, data);
            //MsgTextBox.Text = string.Empty;
        }

        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            var fileViewModel = new FileViewModel(Session);
            await fileViewModel.UploadFileAsync(files);
        }
    }
}
