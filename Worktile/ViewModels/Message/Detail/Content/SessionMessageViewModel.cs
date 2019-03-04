using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Worktile.ApiModels.Message.ApiMessages;
using Worktile.Common;
using Windows.Storage;
using System.Collections.Generic;
using Worktile.Common.WtRequestClient;
using Worktile.ApiModels;
using Worktile.Domain.SocketMessageConverter;
using Worktile.Models.Message.Session;
using Newtonsoft.Json;

namespace Worktile.ViewModels.Message.Detail.Content
{
    abstract class SessionMessageViewModel<S> : MessageViewModel<S>
          where S : ISession
    {
        public SessionMessageViewModel(S session, MainViewModel mainViewModel) : base(session, mainViewModel) { }

        string _latestId;

        protected override string Url => $"/api/messages?ref_id={Session.Id}&ref_type={RefType}&latest_id={_latestId}&size=20";

        protected abstract string IdType { get; }
        protected abstract int ToType { get; }

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
                item.ContentFormat();
                MessageHelper.SetAvatar(item);
                if (flag)
                    Messages.Insert(0, item);
                else
                    Messages.Add(item);
            }
            _latestId = apiData.Data.LatestId;
            HasMore = apiData.Data.More;
        }

        public async override Task PinMessageAsync(Models.Message.Message msg)
        {
            string url = "/api/pinneds";
            var client = new WtHttpClient();
            var req = new
            {
                type = 1,
                message_id = msg.Id,
                worktile = Session.Id
            };
            string json = JsonConvert.SerializeObject(req);
            json = json.Replace("worktile", IdType);
            var response = await client.PostAsync<ApiResponse>(url, json);
            if (response.Code == 200)
            {
                msg.IsPinned = true;
            }
        }

        public async override Task UnPinMessageAsync(Models.Message.Message msg)
        {
            string url = $"/api/messages/{msg.Id}/unpinned?{IdType}={Session.Id}";
            var client = new WtHttpClient();
            var response = await client.DeleteAsync<ApiDataResponse<bool>>(url);
            if (response.Code == 200 && response.Data)
            {
                msg.IsPinned = false;
            }
        }

        public async Task<bool> SendMessageAsync(string msg)
        {
            var data = new
            {
                fromType = 1,
                from = DataSource.ApiUserMeData.Me.Uid,
                to = Session.Id,
                toType = ToType,
                messageType = 1,
                client = 1,
                markdown = 1,
                content = msg
            };
            return await MainViewModel.SendMessageAsync(SocketMessageType.Message, data);
        }

        public async Task UploadFileAsync(IReadOnlyList<StorageFile> files)
        {
            var fileViewModel = new FileViewModel(Session);
            await fileViewModel.UploadFileAsync(files);
        }
    }
}
