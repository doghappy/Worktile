﻿using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WtMessage = Worktile.Message.Models;
using Worktile.Common;
using Worktile.Models;

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
                _latestId = obj["data"].Value<string>("next");
                var msgs = obj["data"]["messages"].Children<JObject>().OrderByDescending(g => g.Value<double>("created_at"));
                foreach (var item in msgs)
                {
                    Messages.Insert(0, item.ToObject<WtMessage.Message>());
                }
                IsActive = false;
            }
        }
    }
}
