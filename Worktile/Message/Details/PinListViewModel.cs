using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WtMessage = Worktile.Message.Models;
using Worktile.Common;
using Worktile.Models;
using Worktile.Main;

namespace Worktile.Message.Details
{
    public class PinListViewModel : BindableBase
    {
        public PinListViewModel()
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

        string _anchor = null;

        public async Task LoadMessagesAsync()
        {
            if (HasMore)
            {
                IsActive = true;
                string idType = Session.Type == SessionType.Session ? "session_id" : "channel_id";
                string url = $"api/pinneds?{idType}={Session.Id}&anchor={_anchor}&size=20";
                var obj = await WtHttpClient.GetAsync(url);
                var msgs = obj["data"]["pinneds"].Children<JObject>();
                foreach (var item in msgs)
                {
                    var msg = item["reference"].ToObject<WtMessage.Message>();
                    if (msg.From.Type == WtMessage.FromType.Service)
                    {
                        var service = MainViewModel.Services.Single(u => u.Id == msg.From.Uid);
                        msg.From.Avatar = service.Avatar;
                        msg.From.DisplayName = service.DisplayName;
                    }
                    else
                    {
                        var member = MainViewModel.Members.Single(u => u.Id == msg.From.Uid);
                        msg.From.Avatar = member.Avatar;
                        msg.From.DisplayName = member.DisplayName;
                    }
                    Messages.Add(msg);
                }
                if (Messages.Count > 0)
                {
                    _anchor = Messages[Messages.Count - 1].Id;
                }
                if (msgs.Count() < 20)
                {
                    HasMore = false;
                }
                IsActive = false;
            }
        }
    }
}
