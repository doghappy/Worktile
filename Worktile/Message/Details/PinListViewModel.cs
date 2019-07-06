using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using WtMessage = Worktile.Message.Models;
using Worktile.Common;
using Worktile.Models;
using Microsoft.Toolkit.Collections;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Toolkit.Uwp;

namespace Worktile.Message.Details
{
    public class PinListViewModel : BindableBase
    {
        public IncrementalLoadingCollection<PinMessageSource, WtMessage.Message> Messages { get; private set; }

        Session _session;
        public Session Session
        {
            get => _session;
            set
            {
                _session = value;
                Messages = new IncrementalLoadingCollection<PinMessageSource, WtMessage.Message>(new PinMessageSource(value));
            }
        }

        public async Task UnPinAsync(WtMessage.Message msg)
        {
            string idType = Session.Type == SessionType.Session ? "session_id" : "channel_id";
            string url = $"api/messages/{msg.Id}/unpinned?{idType}={Session.Id}";
            var obj = await WtHttpClient.DeleteAsync(url);
            if (obj.Value<int>("code") == 200 && obj.Value<bool>("data"))
            {
                Messages.Remove(msg);
            }
        }
    }


    public class PinMessageSource : IIncrementalSource<WtMessage.Message>
    {
        public PinMessageSource(Session session)
        {
            _session = session;
        }

        readonly Session _session;
        string _anchor = null;

        public async Task<IEnumerable<WtMessage.Message>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            string idType = _session.Type == SessionType.Session ? "session_id" : "channel_id";
            string url = $"api/pinneds?{idType}={_session.Id}&anchor={_anchor}&size=20";
            var obj = await WtHttpClient.GetAsync(url);
            var msgs = obj["data"]["pinneds"].Children<JObject>();
            var list = new List<WtMessage.Message>();
            foreach (var item in msgs)
            {
                var msg = item["reference"].ToObject<WtMessage.Message>();
                msg.CompleteMessageFrom();
                list.Add(msg);
            }
            if (list.Count > 0)
            {
                _anchor = list[list.Count - 1].Id;
            }
            return list;
        }
    }
}
