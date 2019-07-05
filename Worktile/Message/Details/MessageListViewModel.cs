using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WtMessage = Worktile.Message.Models;
using Worktile.Common;
using Worktile.Models;
using Newtonsoft.Json;

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
    }
}
