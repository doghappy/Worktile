using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WtMessage = Worktile.Message.Models;
using Worktile.Common;
using Worktile.Models;

namespace Worktile.Message.Details
{
    public class ReadListViewModel : BindableBase
    {
        public ReadListViewModel()
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

        string _next = null;

        public async Task LoadMessagesAsync()
        {
            if (HasMore)
            {
                IsActive = true;
                string url = $"api/pigeon/messages?ref_id={Session.Id}&ref_type=2&filter_type=4&size=20&next={_next}";
                var obj = await WtHttpClient.GetAsync(url);
                var data = obj["data"] as JObject;
                if (data.ContainsKey("next") && data.Value<string>("next") != string.Empty)
                {
                    _next = data.Value<string>("next");
                }
                else
                {
                    HasMore = false;
                }
                var msgs = data["messages"].Children<JObject>().OrderByDescending(g => g.Value<double>("created_at"));
                foreach (var item in msgs)
                {
                    Messages.Insert(0, item.ToObject<WtMessage.Message>());
                }
                IsActive = false;
            }
        }

        public async Task PinAsync(WtMessage.Message msg)
        {
            string url = $"api/unreads/{Session.Id}/messages/{msg.Id}/pending";
            var obj = await WtHttpClient.PostAsync(url);
            if (obj.Value<int>("code") == 200 && obj.Value<bool>("data"))
            {
                msg.IsPending = true;
            }
        }

        public async Task UnPinAsync(WtMessage.Message msg)
        {
            string url = $"api/unreads/{Session.Id}/messages/{msg.Id}/pending";
            var obj = await WtHttpClient.DeleteAsync(url);
            if (obj.Value<int>("code") == 200 && obj.Value<bool>("data"))
            {
                msg.IsPending = false;
            }
        }
    }
}
