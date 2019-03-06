using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Worktile.Models.Message.Session;
using Worktile.Common;
using Worktile.ApiModels.Message.ApiMessages;
using System.Linq;
using Worktile.Domain.SocketMessageConverter;

namespace Worktile.ViewModels.Message.Detail.Content
{
    abstract class SessionMessageViewModel<S> : BaseSessionMessageViewModel<S> where S : ISession
    {
        public SessionMessageViewModel(S session, MainViewModel mainViewModel) : base(session, mainViewModel) { }

        string _latestId;

        protected override string Url => $"/api/messages?ref_id={Session.Id}&ref_type={RefType}&latest_id={_latestId}&size=20";
        protected virtual int ToType { get; }

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

        public async override Task<bool> PinAsync(Models.Message.Message msg)
        {
            bool result = await base.PinAsync(msg);
            if (result)
            {
                msg.IsPinned = true;
            }
            return result;
        }

        public async override Task<bool> UnPinAsync(Models.Message.Message msg)
        {
            bool result = await base.UnPinAsync(msg);
            if (result)
            {
                msg.IsPinned = false;
            }
            return result;
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
    }
}
